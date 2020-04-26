using AccountingSystem.Commands;
using AccountingSystem.Models;
using AccountingSystem.Repositories;
using AccountingSystem.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.ViewModels
{
    public class AddGroupViewModel : ViewModelBase
    {
        private bool _isToUpdate = false;

        public List<Spezialisation> Spezialisations { get; }
        public Spezialisation Spezialisation { get; set; }

        public List<Corpus> Corpuses { get; }
        public Corpus Corpus { get; set; }


        public List<string> Budgets { get; }
        public string Budget { get; set; }

        public Group Group { get; }

        public RelayCommand AcceptCommand { get; }


        public AddGroupViewModel(GroupView groupView)
        {
            using(var repo = new Repository())
            {
                Corpuses = new List<Corpus>(repo.GetAll<Corpus>());
                Spezialisations= new List<Spezialisation>(repo.GetAll<Spezialisation>());

                Budgets = new List<string>()
                {
                    "Бюджет","Вне бюджет"
                };

                Budget = Budgets[0];

                if(groupView == null)
                {
                    Group = new Group();
                    Spezialisation = Spezialisations[0];
                    Corpus = Corpuses[0];
                }
                else
                {
                    _isToUpdate = true;
                    Group = repo.Get<Group>(groupView.Id);
                    Spezialisation = Group.Spezialisation;
                    Corpus = Group.Corpus;
                }
            }


            AcceptCommand = new RelayCommand(Accept);
        }


        public void Accept()
        {
            using(var repo = new Repository())
            {

                if (_isToUpdate)
                {
                    repo.Attach(Group);

                    if(Group.ScheduleRecords.Any((s)=>s.Group.CorpusId != Corpus.CorpusId))
                    {
                        CustomMessageBox messageBox = new CustomMessageBox("Ошибка обновления отделения",
     $"Группа имеет пары в расписании в отделении {Group.Corpus.CorpusName}");

                        messageBox.ShowDialog();

                        return;
                    }
                }


                repo.Attach(Spezialisation);
                repo.Attach(Corpus);

                if (Budget == "Бюджет")
                    Group.IsBudget = true;
                else
                    Group.IsBudget = false;

                if (_isToUpdate)
                {  
                    Group.Spezialisation = Spezialisation;
                    Group.Corpus = Corpus;
                    repo.Update(Group);
                }
                else
                {
                    Group.Spezialisation = Spezialisation;
                    Group.Corpus = Corpus;
                    repo.Add(Group);
                }

                repo.SaveChanges();
                Close();
            }
        }

        public override void Close()
        {
            DialogHost.CloseDialogCommand.Execute(this, null);
        }

    }
}
