using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace AccountingSystem.Models
{
    public partial class ScheduleView
    {
        public int HallNumber
        {
            get => Номер_кабинета == null ? Кабинет_преподователя.Value : Номер_кабинета.Value;
        }

        public string LessonNumber
        {
            get
            {
                string result = Номер_пары.ToString();

                if (Знаменатель.HasValue)
                {
                    if (Знаменатель.Value == true)
                        result += " знам.";
                    else
                        result += " чис.";
                }

                return result;
            }
        }
    }
}
