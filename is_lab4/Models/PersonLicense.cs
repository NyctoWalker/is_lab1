using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace is_lab3.Models
{
    public class PersonLicense : INotifyPropertyChanged
    {
        private string firstName;
        private string lastName;
        private uint age;
        private sbyte hasDrivingLicense;
        private int idCargoLicense;

        public string FirstName 
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        public uint Age
        {
            get { return age; }
            set
            {
                age = value;
                OnPropertyChanged("Age");
            }
        }

        public sbyte HasDrivingLicense
        {
            get { return hasDrivingLicense; }
            set
            {
                hasDrivingLicense = value;
                OnPropertyChanged("HasDrivingLicense");
            }
        }

        public int IdCargoLicense
        {
            get { return idCargoLicense; }
            set
            {
                idCargoLicense = value;
                OnPropertyChanged("IdCargoLicense");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
