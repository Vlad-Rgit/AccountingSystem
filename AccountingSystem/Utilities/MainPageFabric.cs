using AccountingSystem.Models;
using AccountingSystem.Repositories;
using AccountingSystem.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AccountingSystem.Utilities
{
    //Класс для генерации страницы в зависимости от RoleId
    public static class MainPageFabric
    {
        public static Page CreatePage(User user)
        {

            using (var repo = new Repository())
            {
                switch (user.RoleId)
                {
                    case 1: return new AdministratorMainPage();
                    case 2: return new TeacherMainPage(repo.GetWhere<TeacherView>((t) => t.Id == user.UserId));
                    case 3: return new TeacherMainPage(repo.GetWhere<TeacherView>((t) => t.Id == user.UserId));
                    case 4: return new StudentMainPage(repo.GetWhere<Student>((s)=> s.UserId == user.UserId));
                    default: return null;
                }
            }
        }
    }
}
