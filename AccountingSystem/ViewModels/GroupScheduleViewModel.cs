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
using System.Windows;
using AccountingSystem.Repositories;

namespace AccountingSystem.ViewModels
{
    public class GroupScheduleViewModel : BaseScheduleViewModel
    {


        private bool _isClosed = true;
        public bool IsClosed
        {
            get => _isClosed;
            set
            {
                _isClosed = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand<string> OpenAddLessonCommand { get; }
        public RelayCommand<ScheduleView> DeleteScheduleCommand { get; }
        public RelayCommand EditLessonCommand { get; }

        public GroupScheduleViewModel(Student student, bool isToEdit = false)
        {
            _isToEdit = isToEdit;

            if (student == null)
                _selectedGroup = Groups[0];
            else
                _selectedGroup = student.Group.GroupName;

            SelectedDay = 0;

            OpenAddLessonCommand = new RelayCommand<string>(async (g) => {

                    IsClosed = false;

                    await DialogHost.Show(new AddScheduleUserControl(g), "scheduleHost",
                        (object o, DialogClosingEventArgs a)=>Filter());

                    IsClosed = true;             
                });

            EditLessonCommand = new RelayCommand(async () =>
             {
               IsClosed = false;
               IsPopupOpen = false;

               await DialogHost.Show(new AddScheduleUserControl(SelectedScheduleRecord), "scheduleHost",
                   (object o, DialogClosingEventArgs a) => Filter());

                 IsPopupOpen = false;
                 IsClosed = true;
            });


            DeleteScheduleCommand = new RelayCommand<ScheduleView>((s) =>
            {
                CustomMessageBox acceptBox = new CustomMessageBox("Удаление", "Вы действительно хотите удалить эту запись?", true);

                if (acceptBox.ShowDialog() == false)
                    return;

                using(var repo = new Repository())
                {
                    repo.Delete(repo.Get<ScheduleRecord>(s.Id));

                    if(repo.SaveChanges() == (int)Repository.Error.Validation)
                    {
                        CustomMessageBox messageBox = 
                        new CustomMessageBox("Ошибка", "Запись содержит связанные сущности.\nСначала удалите их");

                        messageBox.ShowDialog();
                    }
                }

                Filter();
                IsPopupOpen = false;

            });
        }

    }
}
