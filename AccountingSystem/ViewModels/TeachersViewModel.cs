using AccountingSystem.Commands;
using AccountingSystem.Models;
using AccountingSystem.Repositories;
using AccountingSystem.Views;
using AccountingSystem.Commands;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.ViewModels
{
    public class TeachersViewModel : ViewModelBase
    {

        public bool IsClosed { get; set; } = true;

        public bool IsToEdit { get; }

        private string _name = String.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; Filter(); }
        }


        private string _hallNumber = String.Empty;

        public string HallNumber
        {
            get { return _hallNumber; }
            set { _hallNumber = value; Filter(); }
        }

        public List<string> Corpuses { get; }

        private string _selectedCorpus;

        public string SelectedCorpus
        {
            get { return _selectedCorpus; }
            set { _selectedCorpus = value; Filter(); }
        }


        public ObservableCollection<TeacherView> TeacherViews { get; }

        private bool _isPopup;

        public bool IsPopup
        {
            get => _isPopup;
            set
            {
                if (IsToEdit)
                {
                    _isPopup = value;
                    OnPropertyChanged();
                }
            }
        }


        private TeacherView _selectedTeacher;

        public TeacherView SelectedTeacher
        {
            get { return _selectedTeacher; }
            set
            {
                IsPopup = false;
                _selectedTeacher = value;

                OnPropertyChanged();

                IsPopup = true;
            }
        }


        public RelayCommand<TeacherView> OpenAddTeacher { get; }
        public RelayCommand<TeacherView> DeleteCommand { get; }

        public TeachersViewModel(bool isToEdit)
        {
            IsToEdit = isToEdit;

            using (var repo = new Repository())
            {
                TeacherViews = new ObservableCollection<TeacherView>(repo.GetAll<TeacherView>());

                Corpuses = new List<string>();
                Corpuses.Add("Все отделения");
                Corpuses.AddRange(repo.GetAll<Corpus>().Select(c=>c.CorpusName));
                _selectedCorpus = Corpuses[0];
            }

            OpenAddTeacher = new RelayCommand<TeacherView>(async (x) =>
            {
                IsClosed = false;

                var userControl = new AddTeacherUserControl(x);

                await DialogHost.Show(userControl, "teacherHost");


                Filter();
                IsPopup = false;
                IsClosed = true;

            });


            DeleteCommand = new RelayCommand<TeacherView>((c) =>
            {
                CustomMessageBox acceptBox =
                     new CustomMessageBox("Удаление", "Вы действительно хотите удалить этого учителя?", true);

                if (acceptBox.ShowDialog() == false)
                    return;


                using (var repo = new Repository())
                {
                    repo.Delete<TeacherInfo>(c.Id);

                    if (repo.SaveChanges() == (int)Repository.Error.Validation)
                    {
                        CustomMessageBox message =
                             new CustomMessageBox("Ошибка", "Учитель имеет привязанные сущности.\nСначала удалите их");

                        message.ShowDialog();
                    }
                }


                Filter();
                IsPopup = false;
                IsClosed = true;
            });
        }

        public void Filter()
        {
            TeacherViews.Clear();

            using (var repo = new Repository())
            {
                foreach (var c in repo.GetAll<TeacherView>())
                {
                    if (c.Фио.Contains(Name) && (SelectedCorpus == "Все отделения" || c.Отделение.Contains(SelectedCorpus))
                                                && (!c.Кабинет.HasValue || c.Кабинет.Value.ToString().Contains(HallNumber)))  
                    {
                        TeacherViews.Add(c);
                        continue;
                    }
                }
            }

        }


        public override void Close()
        {
            DialogHost.CloseDialogCommand.Execute(this, null);
        }
    }
}
