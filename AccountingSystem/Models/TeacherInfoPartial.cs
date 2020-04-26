using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models
{
    public partial class TeacherInfo
    {
        public string Fio
        {
            get => $"{LastName} {FirstName} {Patronymic}";
        }
    }

}
