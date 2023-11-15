using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using is_lab6.Models;

namespace is_lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new();
            ConsoleKey choice;
            bool isPrevValid = true;
            DBManipulator db = new();

            while (true)
            {
                if (isPrevValid == true)
                {
                    Console.WriteLine("Внимание! Парсер работает только с товарами с сайта 2droida.ru!");
                    Console.WriteLine("Нажмите 1 - добавить новую запись из введённого url");
                    Console.WriteLine("Нажмите 2 - просмотреть существующие записи");
                    Console.WriteLine("Назмите Esc - выйти");
                    Console.WriteLine();
                }
                else
                    isPrevValid = true;

                choice = Console.ReadKey(true).Key;

                switch (choice)
                {
                    case ConsoleKey.D1:
                        List<string> result = new();

                        Console.WriteLine("Введите полный url или часть после \"collection\\\"");
                        string url = Console.ReadLine();

                        result = parser.Parse(url);
                        foreach (string i in result)
                        {
                            Console.WriteLine(i);
                        }

                        while (true)
                        {
                            Console.WriteLine("Хотите добавить этот предмет в БД? (Y/N)");
                            string input = Console.ReadLine();
                            if (input.ToLower() == "y")
                            {
                                try
                                {
                                    db.DBAdd(result[0], long.Parse(result[1]), result[2]);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine($"Возникла ошибка. Содержание: {e.Message}");
                                }

                                break;
                            }
                            else if (input.ToLower() == "n")
                                break;
                        }

                        Console.WriteLine();

                        break;



                    case ConsoleKey.D2:
                        foreach (string s in db.DBReadByNumber())
                        {
                            Console.WriteLine(s);
                        }

                        break;



                    case ConsoleKey.Escape:
                        Environment.Exit(0);

                        break;



                    default:
                        //Console.WriteLine("Недопустимый ввод");
                        isPrevValid = false;

                        break;
                }
            }
        }
    }
}
