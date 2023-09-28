using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace is_lab2
{
    class Program
    {
        public static UdpClient udpClient;
        private static readonly int serverPort = 11041;
        private static readonly int clientPort = 11042;

        static async Task Main()
        {
            
            udpClient = new UdpClient(clientPort);
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("Клиент запущен.");
            Console.WriteLine("Введите 1 - посмотреть записи в файле");
            Console.WriteLine("Введите 2 - вывести записи по номерам");
            Console.WriteLine("Введите 3 - создать новую запись");
            Console.WriteLine("Введите 4 - удалить записи из файла по номерам(все неподходящие по формату будут так же удалены!)");
            Console.WriteLine();
            //ConsoleKey choice = Console.ReadKey(true).Key;

            try
            {
                Task task = Task.Run(ClientReceiveMessage);
                await ClientSendMessage();

                //ClientSendMessage();
                //ClientReceiveMessage();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Возникла ошибка. Подробности:" + e.Message);
            }
        }

        private static async Task ClientSendMessage()
        {
            while (true)
            {
                string input = Console.ReadLine();
                string message = "";

                switch (input)
                {
                    case "1":
                        message = "[1]";

                        break;

                    case "2":
                        Console.WriteLine("Введите номера записей для просмотра:");
                        message = "[2] " + Console.ReadLine();

                        break;

                    case "3":
                        Console.WriteLine("Введите данные для новой записи:");
                        message = "[3] " + Console.ReadLine();

                        break;

                    case "4":
                        Console.WriteLine("Введите номера записей для удаления:");
                        message = "[4] " + Console.ReadLine();

                        break;

                    default:
                        Console.WriteLine("Некорректный ввод!");
                        break;
                }

                try
                {
                    byte[] data = Encoding.Unicode.GetBytes(message); //Получение размера сообщения в Unicode, возможно стоит взять UTF8
                    //await sender.SendAsync(data, data.Length);
                    await udpClient.SendAsync(data, data.Length, "127.0.0.1", serverPort); //Отправка клиенту
                }
                catch (Exception e)
                {
                    Console.WriteLine("Возникла ошибка. Подробности:" + e.Message);
                }
            }
        }

        private static async Task ClientReceiveMessage()
        {
            while (true)
            {
                try
                {
                    //byte[] data = udpServer.Receive(ref remoteIP);
                    var res = await udpClient.ReceiveAsync();
                    string message = Encoding.Unicode.GetString(res.Buffer);
                    Console.WriteLine("[Сервер]: {0}", message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Возникла ошибка. Подробности:" + e.Message);
                }
            }
            /*            IPEndPoint remoteIP = (IPEndPoint)udpClient.Client.LocalEndPoint; //Адрес, с которого поступают данные
                        try
                        {
                            byte[] data = udpClient.Receive(ref remoteIP);
                            string message = Encoding.Unicode.GetString(data);
                            Console.WriteLine("Ответ сервера: {0}", message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Возникла ошибка. Подробности:" + e.Message);
                        }*/
        }
    }
}
