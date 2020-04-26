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

namespace AccountingSystem.ViewModels
{
    public class GroupsViewModel : ViewModelBase
    {
        public bool IsToEdit { get; }

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

        private bool _isPopup;

        public bool IsPopup
        {
            get => _isPopup;
            set
            {
                _isPopup = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<GroupView> GroupViews { get; }

        private GroupView _selectedGroupView;

        public GroupView SelectedGroupView
        {
            get { return _selectedGroupView; }
            set 
            { 
                IsPopup = false;
                _selectedGroupView = value; 
                IsPopup = true;
                OnPropertyChanged();
            }
        }



        public List<string> Corpuses { get; }

        private string _selectedCorpus;

        public string SelectedCorpus
        {
            get { return _selectedCorpus; }
            set { _selectedCorpus = value; Filter(); }
        }

        private string _groupName = String.Empty;

        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; Filter(); }
        }


        public RelayCommand<GroupView> OpenAddGroupCommand { get; }
        public RelayCommand<GroupView> DeleteGroupCommand { get; }
        public RelayCommand<GroupView> OpenStudentsCommand { get; }

        public GroupsViewModel(bool _isToEdit)
        {
            IsToEdit = _isToEdit;

            using (var repo = new Repository())
            {
                GroupViews = new ObservableCollection<GroupView>(repo.GetAll<GroupView>());

                Corpuses = new List<string>();
                Corpuses.Add("Все отделения");
                Corpuses.AddRange(repo.GetAll<Corpus>().Select(c => c.CorpusName));

                SelectedCorpus = Corpuses[0];
               
            }

            OpenAddGroupCommand = new RelayCommand<GroupView>(async (g)=> {

                IsClosed = false;

                var userControl = new AddGroupUserControl(g);

                await DialogHost.Show(userControl, "groupHost");

                Filter();
                IsPopup = false;
                IsClosed = true;
            });

            DeleteGroupCommand = new RelayCommand<GroupView>((g) =>
            {
                CustomMessageBox acceptBox = new CustomMessageBox("Удаление", "Вы действительно хотите удалить эту группу", true);

                if (acceptBox.ShowDialog() == false)
                    return;


                using(var repo = new Repository())
                {
                    Group group = repo.Get<Group>(g.Id);

                    repo.Delete(group);

                    if(repo.SaveChanges() == (int)Repository.Error.Validation)
                    {
                        CustomMessageBox messageBox = new CustomMessageBox("Ошибка", "Эта группа содержит привязанные сущности\n" +
                            "Удалите сначала их");

                        messageBox.ShowDialog();
                    }

                    Filter();
                    IsPopup = false;
                }
            });
        }

        public void Filter()
        {
            GroupViews.Clear();

            using (var repo = new Repository()) 
            {
                foreach (var g in repo.GetAll<GroupView>())
                {
                    if(g.Группа.Contains(GroupName) 
                        && (SelectedCorpus == "Все отделения" || g.Отделение.Contains(SelectedCorpus)))
                    {
                        GroupViews.Add(g);
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
