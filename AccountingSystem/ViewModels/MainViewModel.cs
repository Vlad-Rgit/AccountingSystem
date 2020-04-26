using AccountingSystem.Commands;
using AccountingSystem.Models;
using AccountingSystem.Views;
using AccountingSystem.Commands;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace AccountingSystem.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private User _user;
        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();

                IsAdmin = _user.RoleId == 1 || _user.RoleId == 2;
                IsTeacher = _user.RoleId == 1 || _user.RoleId == 2 || _user.RoleId == 3;
            }
        }

        private Visibility adminVisibility;
        public Visibility AdminVisibility
        {
            get => adminVisibility;
            set
            {
                adminVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility teacherVisibility;
        public Visibility TeacherVisibility
        {
            get => teacherVisibility;
            set
            {
                teacherVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;

                if (_isAdmin)
                    AdminVisibility = Visibility.Visible;
                else
                    AdminVisibility = Visibility.Collapsed;

                OnPropertyChanged();
            }
        }

        private bool _isTeacher;
        public bool IsTeacher
        {
            get => _isTeacher;
            set
            {
                _isTeacher = value;

                if (_isTeacher)
                    TeacherVisibility = Visibility.Visible;
                else
                    TeacherVisibility = Visibility.Collapsed;

                OnPropertyChanged();
            }
        }

        public RelayCommand OpenScheduleCommand { get; }
        public RelayCommand<bool> OpenGroupScheduleCommand { get; }
        public RelayCommand<bool> OpenCorpusesCommand { get; }
        public RelayCommand<bool> OpenTeachersCommand { get; }
        public RelayCommand<bool> OpenGroupsCommand { get; }
        public RelayCommand OpenAddGroupCommand { get; }
        public RelayCommand<bool> OpenStudentsCommand { get; }
        public RelayCommand OpenJournalCommand { get; }


        public MainViewModel()
        {
            OpenScheduleCommand = new RelayCommand(OpenSchedule);
            OpenGroupScheduleCommand = new RelayCommand<bool>(OpenGroupSchedule);
            OpenCorpusesCommand = new RelayCommand<bool>(async (b) =>
            {
                var userControl = new CorpusesUserControl(b);

                await DialogHost.Show(userControl);
            });

            OpenTeachersCommand = new RelayCommand<bool>(async (b) =>
            {
                var userControl = new TeachersUserControl(b);

                await DialogHost.Show(userControl);
            });

            OpenGroupsCommand = new RelayCommand<bool>(async (b) =>
            {
                var userControl = new GroupsUserControl(b);

                await DialogHost.Show(userControl);

            });

            OpenAddGroupCommand = new RelayCommand(async () =>
           {
               var userControl = new AddGroupUserControl(null);
               await DialogHost.Show(userControl);
           });

            OpenStudentsCommand = new RelayCommand<bool>(async (b)=> {

                var userControl = new StudentsUserControl(b);

                await DialogHost.Show(userControl);

            });


            OpenJournalCommand = new RelayCommand(async () => {

                var userControl = new JournalUserControl(IsTeacher);

                await DialogHost.Show(userControl);

            });

        }

        public async void OpenSchedule()
        {
            var userControl = new ScheduleUserControl(_context.TeacherViews.FirstOrDefault(t=>t.Логин == User.Login
                                                                                                            && t.Пароль == User.Password));
            await DialogHost.Show(userControl);
        }

        public async void OpenGroupSchedule(bool isToEdit)
        {
            GroupScheduleUserControl userControl;

            userControl = new GroupScheduleUserControl(_context.Students
                                                      .FirstOrDefault(s=>s.UserId == User.UserId),
                                                      isToEdit);

            await DialogHost.Show(userControl);
        }
    }
}
