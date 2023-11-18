using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace is_lab9
{
    class Program
    {
        static void Main(string[] args)
        {
            WordManipulator wm = new();
            ExcelManipulator em = new();
            MenuHandler mh = new();

            //Если система в UTF-8, то кириллица не будет читаться правильно, по умолчанию в консоли 866-кодовая страница
            //https://ru.stackoverflow.com/questions/1278009/0-%D1%81%D0%B8%D0%BC%D0%B2%D0%BE%D0%BB-%D0%B2%D0%BC%D0%B5%D1%81%D1%82%D0%BE-%D1%80%D1%83%D1%81%D1%81%D0%BA%D0%B8%D1%85-%D0%B1%D1%83%D0%BA%D0%B2-net
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.InputEncoding = Encoding.GetEncoding(866);

            ConsoleKey key;
            bool isPrevValid = true;

            while (true)
            {
                if (isPrevValid == true)
                {
                    Console.WriteLine("Нажмите 1 - создать Word файл по шаблону");
                    Console.WriteLine("Нажмите 2 - создать создать Excel файл по шаблону");
                    Console.WriteLine("Нажмите Esc - выйти");
                    Console.WriteLine();
                }
                else
                    isPrevValid = true;

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                        mh.WordMenu(wm);

                        break;



                    case ConsoleKey.D2:
                        Console.WriteLine("Нажмите 1 - создать Excel-файл со степенным графиком");
                        Console.WriteLine("Нажмите 2 - создать Excel-файл с расчётом банковких процентов за год");
                        Console.WriteLine("Нажмите Esc - выйти в основное меню");
                        while (true)
                        {
                            key = Console.ReadKey(true).Key;
                            if (key == ConsoleKey.D1)
                            {
                                int initNum = 0;
                                float power = 0;
                                Console.WriteLine("Введите начальное значение(целое число, integer)");
                                if (!Int32.TryParse(Console.ReadLine(), out initNum))
                                {
                                    Console.WriteLine("Введено неправильное число!");
                                    break;
                                }

                                Console.WriteLine("Введите степень, которая будет применяться на каждом шаге");
                                if (!float.TryParse(Console.ReadLine(), out power))
                                {
                                    Console.WriteLine("Введено неправильное число!");
                                    break;
                                }

                                Console.WriteLine("Введите новое имя файла(пустая строка для сохранения в new.xlsx)");
                                em.ExcelCreateFile(Console.ReadLine(), initNum, power, false);

                                break;
                            }
                            if (key == ConsoleKey.D2)
                            {
                                int initSum = 0;
                                float percent = 0;

                                Console.WriteLine("Введите начальное значение(целое число, integer)");
                                if (!Int32.TryParse(Console.ReadLine(), out initSum))
                                {
                                    Console.WriteLine("Введено неправильное число!");
                                    break;
                                }

                                Console.WriteLine("Введите годовой процент");
                                if (!float.TryParse(Console.ReadLine(), out percent))
                                {
                                    Console.WriteLine("Введено неправильное число!");
                                    break;
                                }

                                Console.WriteLine("Введите новое имя файла(пустая строка для сохранения в new.xlsx)");
                                em.ExcelCreateFile(Console.ReadLine(), initSum, percent, true);

                                break;
                            }
                            if (key == ConsoleKey.Escape)
                                break;
                        }

                        Console.WriteLine();

                        break;



                    case ConsoleKey.Escape:
                        Environment.Exit(0);

                        break;



                    default:
                        isPrevValid = false;

                        break;

                }
            }

            /*em.ExcelCreateGraph("test");
             if (wm.ModifyWordDocument("lab9_pattern.docx"))
                Console.WriteLine("Done");*/
        }

        public class MenuHandler
        {
            public void WordMenu(WordManipulator wm)
            {
                List<string> wordInput = new();
                string n = "";

                Console.WriteLine("Введите вид отчёта в дательном падеже (лабораторной работе №1, домашнему заданию)");
                wordInput.Add(Console.ReadLine());

                Console.WriteLine("Введите тему работы для отчёта");
                wordInput.Add(Console.ReadLine());

                Console.WriteLine("Введите название дисциплины");
                wordInput.Add(Console.ReadLine());

                Console.WriteLine("Введите группу студента");
                wordInput.Add(Console.ReadLine());

                Console.WriteLine("Введите ФИО студента");
                wordInput.Add(Console.ReadLine());

                Console.WriteLine("Введите должность преподавателя(Доцент (ОИТ, ИШИТР), ассистент)");
                wordInput.Add(Console.ReadLine());

                Console.WriteLine("Введите ФИО преподавателя");
                wordInput.Add(Console.ReadLine());

                Console.WriteLine("Введите год заполнения");
                wordInput.Add(Console.ReadLine());

                Console.WriteLine("Введите название сформированного файла или пустую строку для сохранения в файл new.docx");
                if (wm.ModifyWordDocument(Console.ReadLine(), wordInput))
                    Console.WriteLine("Файл успешно сформирован!");
                else
                    Console.WriteLine("Что-то пошло не так. Проверьте правильность введённых данных или сообщите об ошибке в программе.");

                Console.WriteLine();
            }
        }
        

    }
}
