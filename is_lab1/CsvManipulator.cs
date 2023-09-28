using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace is_lab1
{
    public class CsvManipulator
    {
        #region Variables

        private CsvConfig csvConfig;
        public CsvConfig CsvConfig
        {
            get { return csvConfig; }
        }

        private List<CsvRecord> records;
        public List<CsvRecord> Records
        {
            get { return records; }
            set { records = value; }
        }

        private string configPath
        {
            get { return (Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\config.ini"); }
        }

        private string Path
        {
            get {return (CsvConfig.CsvPath); }
        }

        public string csvHeader;
        #endregion

        public CsvManipulator()
        {
            csvConfig = new CsvConfig(); //singleton
            ConfigRead();
            CsvReadValue();
        }

        public bool ConfigRead() { return csvConfig.CsvConfigTryRead(configPath); } //Читает конфиг и обновляет данные, возвращает false если не получилось



        //Запись нового значения
        public bool CsvWriteValue(string input)
        {
            //char[] delimiters = {',', ' ', ';'};
            CsvRecord _record = new CsvRecord();

            //Запись в файл, если все значения заданы правильно
            if (!(_record.CsvRecordInput(input, csvConfig.DataTypes, csvConfig.DataNullables)))
                return false;
            else
            {
                using (StreamWriter sw = new StreamWriter(Path, true, Encoding.UTF8))
                {
                    sw.WriteLine(input.Replace(' ', ',').Replace(';', ','));
                }
                return true;
            } 
        }

        //Чтение всех значений
        public void CsvReadValue()
        {
            int lineNumber = 0;
            Records = new List<CsvRecord>();
            
            using (StreamReader reader = new StreamReader(Path, true))
            {
                if (lineNumber == 0 && CsvConfig.HasHeader.ToLower() == "true" && !reader.EndOfStream)
                {
                    csvHeader = reader.ReadLine();
                }

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    CsvRecord _record = new CsvRecord();
                    if (_record.CsvRecordInput(line, csvConfig.DataTypes, csvConfig.DataNullables))
                    {
                        Records.Add(_record);
                        lineNumber++;
                    }
                    else
                    {
                        //Console.WriteLine("Запись {0}: неправильный формат данных! Подставляем другую запись...", lineNumber+1);
                    }
                }
            }
        }

        //Чтение по номеру/номерам
        public List<int> CsvReadByNumber(string input)
        {
            char[] delimiters = { ',', ' ', ';' };
            string[] values = input.Split(delimiters);
            List<int> lineNums = new List<int>();

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

            return lineNums;
        }

        //Удаление по номеру/номерам
        public List<string> CsvDeleteByNumber(string input)
        {
            char[] delimiters = { ',', ' ', ';' };
            string[] values = input.Split(delimiters);
            //Hash set - список, который позволяет добавлять только уникальные значения
            HashSet<int> uniqueLineNums = new HashSet<int>();
            List<string> deletedRecords = new List<string>();

            foreach (string v in values)
            {
                if (!Int32.TryParse(v, out int lineToRead))
                    { }//Console.WriteLine("Некорректный ввод \"{0}\"! Ввод должен содержать число. Значение пропущено.", v);
                else if (lineToRead <= Records.Count() && lineToRead > 0)
                {
                    uniqueLineNums.Add(lineToRead);
                }
                        
            }

            //Сортировка в порядке убывания
            var sortedUnique = uniqueLineNums.OrderByDescending(i => i);
            foreach (int num in sortedUnique)
            {
                //Console.WriteLine("Удалено: {0}. {1}", num, string.Join(" ", Records[num - 1].Records));
                deletedRecords.Add(string.Join(" ", Records[num - 1].Records));
                Records.RemoveAt(num-1);
            }

            using (StreamWriter sw = new StreamWriter(Path, false, Encoding.UTF8))
            {
                sw.WriteLine(csvHeader);
                foreach (var _record in Records)
                {
                    sw.WriteLine(string.Join(",", _record.Records));
                }
            }
            return deletedRecords;
        }
    }
}
