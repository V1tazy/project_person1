using sotrudniki.Helper;
using sotrudniki.Model;
using sotrudniki.View;
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
using Newtonsoft.Json;
using System.Runtime.Remoting.Contexts;


namespace sotrudniki.ViewModel
{
    public class RoleViewModel : INotifyPropertyChanged
    {
        readonly string path = @"C:\Users\у\Desktop\bhwc-v2\sotrudniki\DataModels\RoleData.json";
        
        public ObservableCollection<Role> ListRole { get; set; } = new
       ObservableCollection<Role>();
        
        private Role _selectedRole;
       
        public Role SelectedRole
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

        public string Error { get; set; }

 string _jsonRoles = String.Empty;
        public RoleViewModel()
        {
            ListRole = new ObservableCollection<Role>();

            // Загрузка данных по должностям сотрудников
            ListRole = GetRoles();
        }
        private ObservableCollection<Role> GetRoles()
        {
            using (var context = new CompanyEntities())
            {
                var query = from role in context.Roles
                            orderby role.NameRole
                            select role;
                if (query.Count() != 0)
                {
                    foreach (var c in query)
                        ListRole.Add(c);
                }
            }
            return ListRole;
        }


        #region command AddRole
        /// команда добавления новой должности
        private RelayCommand _addRole;
        public RelayCommand AddRole
        {
            get
            {
                return _addRole ??
                (_addRole = new RelayCommand(obj =>
                {
                    Role newRole = new Role();
                    WindowNewRole wnRole = new WindowNewRole
                    {
                        Title = "Новая должность",
                        DataContext = newRole,
                    };
                    wnRole.ShowDialog();
                    if (wnRole.DialogResult == true)
                    {
                        using (var context = new CompanyEntities())
                        {
                            try
                            {
                                context.Roles.Add(newRole);
                context.SaveChanges();
                                ListRole.Clear();
                                ListRole = GetRoles();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("\nОшибка добавления данных!\n" +
                ex.Message, "Предупреждение");
                            }
                        }
                    }
                }, (obj) => true));
            }
        }

        #endregion

        #region Command EditRole
        /// команда редактирования должности
        private RelayCommand _editRole;
        public RelayCommand EditRole
        {
            get
            {
                return _editRole ??
                (_editRole = new RelayCommand(obj =>
                {
                    Role editRole = SelectedRole;
                    WindowNewRole wnRole = new WindowNewRole
                    {
                        Title = "Редактирование должности",
                        DataContext = editRole,
                    };
                    wnRole.ShowDialog();
                    if (wnRole.DialogResult == true)
                    {
                        using (var context = new CompanyEntities())   
                {
                            Role role = context.Roles.Find(editRole.Id);
                            if (role.NameRole != editRole.NameRole)
                                role.NameRole = editRole.NameRole.Trim();
                            try
                            {
                                context.SaveChanges();
                                ListRole.Clear();
                                ListRole = GetRoles();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("\nОшибка редактирования данных!\n" +
                ex.Message, "Предупреждение");
                            }
                        }
                    }
                    else
                    {
                        ListRole.Clear();
                        ListRole = GetRoles();
                    }
                }, (obj) => SelectedRole != null && ListRole.Count > 0));
            }
        }

        #endregion
        #region DeleteRole
        /// команда удаления должности
        private RelayCommand _deleteRole;
        public RelayCommand DeleteRole
        {
            get
            {
                return _deleteRole ??
                (_deleteRole = new RelayCommand(obj =>
                {
                    Role role = SelectedRole;
                    using (var context = new CompanyEntities())
                    {
                        // Поиск в контексте удаляемого автомобиля
                        Role delRole = context.Roles.Find(role.Id);
                        if (delRole != null)
                        {
        
                MessageBoxResult result = MessageBox.Show("Удалить данные по должности: " + delRole.NameRole,
               
                "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                            if (result == MessageBoxResult.OK)
                            {
                                try
                                {
                                    context.Roles.Remove(delRole);
                                    context.SaveChanges();
                                    ListRole.Remove(role);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("\nОшибка удаления данных!\n" +
                    ex.Message, "Предупреждение");
                                }
                            }
                        }
                    }
                }, (obj) => SelectedRole != null && ListRole.Count >
               0));
            }
        }
        #endregion
        #region Methods

        public ObservableCollection<Role> LoadRole()
        {
            _jsonRoles = File.ReadAllText(path);
            if (_jsonRoles != null)
            {
                ListRole = JsonConvert.DeserializeObject<ObservableCollection<Role>>(_jsonRoles);
                return ListRole;
            }
            else
            {
                return null;
           
            }
        }
        /// <summary>
        /// Нахождение максимального Id в коллекции
        /// </summary>
        /// <returns></returns>
        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListRole)
            {
                if (max < r.Id)
                {
                    max = r.Id;
                };
            }
            return max;
        }
        /// <summary>
        /// Сохранение json-строки с данными по должностям в файл
        /// </summary>
        /// <param name="listRole"></param>
        public void SaveChanges(ObservableCollection<Role> listRole)
        {
            var jsonRole = JsonConvert.SerializeObject(listRole);
            try
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.Write(jsonRole);
                }
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json файла /n" + e.Message;
            }
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]
string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
