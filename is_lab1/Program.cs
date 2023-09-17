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
                                Console.WriteLine("{0}. {1}", i, string.Join(" " , _record.Records));
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
                            CsvReader.CsvReadByNumber(newData);
                            Console.WriteLine();
                            break;
                        }
                    case ConsoleKey.D3:
                        {
                            Console.WriteLine("Введите данные, содержащиеся в новой записи через пробел или запятую:");
                            string newData = Console.ReadLine();
                            CsvReader.CsvWriteValue(newData);
                            Console.WriteLine();
                            break;
                        }
                    case ConsoleKey.D4:
                        {
                            Console.WriteLine("Введите номер/номера(через пробелы или запятые) записи для удаления:");
                            string newData = Console.ReadLine();
                            CsvReader.CsvDeleteByNumber(newData);
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
    }
}
