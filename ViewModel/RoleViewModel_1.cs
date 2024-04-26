using Newtonsoft.Json;
using sotrudniki;
using sotrudniki.Helper;
using sotrudniki.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace project_person.ViewModel
{
    public class RoleViewModel
    {
        public ObservableCollection<Role> ListRole { get; set; }
        public RoleViewModel()
        {
            ListRole = new ObservableCollection<Role>();

            ListRole = GetRoles();
        }

        private ObservableCollection<Role> GetRoles()
        {
            using(var context = new CompanyEntities())
            {
                var query = from role in context.Roles
                            orderby role.NameRole
                            select role;
                if(query.Count() != 0)
                {
                    foreach(var c in query)
                    {
                        ListRole.Add(c);
                    }
                }
                return ListRole;
            }
        }

        #region AddRole
        private RelayCommand _addRole;
        public RelayCommand AddRole
        {
            get
            {
                return _addRole ??
                    (_addRole = new RelayCommand(obj =>
                    {

                    }));
            }
        }
        #endregion
    }


}