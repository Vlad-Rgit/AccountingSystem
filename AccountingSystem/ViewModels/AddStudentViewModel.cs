using AccountingSystem.Commands;
using AccountingSystem.Models;
using AccountingSystem.Repositories;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.ViewModels
{
    public class AddStudentViewModel : ViewModelBase
    {
        //Модифицировать или добавить нового студента
        private bool _isToUpdate = false;

        //Группы
        public List<Group> Groups { get; }
        public Group Group { get; set; }

        //Адрес
        public Address Address { get; private set; }
        public City City { get; private set; }
        public Street Street { get; private set; }

        //Положение студента (Академ. отпуск или отчислен)
        public List<StudentState> StudentStates { get; }
        public StudentState StudentState { get; set; }

        //User для указания логина и пароля
        public User User { get; private set; }

        public Student Student { get; }

        //Команда потвердить
        public RelayCommand AcceptCommand { get; }
        public RelayCommand CancelCommand { get; }
        public AddStudentViewModel(StudentView student)
        {
            using (var repo = new Repository())
            {

                Groups = new List<Group>(repo.GetAll<Group>());
                StudentStates = new List<StudentState>(repo.GetAll<StudentState>());

                //Подготовка добавления нового студента
                if (student == null)
                {
                    Student = new Student();
                    Student.DateBirth = new DateTime(1990, 2, 2);
                    Address = new Address();
                    City = new City();
                    Street = new Street();

                    User = new User() { RoleId = 4 };

                    Group = Groups[0];
                    StudentState = StudentStates[2];
                }
                //Подготовка модификации студента
                else
                {
                    _isToUpdate = true;

                    Student = repo.Get<Student>(student.Id);

                    Address = Student.Address;
                    City = Address.City;
                    Street = Address.Street;

                    User = Student.User;

                    if (User == null)
                        User = new User();

                    Group = Student.Group;
                    StudentState = Student.StudentState;
                }
            }
            
            AcceptCommand = new RelayCommand(Accept);
            CancelCommand = new RelayCommand(() => DialogHost.CloseDialogCommand.Execute(this, null));
        }


        public void Accept()
        {
            using(var repo = new Repository())
            {
                //Присоединение зависимых сущностей
                repo.Attach(Group);
                repo.Attach(StudentState);

                //Если пароль или логин не указан - User к студенту не приклепляется
                if(User.Login == String.Empty || User.Password == String.Empty)                
                    User = null;               
                else
                    repo.Add(User);


                //Поиск адреса, если нет то добавить в базу
                City = repo.AddOrAttach(City, (c) =>
                {
                    return c.CityName == City.CityName;
                });

                Street = repo.AddOrAttach(Street, (s) =>
                {
                    return s.StreetName == Street.StreetName;
                });

                Address.City = City;
                Address.Street = Street;

                Address = repo.AddOrAttach<Address>(Address, (adr) => {

                    return adr.CityId == City.CityId &&
                           adr.StreetId == Street.StreetId &&
                           adr.HausNumber == adr.HausNumber;

                });

                //Присоединение зависимых сущностей к студенту
                Student.Address = Address;
                Student.User = User;
                Student.Group = Group;
                Student.StudentState = StudentState;


                //Модификация или добавление студента
                if (_isToUpdate)
                    repo.Update(Student);
                else
                    repo.Add(Student);


                repo.SaveChanges();
            }


            Close();
        }

        //Закрытие диалога
        public override void Close()
        {
            DialogHost.CloseDialogCommand?.Execute(this, null);
        }
    }
}
