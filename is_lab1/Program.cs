using NLog;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
/*using csv_manipulator.cs;*/

namespace is_lab1
{
    class Program
    {

        /*Lab 2(server) main*/
        //static string message;
        public static UdpClient udpServer;
        private static readonly int serverPort = 11041;
        private static readonly int clientPort = 11042;
        private static readonly IPAddress localAddress = IPAddress.Parse("127.0.0.1");
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main()
        {
            udpServer = new UdpClient(serverPort);
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("Сервер запущен.");
            logger.Debug("Сервер запущен");
            try
            {
                Task task = Task.Run(ServerReceiveMessage);
                //await ServerSendMessage();

                //ServerReceiveMessage();
                //ServerSendMessage();
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
                udpServer.Send(data, data.Length, "127.0.0.1", clientPort); //Отправка клиенту
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

            CsvManipulator CsvReader = new CsvManipulator();
            CsvReader.ConfigRead();
            string newData = "";

            while (true)
            {
                try
                {
                    //byte[] data = udpServer.Receive(ref remoteIP);
                    var res = await udpServer.ReceiveAsync();
                    string message = Encoding.Unicode.GetString(res.Buffer);
                    logger.Debug("Получено сообщение от клиента: " + message);
                    if (message.Length >= 3)
                        switch (message[1])
                        {
                            case '1':
                                {
                                    CsvReader.CsvReadValue();

                                    if (CsvReader.CsvConfig.HasHeader == "true")
                                    {
                                        ServerSendMessage(CsvReader.csvHeader.Replace(',', ' '));
                                    }
                                    ServerSendMessage("----------------------------");
                                    int i = 1;
                                    foreach (CsvRecord _record in CsvReader.Records)
                                    {
                                        string _Record = string.Join(" ", _record.Records);
                                        string msg = i.ToString() + ". " + _Record;
                                        ServerSendMessage(msg);
                                        i++;
                                    }
                                    ServerSendMessage("----------------------------\n");

                                    break;
                                }

                            case '2':
                                newData = message[4..];
                                ServerSendMessage("----------------------------");
                                foreach (int _int_ in CsvReader.CsvReadByNumber(newData))
                                {
                                    try
                                    {
                                        string _Record = string.Join(" ", CsvReader.Records[_int_ - 1].Records);
                                        string msg = _int_.ToString() + ". " + _Record;
                                        ServerSendMessage(msg);
                                    }
                                    catch //(Exception e)
                                    {
                                        //Console.WriteLine("Ошибка при выводе строки: "+e.Message);
                                    }
                                }
                                ServerSendMessage("----------------------------\n");

                                break;

                            case '3':
                                newData = message[4..];
                                if (CsvReader.CsvWriteValue(newData))
                                    ServerSendMessage("Новые данные успешно записаны!\n");
                                else
                                    ServerSendMessage("Некорректный ввод! Данные не будут записаны.\n");

                                break;

                            case '4':
                                newData = message[4..];
                                foreach (string _str_ in CsvReader.CsvDeleteByNumber(newData))
                                {
                                    try
                                    {
                                        ServerSendMessage("Удалено: " + _str_);
                                    }
                                    catch //(Exception e)
                                    {
                                        //Console.WriteLine("Ошибка при выводе строки: "+e.Message);
                                    }
                                }
                                Console.WriteLine();

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
        


        /* Lab1 main *//*
        static void Main(string[] args)
        {
            CsvManipulator CsvReader = new CsvManipulator();
            ConsoleKey choice;

            Console.WriteLine("Читаем конфиг-файл...\n");
            //Тут должен читаться конфиг и быть отдельная менюшка в зависимости от успешности прочтения
            if (CsvReader.ConfigRead())
            {
                Console.WriteLine("Название csv-файла для чтения: {0}", CsvReader.CsvConfig.CsvName);
                Console.WriteLine("CSV имеет заголовок? (для игнорирования первой строки если это не запись)\n{0}", CsvReader.CsvConfig.HasHeader);
                Console.WriteLine("Заголовочное поле: \n{0}\n", CsvReader.CsvConfig.DataHeaders.Replace(',', ' '));
                Console.WriteLine("Возможен ли null? (1 - да, 0 - нет): \n{0}\n", CsvReader.CsvConfig.DataNullables.Replace(',', ' '));
                Console.WriteLine("Типы данных: \n{0}\n", CsvReader.CsvConfig.DataTypes.Replace(',', ' '));
                Console.WriteLine("Для типов данных:\n1 - str, \n2 - int, \n3 - bool, \n4 - byte\n");
            }
            else
                Console.WriteLine("Ошибка чтения конфиг-файла!");


            while (true)
            {
                Console.WriteLine("Добро пожаловать в основное меню!");
                Console.WriteLine("Нажмите 1 - посмотреть записи в файле");
                Console.WriteLine("Нажмите 2 - вывести запись по номеру");
                Console.WriteLine("Нажмите 3 - создать новую запись");
                Console.WriteLine("Нажмите 4 - удалить записи из файла(все неподходящие по формату будут так же удалены!)");
                Console.WriteLine("Нажмите 5 - посмотреть файл конфигурации");
                Console.WriteLine("Нажмите Esc для выхода");
                Console.WriteLine();
                choice = Console.ReadKey(true).Key;

                switch (choice)
                {
                    case ConsoleKey.D1:
                        {
                            CsvReader.CsvReadValue();

                            if (CsvReader.CsvConfig.HasHeader == "true")
                            {
                                Console.WriteLine(CsvReader.csvHeader.Replace(',', ' '));
                            }
                            Console.WriteLine("----------------------------");
                            int i = 1;
                            foreach (CsvRecord _record in CsvReader.Records)
                            {
                                Console.WriteLine("{0}. {1}", i, string.Join(" ", _record.Records));
                                i++;
                            }
                            Console.WriteLine("----------------------------");
                            Console.WriteLine();

                            break;
                        }

                    case ConsoleKey.D2:
                        {
                            Console.WriteLine("Введите номер/номера(через пробелы или запятые) записи:");
                            string newData = Console.ReadLine();
                            Console.WriteLine("\n----------------------------");
                            foreach (int _int_ in CsvReader.CsvReadByNumber(newData))
                            {
                                try
                                {
                                    Console.WriteLine("{0}. {1}", _int_, string.Join(" ", CsvReader.Records[_int_].Records));
                                }
                                catch //(Exception e)
                                {
                                    //Console.WriteLine("Ошибка при выводе строки: "+e.Message);
                                }
                            }
                            Console.WriteLine("----------------------------");
                            Console.WriteLine();

                            break;
                        }

                    case ConsoleKey.D3:
                        {
                            Console.WriteLine("Введите данные, содержащиеся в новой записи через пробел или запятую:");
                            string newData = Console.ReadLine();
                            if (CsvReader.CsvWriteValue(newData))
                                Console.WriteLine("Новые данные успешно записаны!");
                            else
                                Console.WriteLine("\aНекорректный ввод! Данные не будут записаны.");
                            Console.WriteLine();

                            break;
                        }

                    case ConsoleKey.D4:
                        {
                            Console.WriteLine("Введите номер/номера(через пробелы или запятые) записи для удаления:");
                            string newData = Console.ReadLine();
                            foreach (string _str_ in CsvReader.CsvDeleteByNumber(newData))
                            {
                                try
                                {
                                    Console.WriteLine("Удалено: {0}", _str_);
                                }
                                catch //(Exception e)
                                {
                                    //Console.WriteLine("Ошибка при выводе строки: "+e.Message);
                                }
                            }
                            Console.WriteLine();

                            break;
                        }

                    case ConsoleKey.D5:
                        {
                            if (CsvReader.ConfigRead())
                            {
                                Console.WriteLine("Название csv-файла для чтения: {0}", CsvReader.CsvConfig.CsvName);
                                Console.WriteLine("CSV имеет заголовок? (для игнорирования первой строки если это не запись)\n{0}", CsvReader.CsvConfig.HasHeader);
                                Console.WriteLine("Заголовочное поле: \n{0}\n", CsvReader.CsvConfig.DataHeaders.Replace(',', ' '));
                                Console.WriteLine("Возможен ли null? (1 - да, 0 - нет): \n{0}\n", CsvReader.CsvConfig.DataNullables.Replace(',', ' '));
                                Console.WriteLine("Типы данных: \n{0}\n", CsvReader.CsvConfig.DataTypes.Replace(',', ' '));
                                Console.WriteLine("Для типов данных:\n1 - str, \n2 - int, \n3 - bool, \n4 - byte");
                            }
                            else
                                Console.WriteLine("Ошибка чтения конфиг-файла!");
                            Console.WriteLine();

                            break;
                        }

                    case ConsoleKey.Escape:
                        {
                            Environment.Exit(1);

                            break;
                        }

                    default:
                        {
                            Console.WriteLine("\aОшибка ввода! Попробуйте снова.");
                            Console.WriteLine();

                            break;
                        }
                }
            }
        }
        */
    }
}
