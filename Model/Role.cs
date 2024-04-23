using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace project_person.Model
{
    public class Role: INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string nameRole { get; set; }

        public string NameRole { 
            get { return nameRole; }
            set
            {
                nameRole = value;
                OnPropertyChanged("NameRole");
            }
                }
        public Role() { }

        public Role(int id, string nameRole)
        {
            this.Id = id;
            this.NameRole = nameRole;
        }

        public Role ShallowCopy()
        {
            return (Role)this.MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
