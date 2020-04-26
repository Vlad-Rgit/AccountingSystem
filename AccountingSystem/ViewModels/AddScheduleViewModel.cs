using AccountingSystem.Commands;
using AccountingSystem.Converters;
using AccountingSystem.Models;
using AccountingSystem.Repositories;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.ViewModels
{
    public class AddScheduleViewModel : ViewModelBase
    {
        private bool _isToUpdate = false;


        public ScheduleRecord ScheduleRecord { get; set; }
        
        public List<Group> Groups { get; set; }

        public List<TeacherInfo> TeacherViews { get; set; }

        private TeacherInfo _selectedTeacher;
        public TeacherInfo SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                _selectedTeacher = value;
                HallNumber = _selectedTeacher.HallNumber.Value;
            }
        }


        private int _hallNumber;
        public int HallNumber
        {
            get => _hallNumber;
            set
            {
                _hallNumber = value;
                ScheduleRecord.HallNumber = _hallNumber;
                OnPropertyChanged();
            }
        }

        public int SelectedDay { get; set; }
        public List<string> Days { get; set; }

        public int SelectedLessonFrequency { get; set; }
        public List<string> LessonFrequencies { get; set; }

        public List<Subject> Subjects { get; set; }

        public List<int> LessonNumbres { get; set; }

        public RelayCommand AcceptCommand { get; }
        public RelayCommand CancelCommand { get; }


        private AddScheduleViewModel()
        {
            AcceptCommand = new RelayCommand(Accept);
            CancelCommand = new RelayCommand(() => DialogHost.CloseDialogCommand.Execute(this, null));

            TeacherViews = new List<TeacherInfo>(_context.TeacherInfoes.ToList());

            LessonNumbres = new List<int>(Enumerable.Range(1, 4));
            LessonFrequencies = new List<string>() { "Каждую неделю", "Числитель", "Знаменатель" };

            Groups = new List<Group>(_context.Groups.ToList());

            Subjects = new List<Subject>(_context.Subjects);

            DayOfWeekConverter converter = new DayOfWeekConverter();

            Days = new List<string>();

            for (int i = 1; i <= 7; i++)
                Days.Add(converter.GetDayName(i));
        }

        public AddScheduleViewModel(string group) : this()
        {
   
            ScheduleRecord = new ScheduleRecord();
            ScheduleRecord.LessonNumber = 1;

            SelectedTeacher = TeacherViews[0];
            HallNumber = SelectedTeacher.HallNumber.Value;

            ScheduleRecord.Group = Groups.First(g => g.GroupName == group);
       
            SelectedLessonFrequency = 0;   

            SelectedDay = 0;
        }

        public AddScheduleViewModel(ScheduleView schedule) : this()
        {
            _isToUpdate = true;
            ScheduleRecord = _context.ScheduleRecords.Find(schedule.Id);

            SelectedTeacher = ScheduleRecord.TeacherInfo;
            HallNumber = schedule.HallNumber;

            SelectedDay = ScheduleRecord.DayOfWeek-1;

            if (ScheduleRecord.IsDenominator.HasValue)           
                SelectedLessonFrequency = ScheduleRecord.IsDenominator.Value == false ? 1 : 2;            
            else
                SelectedLessonFrequency = 0;
        }

        public void Accept()
        {
            ScheduleRecord.TeacherInfo = SelectedTeacher;

            if (SelectedLessonFrequency == 1)
                ScheduleRecord.IsDenominator = false;
            else if (SelectedLessonFrequency == 2)
                ScheduleRecord.IsDenominator = true;
            else
                ScheduleRecord.IsDenominator = null;

            ScheduleRecord.DayOfWeek = SelectedDay + 1;

            if (ScheduleRecord.HallNumber == ScheduleRecord.TeacherInfo.HallNumber)
                ScheduleRecord.HallNumber = null;

            if (_isToUpdate)
            {
                _context.Entry(ScheduleRecord).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                _context.ScheduleRecords.Add(ScheduleRecord);              
            }

            _context.SaveChanges();

            DialogHost.CloseDialogCommand.Execute(this, null);
        }
    }
}
