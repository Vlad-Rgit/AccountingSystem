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
    public class CorpusesViewModel : ViewModelBase
    {

        public bool IsClosed { get; set; } = true;

        public bool IsToEdit { get; }

        public ObservableCollection<CorpusView> CorpusViews { get; }

        private string _name =  String.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; Filter(); }
        }


        private string _address = String.Empty;

        public string Address
        {
            get { return _address; }
            set { _address = value;  Filter(); }
        }


        private bool _isPopup;

        public bool IsPopup
        {
            get { return _isPopup; }
            set { _isPopup = value; OnPropertyChanged(); }
        }


        private CorpusView _selectedCorpus;

        public CorpusView SelectedCorpus
        {
            get { return _selectedCorpus; }
            set {
                IsPopup = false; 
                _selectedCorpus = value;

                OnPropertyChanged();

                IsPopup = true; }
        }


        public RelayCommand<CorpusView> OpenAddCorpusCommand { get; }
        public RelayCommand<CorpusView> DeleteCommand { get; }

        public CorpusesViewModel(bool isToEdit)
        {
            IsToEdit = isToEdit;

            using (var repo = new Repository())
                CorpusViews = new ObservableCollection<CorpusView>(repo.GetAll<CorpusView>());


            OpenAddCorpusCommand = new RelayCommand<CorpusView>(async (x) =>
            {
                IsClosed = false;

                var userControl = new AddCorpusUserControl(x);

                await DialogHost.Show(userControl, "corpusHost");

        
                Filter();
                IsPopup = false;
                IsClosed = true;

            });


            DeleteCommand = new RelayCommand<CorpusView>((c) =>
            {
                CustomMessageBox acceptBox =
                     new CustomMessageBox("Удаление", "Вы действительно хотите удалить этот корпус", true);

                if (acceptBox.ShowDialog() == false)
                    return;
                

                using(var repo = new Repository())
                {
                    repo.Delete<Corpus>(c.Id);

                    if(repo.SaveChanges() == (int)Repository.Error.Validation)
                    {
                        CustomMessageBox message = 
                             new CustomMessageBox("Ошибка", "Корпус имеет привязанные сущности.\nСначала удалите их");

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
            CorpusViews.Clear();

            using(var repo = new Repository())
            {
                foreach(var c in repo.GetAll<CorpusView>())
                {
                    if (c.Название.Contains(Name) && c.Адрес.Contains(Address))
                    {
                        CorpusViews.Add(c);
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
