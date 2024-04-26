using sotrudniki.Helper;
using sotrudniki.Model;
using sotrudniki.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace sotrudniki.View
{
    /// <summary>
    /// Логика взаимодействия для WindowEmployee.xaml
    /// </summary>
    public partial class WindowEmployee : Window
    {
        PersonViewModel vmPerson;
        public WindowEmployee()
        {
            InitializeComponent();
            vmPerson = new PersonViewModel();
            DataContext = vmPerson;
        }
        private void EmployeeListView_Select(object sender, SelectionChangedEventArgs e)
        {
            if (lvEmployee.SelectedItem != null)
            {
                var selectedPerson = lvEmployee.SelectedItem as Person;
                vmPerson.SelectedPerson = selectedPerson;
            }
        }
    }
}

