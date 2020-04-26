using AccountingSystem.Models;
using AccountingSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AccountingSystem.Commands;
using AccountingSystem.Views;
using MaterialDesignThemes.Wpf;

namespace AccountingSystem.ViewModels
{
    public class StudentsViewModel : ListViewModel<StudentView>
    {

        public List<string> Groups { get; }

        private string _selectedGroup;

        public string SelectedGroup
        {
            get { return _selectedGroup; }
            set { _selectedGroup = value; Filter(); }
        }

        private string _name = String.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; Filter(); }
        }



        public StudentsViewModel(bool isToEdit) : base(isToEdit)
        {
            using(var repo = new Repository())
            {

                Entities = new ObservableCollection<StudentView>(repo.GetAll<StudentView>());

                Groups = new List<string>();
                Groups.Add("Все группы");
                Groups.AddRange(repo.GetAll<Group>().Select(g=>g.GroupName));

                SelectedGroup = Groups[0];
            }

            OpenAddCommand = new RelayCommand<StudentView>(async (s) =>
            {
                IsClosed = false;
                IsPopup = false;

                var userControl = new AddStudentUserControl(s);
                await DialogHost.Show(userControl, "studentHost");

                Filter();

                IsClosed = true;
                IsPopup = false;
            });
        }



        public override void Filter()
        {
            Entities.Clear();

            using (var repo = new Repository())
            {

                foreach (var s in repo.GetAll<StudentView>())
                {
                    if((SelectedGroup == "Все группы" || s.Группа == SelectedGroup)
                        && (s.Фио.Contains(Name)))
                    {
                        Entities.Add(s);
                    }
                }


            }
        }
    }
}
