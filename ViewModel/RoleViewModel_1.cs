using project_person.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_person.ViewModel
{
    public class RoleViewModel
    {
        public ObservableCollection<Role> ListRole = 
            new ObservableCollection<Role> ();
        
        public RoleViewModel()
        {
            this.ListRole.Add(
                new Role()
                {
                    Id = 1,
                    NameRole = "Директор"
                });

            this.ListRole.Add(
                new Role()
                {
                    Id = 2,
                    NameRole = "Бухгалтер"
                });

            this.ListRole.Add(
                new Role()
                {
                    Id = 3,
                    NameRole = "Менеджер"
                });
        }

        public int MaxId()
        {
            int max = 0;
            foreach(var r in this.ListRole)
            {
                if(max < r.Id)
                {
                    max = r.Id;
                }
            }
            return max;
        }
    }
}
