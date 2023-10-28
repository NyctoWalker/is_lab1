using MySql.Data.MySqlClient;
using NLog;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using is_lab3.Models;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace is_lab3
{
    class Program
    {

        public static UdpClient udpServer2;
        private static readonly int serverPort = 11043;
        //private static readonly int clientPort = 11042;
        private static readonly int clientPort = 11044;
        private static readonly IPAddress localAddress = IPAddress.Parse("127.0.0.1");
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            //const string connectionString = "server=localhost;port=3306;user=root;password=Gato1_otaG990;database=is_arch";
            udpServer2 = new UdpClient(serverPort);
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("Сервер запущен.");
            logger.Debug("Сервер запущен");
            try
            {
                Task task = Task.Run(ServerReceiveMessage);
                
                Console.ReadKey();
            }
            catch (Exception e)
            {
                logger.Error("Возникла ошибка. Подробности: " + e.Message);
                Console.WriteLine("Возникла ошибка. Подробности: " + e.Message);
            }

        }

        public static void ServerSendMessage(string message)
        {
            //using UdpClient sender = new UdpClient(clientPort);
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(message); //Получение размера сообщения в Unicode, возможно стоит взять UTF8
                                                                  //await sender.SendAsync(data, data.Length);
                logger.Debug("Отправлено сообщение клиенту: " + message);
                udpServer2.Send(data, data.Length, "127.0.0.1", clientPort); //Отправка клиенту
            }
            catch (Exception e)
            {
                logger.Error("Возникла ошибка при отправке сообщения. Подробности: " + e.Message);
                Console.WriteLine("Возникла ошибка. Подробности: " + e.Message);
            }
        }

        public static async Task ServerReceiveMessage()
        {
            //IPEndPoint remoteIP = (IPEndPoint)udpServer.Client.LocalEndPoint; //Адрес, с которого поступают данные
            string newData = "";
            DBManipulator DBReader = new();

            while (true)
            {
                try
                {
                    //byte[] data = udpServer.Receive(ref remoteIP);
                    var res = await udpServer2.ReceiveAsync();
                    string message = Encoding.Unicode.GetString(res.Buffer);
                    logger.Debug("Получено сообщение от клиента: " + message);

                    if (message.Length >= 3)
                        switch (message[1])
                        {
                            case '1':
                                {
                                    //ServerSendMessage("Идёт обращение к базе данных. Пожалуйста, подождите...");
                                    //ServerSendMessage("FirstName LastName Age HasDrivingLicense CargoLicenseID");
                                    //ServerSendMessage("----------------------------");
                                    List<string> dbValues = DBReader.GetDBValues();
                                    ServerSendMessage(dbValues.Count.ToString());

                                    foreach (string msg in dbValues)
                                        ServerSendMessage(msg);
                                    //ServerSendMessage("----------------------------\n");

                                    break;
                                }

                            case '2':
                                newData = message[4..];

                                if (DBReader.DBAddNew(newData))
                                    ServerSendMessage("Новые данные успешно записаны!\n");
                                else
                                    ServerSendMessage("Некорректный ввод! Данные не будут записаны.\n");

                                break;

                            case '3':
                                newData = message[4..];

                                DBReader.DeleteValueByID(newData);
                                /*foreach (string msg in DBReader.DeleteDBValuesByNumber(newData))
                                    ServerSendMessage(msg);
                                Console.WriteLine();*/

                                break;

                            case '4':
                                string serializedPeople = message[4..];

                                List<PersonLicense> people = JsonConvert.DeserializeObject<List<PersonLicense>>(serializedPeople);
                                DBReader.UpdateDB(people);

                                break;

                            default:
                                break;
                        }

                    Console.WriteLine("Сообщение от клиента: {0}", message);
                }
                catch (Exception e)
                {
                    logger.Error("Возникла ошибка при обработке полученного сообщения. Подробности: " + e.Message);
                    Console.WriteLine("Возникла ошибка. Подробности:" + e.Message);
                }
            }
        }

        
    }
}
