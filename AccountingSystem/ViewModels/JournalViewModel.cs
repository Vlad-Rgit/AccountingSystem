using AccountingSystem.Commands;
using AccountingSystem.Models;
using AccountingSystem.Repositories;
using AccountingSystem.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AccountingSystem.ViewModels
{
    public class JournalViewModel : ViewModelBase
    {
        public class JournalRecord
        {
           public Student Student { get; set; }
           public List<StudentStatusOnLesson> Statuses { get; set; }
        }

        public List<Group> Groups { get; }

        private Group _selectedGroup;

        public Group SelectedGroup
        {
            get { return _selectedGroup; }
            set { _selectedGroup = value; Filter(); }
        }


        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; Filter(); }
        }

        public List<string> Statuses { get; }

        public List<Subject> Subjects { get; }

        private Subject _selectedSubject;

        public Subject SelectedSubject
        {
            get { return _selectedSubject; }
            set { _selectedSubject = value; Filter(); }
        }

        public ObservableCollection<JournalRecord> JournalRecords { get; }



        public RelayCommand AcceptCommand { get; }

        public JournalViewModel()
        {
            JournalRecords = new ObservableCollection<JournalRecord>();


            Statuses = new List<string>()
            {
                "На паре","П","З","Б"
            };

            using (var repo = new Repository())
            {

                Groups = new List<Group>(repo.GetAll<Group>());
                _selectedGroup = Groups[0];

                Subjects = new List<Subject>(repo.GetAll<Subject>());

                SelectedSubject = Subjects[0];

                DateTime now = DateTime.Now;

                while(now.DayOfWeek != DayOfWeek.Monday)
                {
                    now = now.AddDays(-1);
                }

                _selectedDate = now;
            }

            AcceptCommand = new RelayCommand(Accept);

            Filter();
        }


        public void Filter()
        {
            JournalRecords.Clear();

            using (var repo = new Repository()) {

                var records = repo.GetAllWhere<StudentStatusOnLesson>(s => s.Date >= SelectedDate && s.Date <= SelectedDate.AddDays(6)
                                                                          && s.Student.Group.GroupName == SelectedGroup.GroupName 
                                                                          && s.ScheduleRecord.Subject.SubjectName == SelectedSubject.SubjectName);


                foreach (var r in records)
                {
                    JournalRecord journalRecord = JournalRecords.FirstOrDefault(j => j.Student.StudentId == r.StudentId);

                    if (journalRecord == null)
                    {
                        journalRecord = new JournalRecord();
                        journalRecord.Student = r.Student;

                        //Заполнить статусы пустыми занчениями
                        journalRecord.Statuses =
                            new List<StudentStatusOnLesson>
                            (Enumerable.Repeat<StudentStatusOnLesson>(null, 6));

                    }

                    //Добавить статус в нужную ячейку
                    journalRecord.Statuses.Insert(r.ScheduleRecord.DayOfWeek-1, r);

                    JournalRecords.Add(journalRecord);
                }

            }
        }

        public void Accept()
        {
            using (var repo = new Repository())
            {
                foreach (var j in JournalRecords)
                {
                    foreach (var s in j.Statuses)
                    {
                        if (s == null)
                            continue;

                        if (s.StudentStatusId == 0)
                            s.StudentStatusId = null;

                        repo.Update(s);
                    }
                }

                repo.SaveChanges();
            }

            Biscuit.ShowBiscuit("journalBiscuit");
        }


        public override void Close()
        {
            DialogHost.CloseDialogCommand.Execute(this, null);
        }
    }
}
