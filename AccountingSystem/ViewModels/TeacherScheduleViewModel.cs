using AccountingSystem.Converters;
using AccountingSystem.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Group = AccountingSystem.Models.Group;

namespace AccountingSystem.ViewModels
{
    public class TeacherScheduleViewModel : BaseScheduleViewModel
    {

        #region Properties

        public List<string> Teachers { get; set; }

        private string _selectedTeacher;
        public string SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                _selectedTeacher = value;
                Filter();
            }
        }

        #endregion Properties

        public TeacherScheduleViewModel(TeacherView teacher)
        {
            Groups.Insert(0, Constants.AllGroups);

            //Инициализация Teachers
            Teachers = new List<string>(_context.TeacherViews.Select(t=>t.Фио));

            //Ставим напрямую поле,чтобы не вызывать здесь лишний раз фильтр
            if (teacher != null)
                _selectedTeacher = teacher.Фио;
            else
                _selectedTeacher = Teachers[0];

            _selectedGroup = Groups[0];

            //В свойстве вызывется фильтр, где первоначально заполняется ScheduleDays
            SelectedDay = 0;
        }



        public override void Filter()
        {
            base.Filter();

            foreach(var s in ScheduleDays.ToList())
            {
                foreach(var l in s.Lessons.ToList())
                {
                    if (l.Фио_преподователя != SelectedTeacher)
                        s.Lessons.Remove(l);
                }

                if (s.Lessons.Count == 0)
                    ScheduleDays.Remove(s);
            }
        }



    }

}

