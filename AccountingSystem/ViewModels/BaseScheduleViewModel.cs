using AccountingSystem.Converters;
using AccountingSystem.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AccountingSystem.ViewModels
{
    public class BaseScheduleViewModel: ViewModelBase
    {
        //Класс представляющий уроки в определенном дне
        public class ScheduleDay
        {
            public DayOfWeek Day { get; set; }
            public ObservableCollection<ScheduleView> Lessons { get; set; }
        }

        public static class Constants
        {
            public const string AllGroups = "Все группы";
            public const string AllDays = "Все дни";
        }

        #region Properties

        //Коллекция ScheduleDay
        public ObservableCollection<ScheduleDay> ScheduleDays { get; set; }

        private bool _isPopupOpen;
        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                _isPopupOpen = value;
                OnPropertyChanged();
            }
        }

        protected bool _isToEdit;
        public Visibility EditButtonVisibility
        {
            get => _isToEdit == true ? Visibility.Visible : Visibility.Collapsed;
        }

        private ScheduleView _selectedScheduleRecord;
        public ScheduleView SelectedScheduleRecord
        {
            get => _selectedScheduleRecord;
            set
            {
                IsPopupOpen = false;
                _selectedScheduleRecord = value;

                if(_isToEdit)
                    IsPopupOpen = true;

                OnPropertyChanged();
            }
        }

        public List<string> Groups { get; set; }

        protected string _selectedGroup;
        public string SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                Filter();
            }
        }

        public List<string> Days { get; set; }

        protected int _selectedDay;
        public int SelectedDay
        {
            get => _selectedDay;
            set
            {
                _selectedDay = value;
                Filter();
            }
        }

        #endregion

        public BaseScheduleViewModel()
        {
            ScheduleDays = new ObservableCollection<ScheduleDay>();

            //Инициализация Groups
            Groups = new List<string>();
            Groups.AddRange(_context.Groups.Select(g => g.GroupName));

            //Инициализация Days
            Days = new List<string>();
            Days.Add(Constants.AllDays);

            DayOfWeekConverter converter = new DayOfWeekConverter();

            for (int i = 1; i <= 7; i++)
            {
                Days.Add(converter.GetDayName(i));
            }
        }


        public virtual void Filter()
        {
            _context.Dispose();
            _context = new Context();

            ScheduleDays.Clear();

            for (int i = 1; i <= 7; i++)
            {
                if (SelectedDay != 0 && SelectedDay != i)
                    continue;

                ScheduleDay day = new ScheduleDay() { Day = (DayOfWeek)i };

                //Подгрузить все расписание
                var temp = _context.ScheduleViews.Where(s => (DayOfWeek)s.День_недели == day.Day)
                                                 .OrderBy(s => s.Номер_пары)
                                                 .ThenBy(s=>  s.Знаменатель.Value)
                                                 .ToList();
                //Удалить неподходящие записи
                foreach (var s in temp.ToList())
                {
                    if (s.Группа != SelectedGroup && SelectedGroup != Constants.AllGroups)
                        temp.Remove(s);
                }

                //Добавить день если в нем есть пары
                if (temp.Count > 0)
                {
                    day.Lessons = new ObservableCollection<ScheduleView>(temp);
                    ScheduleDays.Add(day);
                }
            }
        }


        public override void Close()
        {
            DialogHost.CloseDialogCommand.Execute(this, null);
        }


    }
}
