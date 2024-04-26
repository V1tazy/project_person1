using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace sotrudniki.Model
{
    public class Role
    {
        /// <summary>
        /// код должности
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// наименование должности
        /// </summary>
        public string NameRole { get; set; }
        public Role()
        {
            this.Persons = new HashSet<Person>();
        }
        /// <summary>
        /// коллекция Persons для связи с классом Person
        /// </summary>
        public virtual ICollection<Person> Persons { get; set; }
    }
}
