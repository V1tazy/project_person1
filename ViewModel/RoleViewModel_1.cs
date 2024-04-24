using Newtonsoft.Json;
using project_person.Helper;
using project_person.Model;
using project_person.View;
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
    public class RoleViewModel : INotifyPropertyChanged
    {
        readonly string path = @"C:\Users\89960\Desktop\project_person\DataModels\RoleData.json";

        public ObservableCollection<Role> ListRole {get; set;} = new ObservableCollection<Role> ();

        private Role _selectedRole;
        private Role SelectedRole
        {
            get
            {
                return _selectedRole;
            }
            set
            {
                _selectedRole = value;
                OnPropertyChanged("SelectedRole");
                EditRole.CanExecute(true);
            }
        }

        string _jsonRoles = String.Empty;

        public string Error { get; set; }

        public RoleViewModel()
        {
            ListRole = LoadRole();
        }

        #region command AddRole
        private RelayCommand _addRole;

        public RelayCommand AddRole
        {
            get
            {
                return _addRole ??
                    (_addRole = new RelayCommand(obj =>
                    {
                        WindowNewRole wnRole = new WindowNewRole
                        {
                            Title = "Новая должность"
                        };

                        int maxIdRole = MaxId() + 1;
                        Role role = new Role { Id = maxIdRole };

                        wnRole.DataContext = role;

                        if (wnRole.ShowDialog() == true)
                        {
                            ListRole.Add(role);
                            SaveChanges(ListRole);
                        }
                        SelectedRole = role;
                    },
                    (obj => true)));
            }
        }
        #endregion
        #region Command EditRole
        private RelayCommand _editRole;
        public RelayCommand EditRole
        {
            get
            {
                return _editRole ??
                    (_editRole = new RelayCommand(obj =>
                    {
                        WindowNewRole wnRole = new WindowNewRole
                        {
                            Title = "Редактировать Должность"
                        };

                        Role role = SelectedRole;

                        var tempRole = role.ShallowCopy();
                        wnRole.DataContext = tempRole;

                        if(wnRole.ShowDialog() == true)
                        {
                            role.NameRole = tempRole.NameRole;
                            SaveChanges(ListRole);
                        }
                    }, (obj => SelectedRole != null && ListRole.Count > 0)
                    ));
            }
        }
        #endregion
        #region Command DeleteRole
        private RelayCommand _deleteRole;
        public RelayCommand DeleteRole
        {
            get
            {
                return _deleteRole ??
                (_deleteRole = new RelayCommand(obj =>
                {
                    Role role = SelectedRole;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по должности: " + role.NameRole, "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        ListRole.Remove(role);
                        SaveChanges(ListRole);
                    }
                }, (obj) => SelectedRole != null && ListRole.Count > 0));
            }
        }

        #endregion

        public int MaxId()
        {
            int max = 0;
            foreach(var r in this.ListRole)
            {
                if(max < r.Id)
                {
                    max = r.Id;
                };
            }
            return max;
        }
        public ObservableCollection<Role> LoadRole()
        {
            _jsonRoles = File.ReadAllText(path);
            
            if(_jsonRoles != null)
            {
                ListRole = JsonConvert.DeserializeObject<ObservableCollection<Role>> (_jsonRoles);

                return ListRole;
            }
            else
            {
                return null;
            }
        }

        private void SaveChanges(ObservableCollection<Role> listRoles) 
        {
            var jsonRole = JsonConvert.SerializeObject(listRoles);
            try
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.Write(jsonRole);
                }
            }
            catch (IOException ex)
            {
                Error = "Ошибка записи json файла \n" + ex.Message;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
    }


}