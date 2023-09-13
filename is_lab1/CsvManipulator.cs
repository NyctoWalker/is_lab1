using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace is_lab1
{
    public class CsvManipulator
    {
        /*private string path;*/
        public string Path
        {
            get {return (Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\lab1.csv"); }
        }

        public CsvManipulator()
        {
            //Создание файла если не существует
            if (!File.Exists(Path))
            {
                using (StreamWriter sw = new StreamWriter(Path, false, Encoding.UTF8))
                {
                    sw.WriteLine("FirstName,LastName,Age,HasLicense"); //License_name, L_ID
                }
            }

            //Создание заголовка
            
        }

        //Запись нового значения
        public void CsvWriteValue(string input)
        {
            char[] delimiters = {',', ' '};
            string[] values = input.Split(delimiters);

            //Проверка чтобы на одну запись приходилось 4 значения
            if (values.Length != 4)
            { 
                Console.WriteLine("\aНекорректный ввод! Ввод должен содержать 4 элемента, разделённых пробелом или запятыми!");
                return;
            }

            //Конвертирование 4 поля в корректный формат, если ввод был не в том регистре или альтернативным
            if ((values[3].ToLower() == "t") || (values[3] == "1") || (values[3].ToLower() == "true"))
                values[3] = "true";
            else if ((values[3].ToLower() == "f") || (values[3] == "0") || (values[3].ToLower() == "false"))
                values[3] = "false";
            
            //Запись в файл, если данные в 3 и 4 полях находятся в нужном формате
            if (!(Byte.TryParse(values[2], out byte age) && Boolean.TryParse(values[3], out bool hasDocument)))
                Console.WriteLine("\aНекорректный ввод! Проверьте чтобы первые 2 значения были текстом, третье - числом от 0 до 255, четвёртое - булевым значением(к примеру, 0, f, true, fAlSe)");
            else
            {
                using (StreamWriter sw = new StreamWriter(Path, true, Encoding.UTF8))
                {
                    sw.WriteLine("{0},{1},{2},{3}", values[0], values[1], values[2].ToString(), values[3].ToString());
                }
            }
            
        }

        //Чтение всех значений
        public void CsvReadValue()
        {
            int lineNumber = 0;

            Console.WriteLine("----------------------------");
            using (StreamReader reader = new StreamReader(Path, true))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    //string[] values = line.Split(',');
                    Console.WriteLine("{0}. {1}", lineNumber, line.Replace(',', ' '));
                    lineNumber++;
                }
            }
            Console.WriteLine("----------------------------");
        }

        //Удаление по номеру/номерам
        public void CsvReadByNumber(string input)
        {
            char[] delimiters = { ',', ' ' };
            string[] values = input.Split(delimiters);
            List<int> lineNums = new List<int>();
            List<string> allLines = new List<string>();

            //Приём разрешённых значений и их запись в список
            foreach (string v in values)
            {
                if (!Int32.TryParse(v, out int lineToRead))
                {
                    Console.WriteLine("Некорректный ввод \"{0}\"! Ввод должен содержать число. Значение пропущено.", v);
                    //return;
                }
                else
                {
                    lineNums.Add(lineToRead);
                }
            }

            #region comment
            /*
                        int lineNumber = 0;
                        using (StreamReader reader = new StreamReader(Path))
                        {
                            while (!reader.EndOfStream)
                            {
                                string line = reader.ReadLine();
                                if (lineToRead == lineNumber)
                                {
                                    Console.WriteLine("----------------------------");
                                    Console.WriteLine("{0}. {1}", lineNumber, line.Replace(',', ' '));
                                    Console.WriteLine("----------------------------");
                                    return;
                                }
                                lineNumber++;
                            }
                        }
            Console.WriteLine("Записи с таким номером нет!");*/
            #endregion
            using (StreamReader reader = new StreamReader(Path, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    allLines.Add(line);
                }
            }
            Console.WriteLine("----------------------------");
            foreach (int _int_ in lineNums)
            {
                try
                {
                    Console.WriteLine("{0}. {1}", _int_, allLines[_int_]);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка при выводе строки: "+e.Message);
                }
            }
            Console.WriteLine("----------------------------");
        }

        //Удаление по номеру/номерам
        public void CsvDeleteByNumber(string input)
        {
            string tempPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\temp.csv";
            char[] delimiters = { ',', ' ' };
            string[] values = input.Split(delimiters);
            //Hash set - список, который позволяет добавлять только уникальные значения
            HashSet<int> uniqueLineNums = new HashSet<int>();

            foreach (string v in values)
            {
                if (!Int32.TryParse(v, out int lineToRead))
                {
                    Console.WriteLine("Некорректный ввод \"{0}\"! Ввод должен содержать число. Значение пропущено.", v);
                    //return;
                }
                else
                {
                    //Запрет на удаление предустановленного заголовка
                    if (lineToRead !=0) 
                        uniqueLineNums.Add(lineToRead);
                }
            }

            using (StreamReader reader = new StreamReader(Path))
            using (StreamWriter writer = new StreamWriter(tempPath))
            {
                int lineNumber = 0;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (!uniqueLineNums.Contains(lineNumber))
                    {
                        writer.WriteLine(line);
                    }
                    else
                    {
                        Console.WriteLine("Удалено: {0}", line);
                    }
                    lineNumber++;
                    
                }
            }

            File.Delete(Path);
            File.Move(tempPath, Path);
        }
    }
}
