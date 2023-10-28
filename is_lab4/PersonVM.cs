using GalaSoft.MvvmLight.Command;
using is_lab3.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace is_lab4
{
    public class PersonVM : INotifyPropertyChanged
    {
        public static UdpClient udpClient;
        //public static UdpClient udpServer;
        public static IPAddress localAddress;
        private char[] delimiters = { ',', ' ', ';' };

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
            LoadCommand = new RelayCommand(UpdateRecords);
            SaveCommand = new RelayCommand(UploadAllData);
            
            //udpServer = new UdpClient(11043);
            udpClient = new UdpClient(11044);
            localAddress = IPAddress.Parse("127.0.0.1");
            //UpdateRecords();
            Records = new ObservableCollection<PersonLicense>();
            Records.CollectionChanged += RecordsCollectionChanged;
        }

        public void UpdateRecords()
        {
            int number;
            List<string> message = new();
            SendMessage("[1]");
            if (!int.TryParse(RecieveMessage(), out number))
                return;

            if (Records == null)
            {
                Records = new ObservableCollection<PersonLicense>();
                Records.CollectionChanged += RecordsCollectionChanged;
            }
            else
            {
                Records.CollectionChanged -= RecordsCollectionChanged;
                Records.Clear();
                Records.CollectionChanged += RecordsCollectionChanged;
            }
                

            for (int i = 0; i < number; i++)
            {
                message.Add(RecieveMessage());
            }

            foreach (string s in message)
            {
                string[] mes = s.Split(delimiters);
                if (mes.Length != 5)
                    return;
                //string firstName;
                //string lastName;
                uint age;
                sbyte hasDrivingLicense;
                int id;

                if (uint.TryParse(mes[2], out age) && sbyte.TryParse(mes[3], out hasDrivingLicense) && int.TryParse(mes[4], out id))
                {
                    PersonLicense person = new PersonLicense { IdCargoLicense = id, Age = age, FirstName = mes[0], LastName = mes[1], HasDrivingLicense = hasDrivingLicense };
                    Records.Add(person);
                }
            }
        }

        //Загрузка всех записей
        //Загрузка/изменение/удаление записей?
        public void UploadAllData()
        {
            List<PersonLicense> people = Records.ToList();
            string serializedPeople = JsonConvert.SerializeObject(people);
            SendMessage("[4] " + serializedPeople);
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

        #region UDP

        private static void SendMessage(string message)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(message);
                udpClient.Send(data, data.Length, localAddress.ToString(), 11043);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static string RecieveMessage()
        {
            IPEndPoint remoteIP = (IPEndPoint)udpClient.Client.LocalEndPoint;
            try
            {
                byte[] data = udpClient.Receive(ref remoteIP);
                string message = Encoding.Unicode.GetString(data);
                return message;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        #endregion

        private void RecordsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (PersonLicense p in e.NewItems)
                {
                    // Send message to server to add person
                    //SendMessage($"[2] {p.FirstName} {p.LastName} {p.Age} {p.HasDrivingLicense}");
                    //LoadData()
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (PersonLicense p in e.OldItems)
                {
                    // Send message to server to remove person
                    SendMessage($"[3] {p.IdCargoLicense}");
                    //LoadData()
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
