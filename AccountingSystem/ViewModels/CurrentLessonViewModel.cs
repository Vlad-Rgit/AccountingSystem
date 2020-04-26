using AccountingSystem.Commands;
using AccountingSystem.Models;
using AccountingSystem.Repositories;
using AccountingSystem.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.ViewModels
{
    public class CurrentLessonViewModel: ViewModelBase
    {

        public class StudentRecord
        {
            public Student Student { get; set;  }
            public int StatusIndex { get; set; }
        }


        public List<StudentRecord> StudentRecords { get; set; }
        public List<Student> Students { get; set; }
        public Group Group { get; set; }
        public List<string> Statuses { get; set; }
        public ScheduleView ScheduleView { get; set; }

        public RelayCommand AcceptCommand { get; }


       

        public CurrentLessonViewModel(ScheduleView schedule)
        {
            ScheduleView = schedule;
            using (var repo = new Repository())
            {
                Group = repo.GetWhere<ScheduleRecord>(s => s.ScheduleRecordId == schedule.Id)
                                                 .Group;

                Students = new List<Student>(Group.Students.OrderBy(s => s.Fio));

                StudentRecords = new List<StudentRecord>();

                foreach (var s in Students)
                {
                    StudentRecord studentRecord = new StudentRecord();
                    studentRecord.Student = s;
                  

                    int? status =  repo.GetWhere<StudentStatusOnLesson>((stu) => stu.ScheduleRecordId == schedule.Id && stu.StudentId == s.StudentId)
                        ?.StudentStatusId;


                    if (status.HasValue)
                        studentRecord.StatusIndex = status.Value;
                    else
                        studentRecord.StatusIndex = 0;
                            

                    StudentRecords.Add(studentRecord);
                }
     
            };

            Statuses = new List<string>()
            {
                "На паре", "П", "З", "Б"
            };

            AcceptCommand = new RelayCommand(Accept);
        }

        public void Accept()
        {
            using(var repo = new Repository())
            {

                ScheduleRecord record = repo.GetWhere<ScheduleRecord>((s) => s.ScheduleRecordId == ScheduleView.Id);

                foreach(var s in StudentRecords)
                {
                    bool isToUpdate = true;

                    Student stu = repo.Get<Student>(s.Student.StudentId);

                    StudentStatusOnLesson status = repo.GetWhere<StudentStatusOnLesson>((st) => st.StudentId == s.Student.StudentId
                                                                                && st.ScheduleRecordId == record.ScheduleRecordId);

                    if (status == null)
                    {
                        status = new StudentStatusOnLesson();
                        isToUpdate = false;
                    }
                   

                    status.ScheduleRecord = record;
                    status.Student = stu;
                    status.Date = DateTime.Now;

                    if (s.StatusIndex == 0)
                        status.StudentStatusId = null;
                    else
                        status.StudentStatusId = s.StatusIndex;

                    if (isToUpdate)
                        repo.Update(status);
                    else
                        repo.Add(status);
                }

                repo.SaveChanges();
            }

            Biscuit.ShowBiscuit("lessonBiscuit");
        }

        public override void Close()
        {
            DialogHost.CloseDialogCommand.Execute(this, null);
        }
    }
}
