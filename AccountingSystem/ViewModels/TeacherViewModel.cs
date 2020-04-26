using AccountingSystem.Commands;
using AccountingSystem.Models;
using AccountingSystem.Utilities;
using AccountingSystem.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AccountingSystem.ViewModels
{
    public class TeacherViewModel : ViewModelBase
    {
        private Timer _timeTimer { get; set; }

        public TeacherView Teacher { get; set; }
        public List<ScheduleView> TeacherSchedule { get; set; }
    

        private ScheduleView _currentLesson;
        public ScheduleView CurrentLesson
        {
            get => _currentLesson;
            set
            {
                _currentLesson = value;
                OnPropertyChanged();
            }
        }

        private DateTime _currentTime;
        public DateTime CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand CurrentLessonCommand { get; }


        public TeacherViewModel(TeacherView teacher)
        { 
            Teacher = teacher;
            CurrentTime = NtpHelper.GetNtpTime();
            _timeTimer = new Timer(UpdateTime, null, 1000, 1000);

            TeacherSchedule = new List<ScheduleView>(_context.ScheduleViews.Where(s => (DayOfWeek)s.День_недели == DayOfWeek.Tuesday
                                                                                       && s.Фио_преподователя == Teacher.Фио).OrderBy(s=>s.Номер_пары).ThenBy(s=>s.Знаменатель));

            CurrentLessonCommand = new RelayCommand(async () =>
            {
                await DialogHost.Show(new CurrentLessonUserControl(CurrentLesson));
            }, ()=>CurrentLesson!=null);

            UpdateCurrentLesson();

        }
       

        public void UpdateTime(object state)
        {
            CurrentTime = NtpHelper.GetNtpTime();

            UpdateCurrentLesson();
        }

        public void UpdateCurrentLesson()
        {

            CurrentLesson = TeacherSchedule.FirstOrDefault(s => s.Начало_пары <= CurrentTime.TimeOfDay
                                                     && s.Конец_пары >= CurrentTime.TimeOfDay);
        }



    }
}
