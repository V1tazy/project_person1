﻿using project_person.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_person.ViewModel
{
    public class RoleModelView
    {
        ObservableCollection<Role> ListRole = 
            new ObservableCollection<Role> ();
        

        public RoleModelView()
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
    }
}
