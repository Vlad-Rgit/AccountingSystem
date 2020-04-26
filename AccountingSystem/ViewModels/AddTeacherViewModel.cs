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
    public class AddTeacherViewModel : ViewModelBase
    {
        private bool _isToEdit = false;

        public TeacherInfo TeacherInfo { get; }

        public List<Corpus> Corpuses { get; }
        public Corpus Corpus { get; set; }
      
        public List<Education> Educations { get; }
        public Education Education { get; set; }

        public string Hall { get; set; } = String.Empty;

        public List<Role> Roles { get; }
        public Role Role { get; set; }

        public User User { get; set; }

        public RelayCommand AcceptCommand { get; }
        public RelayCommand CancelCommand { get; }

        public AddTeacherViewModel(TeacherView teacher)
        {
            using (var repo = new Repository()) {

                Corpuses = new List<Corpus>(repo.GetAll<Corpus>());
                Educations = new List<Education>(repo.GetAll<Education>());
               
                Roles = new List<Role>()
                {
                    repo.Get<Role>(2), repo.Get<Role>(3)
                };

                if (teacher == null)
                {
                    TeacherInfo = new TeacherInfo();
                    TeacherInfo.DateBirth = new DateTime(1990, 2, 2);
                    Education = Educations[0];
                    Corpus = Corpuses[0];
                    User = new User();
                    Role = Roles[1];
                }
                else
                {
                    _isToEdit = true;

                    TeacherInfo = repo.Get<TeacherInfo>(teacher.Id);
                    Corpus = TeacherInfo.Corpus;
                    Education = TeacherInfo.Education;
                    User = repo.Get<User>(TeacherInfo.UserId);
                    Role = TeacherInfo.User.Role;

                    if(TeacherInfo.HallNumber.HasValue)
                          Hall = TeacherInfo.HallNumber.ToString();
                }
            }

            AcceptCommand = new RelayCommand(Accept);

            CancelCommand = new RelayCommand(() => DialogHost.CloseDialogCommand.Execute(this, null));
        }


        public void Accept()
        {
            using (var repo = new Repository())
            {

                if (_isToEdit)
                {
                    repo.Attach(TeacherInfo);
                    repo.Attach(User);

                    if(TeacherInfo.ScheduleRecords.Any(s=>s.Group.CorpusId != Corpus.CorpusId))
                    {
                        CustomMessageBox messageBox = new CustomMessageBox("Ошибка обновления отделения",
                            $"Учитель имеет пары в расписании в отделении {TeacherInfo.Corpus.CorpusName}");

                        messageBox.ShowDialog();

                        return;
                    }
                }

                repo.Attach(Role);
                repo.Attach(Education);
                repo.Attach(Corpus);

                if (_isToEdit)
                {
                    User.Role = Role;
                    repo.Update(User);
                }
                else
                {
                    User.Role = Role;
                    repo.Add(User);
                }

                TeacherInfo.User = User;
                TeacherInfo.Education = Education;
                TeacherInfo.Corpus = Corpus;
   
                if (Int32.TryParse(Hall, out int hall))
                    TeacherInfo.HallNumber = hall;

                if (_isToEdit)
                {
                    repo.Update(TeacherInfo);
                }
                else
                {
                    repo.Add(TeacherInfo);
                }

                repo.SaveChanges();

                DialogHost.CloseDialogCommand.Execute(this, null);
            }
        }
    }
}
