using AccountingSystem.Models;
using AccountingSystem.Repositories;
using AccountingSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AccountingSystem.ViewModels
{
    public class StudentMainViewModel : ViewModelBase
    {
        private Timer _timer;

        public List<ScheduleView> StudentSchedule { get; }

        public Student Student { get; }

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


        public StudentMainViewModel(Student student)
        {
            Student = student;
            _timer = new Timer((o) => CurrentTime = NtpHelper.GetNtpTime(), null, 0, 1000);

            using(var repo = new Repository())
            {
                StudentSchedule = new List<ScheduleView>(repo.GetAllWhere<ScheduleView>((s) => s.Группа == Student.Group.GroupName 
                && s.День_недели == (int)CurrentTime.DayOfWeek).OrderBy(s => s.Номер_пары).ThenBy(s => s.Знаменатель));
            }
        }



    }
}
