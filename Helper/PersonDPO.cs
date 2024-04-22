using project_person.Model;
using project_person.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_person.Helper
{
    public class PersonDPO
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        
        public PersonDPO() { }

        public PersonDPO(int id, string role, string firstName, string lastName, DateTime birthday)
        {
            this.Id = id;
            this.Role = role;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Birthday = birthday;
        }

        public PersonDPO CopyFromPerson(Person person)
        {
            PersonDPO perDPO = new PersonDPO();
            RoleViewModel vmRole = new RoleViewModel();
            string role = string.Empty;

            foreach (var r in vmRole.ListRole)
            {
                if(r.Id == person.RoleId)
                {
                    role = r.NameRole;
                    break;
                }
            }
            if(role != string.Empty)
            {
                perDPO.Id = person.Id;
                perDPO.Role = role;
                perDPO.FirstName = person.FirstName;
                perDPO.LastName = person.LastName;
                perDPO.Birthday = person.Birthday;
            }
            return perDPO;
        }

        internal PersonDPO ShallowCopy()
        {
            return (PersonDPO)this.MemberwiseClone();
        }
    }
}
