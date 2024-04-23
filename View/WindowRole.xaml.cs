using project_person.Model;
using project_person.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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

namespace project_person.View
{
    public partial class WindowRole : Window
    {
        private RoleViewModel vmRole;
        public WindowRole()
        {
            InitializeComponent();
            DataContext = new RoleViewModel();
        }
    }
}
