using JetBrains.Annotations;
using Newtonsoft.Json;
using project_person.Helper;
using project_person.Model;
using project_person.View;
using project_person.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace project_person.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        readonly string path = @"C:\Users\89960\Desktop\project_person\DataModels\PersonData.json";
        private PersonDPO _selectedPersonDpo;
        /// <summary>
        /// выделенные в списке данные по сотруднику 
        /// </summary>
        public PersonDPO SelectedPersonDpo
        {
            get { return _selectedPersonDpo; }
            set
            {
                _selectedPersonDpo = value;
                OnPropertyChanged("SelectedPersonDpo");
            }
        }
        /// <summary>
        /// коллекция данных по сотрудникам
        /// </summary>
        public ObservableCollection<Person> ListPerson { get; set; }
        public ObservableCollection<PersonDPO> ListPersonDpo
        {
            get;
            set;
        }
        string _jsonPersons = String.Empty;
        public string Error { get; set; }
        public string Message { get; set; }
        public PersonViewModel()
        {
            ListPerson = new ObservableCollection<Person>();
            ListPersonDpo = new ObservableCollection<PersonDPO>();
            ListPerson = LoadPerson();
            ListPersonDpo = GetListPersonDpo();
        }

        #region AddPerson
        private RelayCommand _addPerson;
        public RelayCommand AddPerson
        {
            get
            {
                return _addPerson ??
                (_addPerson = new RelayCommand(obj =>
                {
                    WindowNewEmployee wnPerson = new WindowNewEmployee
                    {
                        Title = "Новый сотрудник"
                    };
                    int maxIdPerson = MaxId() + 1;
                    PersonDPO per = new PersonDPO
                    {
                        Id = maxIdPerson,
                        Birthday = DateTime.Now.ToString(),
                    };

                    wnPerson.DataContext = per;
                    wnPerson.CbRole.ItemsSource = new RoleViewModel().LoadRole();
                    if (wnPerson.ShowDialog() == true)
                    {
                        var r = (Role)wnPerson.CbRole.SelectedValue;
                        if (r != null)
                        {
                            per.RoleName = r.NameRole;
                            per.Birthday = PersonDPO.GetStringBirthday(per.Birthday);
                            ListPersonDpo.Add(per);
                            Person p = new Person();
                            p = p.CopyFromPersonDPO(per);
                            ListPerson.Add(p);
                            try
                            {
                                SaveChanges(ListPerson);
                            }
                            catch (Exception e)
                            {
                                Error = "Ошибка добавления данных в json файл\n" + e.Message;
                            }
                            SelectedPersonDpo = per;
                        }
                    }
               
                },
                (obj) => true));
            }
        }
        #endregion
        #region EditPerson
        private RelayCommand _editPerson;
        public RelayCommand EditPerson
        {
            get
            {
                return _editPerson ??
                (_editPerson = new RelayCommand(obj =>
                {
                    WindowNewEmployee wnPerson = new WindowNewEmployee()
                    {
                        Title = "Редактирование данных сотрудника",
                    };
                    PersonDPO personDpo = SelectedPersonDpo;
                    var tempPerson = personDpo.ShallowCopy();
                    wnPerson.DataContext = tempPerson;
                    wnPerson.CbRole.ItemsSource = new RoleViewModel().LoadRole();

                    if (wnPerson.ShowDialog() == true)
                    {
                        var r = (Role)wnPerson.CbRole.SelectedValue;
                        if (r != null)
                        {
                            personDpo.RoleName = r.NameRole;
                            personDpo.FirstName = tempPerson.FirstName;
                            personDpo.LastName = tempPerson.LastName;
                            personDpo.Birthday = PersonDPO.GetStringBirthday(tempPerson.Birthday);
        
                            var per = ListPerson.FirstOrDefault(p => p.Id == personDpo.Id);
                                if (per != null)
                                {
                                    per = per.CopyFromPersonDPO(personDpo);
                                }
                            try
                                {
                                SaveChanges(ListPerson);
                            }
                            catch (Exception e)
                            {
            
                            Error = "Ошибка редактирования данных в json файл\n" + e.Message;
                            }
                            SelectedPersonDpo = personDpo;
                        }
                        else
                        {
                            Message = "Необходимо выбрать должность сотрудника.";
                        }
                    }
                }, (obj) => SelectedPersonDpo != null && ListPersonDpo.Count > 0));
            }
        }
        #endregion
        #region DeletePerson
        private RelayCommand _deletePerson;
        public RelayCommand DeletePerson
        {
            get
            {
                return _deletePerson ??
                (_deletePerson = new RelayCommand(obj =>
                {
                    PersonDPO person = SelectedPersonDpo;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по сотруднику: \n" + person.LastName + " " + person.FirstName, "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        try
                        {
                            ListPersonDpo.Remove(person);
                            var per = ListPerson.FirstOrDefault(p => p.Id == person.Id);
                            if (per != null)
                            {
                                ListPerson.Remove(per);
                                SaveChanges(ListPerson);
                            }
                        }
                        catch (Exception e)
                        {
                            Error = "Ошибка удаления данных\n" + e.Message;
                        }
                    }
               
                }, (obj) => SelectedPersonDpo != null && ListPersonDpo.Count > 0));
            }
        }
        #endregion
        #region Method

        public ObservableCollection<Person> LoadPerson()
        {
            _jsonPersons = File.ReadAllText(path);
            if (_jsonPersons != null)
            {
                ListPerson = JsonConvert.DeserializeObject < ObservableCollection <Person>> (_jsonPersons);
                return ListPerson;
            }
            else
            {
                return null;
            }
        }

        public ObservableCollection<PersonDPO> GetListPersonDpo()
        {
            foreach (var person in ListPerson)
            {
                PersonDPO p = new PersonDPO();
                p = p.CopyFromPerson(person);
                ListPersonDpo.Add(p);
            }
            return ListPersonDpo;
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListPerson)
            {
                if (max < r.Id)
                {
                    max = r.Id;
                };
            }
            return max;
        }
            private void SaveChanges(ObservableCollection<Person> listPersons)
        {
            var jsonPerson = JsonConvert.SerializeObject(listPersons);
            try
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.Write(jsonPerson);
                }
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json файла /n" + e.Message;
            }
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
