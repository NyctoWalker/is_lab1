using GalaSoft.MvvmLight.Command;
using is_lab3.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace is_lab4
{
    public class PersonVM : INotifyPropertyChanged
    {
        private ObservableCollection<PersonLicense> _records;
        public ObservableCollection<PersonLicense> Records
        {
            get { return _records; }
            set
            {
                if (_records != value)
                {
                    _records = value;
                    //SaveData();
                    OnPropertyChanged(nameof(Records));
                }
            }
        }

        public ICommand LoadCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public PersonVM()
        {
            LoadCommand = new RelayCommand(LoadData);
            SaveCommand = new RelayCommand(SaveData);
            LoadData();
        }

        private void LoadData()
        {
            using (is_archContext db = new())
            {
                var recordEntities = db.PersonLicenses;
                Records = new ObservableCollection<PersonLicense>();
                foreach (PersonLicense p in recordEntities)
                { Records.Add(p); }
                //(recordEntities.Select(e => new PersonLicense()));
            }
        }
        //Переделать на клиент-сервер, выборочно сохранять записи, 
        private void SaveData()
        {
            using (is_archContext db = new())
            {
                foreach (var recordModel in Records)
                {
                    //try-catch
                    var recordEntity = db.PersonLicenses.Find(recordModel.IdCargoLicense);
                    recordEntity.LastName = recordModel.LastName;
                    recordEntity.FirstName = recordModel.FirstName;
                    recordEntity.Age = recordModel.Age;
                    recordEntity.HasDrivingLicense = recordModel.HasDrivingLicense;
                    recordEntity.IdCargoLicense = recordModel.IdCargoLicense;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Возникла ошибка при сохранении данных: " + e.Message);
                    //LoadData();
                }
                finally
                {
                    MessageBox.Show("Сохранение успешно!");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
