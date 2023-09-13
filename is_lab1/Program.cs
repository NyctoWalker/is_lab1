using System;
using System.IO;
using System.Threading;
/*using csv_manipulator.cs;*/

namespace is_lab1
{

    class Program
    {
        static void Main(string[] args)
        {
            /*int i = 0;*/
            /*Console.CancelKeyPress += delegate {
                Environment.Exit(1);
            };*/

            CsvManipulator CsvReader = new CsvManipulator();
            ConsoleKey choice;

            while (true)
            {
                Console.WriteLine("Добро пожаловать в основное меню!");
                Console.WriteLine("Нажмите 1 - посмотреть записи в файле");
                Console.WriteLine("Нажмите 2 - вывести запись по номеру");
                Console.WriteLine("Нажмите 3 - создать новую запись");
                Console.WriteLine("Нажмите 4 - удалить записи из файла");
                Console.WriteLine("Нажмите Esc для выхода");
                Console.WriteLine();
                choice = Console.ReadKey(true).Key;
                switch (choice)
                {
/*                    case 0:
                        {
                            Console.WriteLine("Добро пожаловать в основное меню!");
                            Console.WriteLine("Нажмите 1 - посмотреть записи в файле");
                            Console.WriteLine("Нажмите 2 - вывести запись по номеру");
                            Console.WriteLine("Нажмите 3 - создать новую запись");
                            Console.WriteLine("Нажмите 4 - удалить записи из файла");
                            Console.WriteLine("Нажмите Esc для выхода");
                            if (!Int32.TryParse(Console.ReadLine(), out i))
                                i = -2;
                            Console.WriteLine();
                            break;
                        }*/
                    case ConsoleKey.D1:
                        {
                            CsvReader.CsvReadValue();
                            /*i = 0;*/
                            Console.WriteLine();
                            break;
                        }
                    case ConsoleKey.D2:
                        {
                            Console.WriteLine("Введите номер/номера(через пробелы или запятые) записи:");
                            string newData = Console.ReadLine();
                            CsvReader.CsvReadByNumber(newData);
                            /*i = 0;*/
                            Console.WriteLine();
                            break;
                        }
                    case ConsoleKey.D3:
                        {
                            Console.WriteLine("Введите данные, содержащиеся в новой записи через пробел или запятую(str,str,byte,bool):");
                            string newData = Console.ReadLine();
                            CsvReader.CsvWriteValue(newData);
                            /*i = 0;*/
                            Console.WriteLine();
                            break;
                        }
                    case ConsoleKey.D4:
                        {
                            Console.WriteLine("Введите номер/номера(через пробелы или запятые) записи для удаления:");
                            string newData = Console.ReadLine();
                            CsvReader.CsvDeleteByNumber(newData);
                            /*i = 0;*/
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
                            /*i = 0;*/
                            Console.WriteLine();
                            break;
                        }
                }
            }
        }
    }
}
