using sotrudniki.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sotrudniki.Model
{
    public class Person
    {
        /// <summary>
        /// код сотрудника
        /// </summary>
        public int Id { get; set; } // код
        /// <summary>
        /// код должности сотрудника
        /// </summary>
        public int RoleId { get; set; } // код должности
 /// <summary>
 /// имя сотрудника
 /// </summary>

        public string FirstName { get; set; } // имя
        /// <summary>
        /// фамилия сотрудника
        /// </summary>
        public string LastName { get; set; } // фамилия
        /// <summary>
        /// дата рождения сотрудника
        /// </summary>
        public DateTime Birthday { get; set; } // дата рождения
        /// <summary>
        /// класс должности для связи с сущностью Role
        /// </summary>
        public virtual Role Role { get; set; }
    }
}
