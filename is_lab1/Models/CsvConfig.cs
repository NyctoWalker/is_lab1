using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace is_lab1
{
    public class CsvConfig
    {

        #region Variables

        private string dataHeaders;
        public string DataHeaders
        {
            get { return dataHeaders; }
            set { dataHeaders = value; }
        }

        private string hasHeader;
        public string HasHeader
        {
            get { return hasHeader; }
        }

        private string dataTypes;
        public string DataTypes
        {
            get { return dataTypes; }
            set { dataTypes = value; }
        }

        private string dataNullables;
        public string DataNullables
        {
                get { return dataNullables; }
                set { dataNullables = value; }
        }

        public string CsvName;
        public string CsvPath
        {
            get { return (Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\" + CsvName); }
        }
        #endregion

        //Проверка, в правильном ли формате находятся данные конфига
        public bool CsvConfigCheckValid(string inputHeaders, string inputTypes, string inputNullables, string inputCsvName)
        {
            char[] delimiters = { ',', ' ', ';' };

            List<int> equalityList = new List<int>()
            {
                inputHeaders.Split(delimiters).Count(),
                inputTypes.Split(delimiters).Count(),
                inputNullables.Split(delimiters).Count()
            };

            if (!equalityList.Distinct().Skip(1).Any() && //Проверка, равное ли количество элементов введено для всех ячеек(для n заголовков n типов данных)
                inputNullables.Split(delimiters).Any(t => (t=="1" || t=="0")) && //Проверка, указаны ли значения nullables как 1 и 0 для преобразования
                inputTypes.Split(delimiters).Any(t => (t == "1" || t == "2" || t == "3" || t=="4") //Разрешённые ли значения вставлены
                )) //По-хорошему разрешённые значения в списках нужно вынести куда-нибудь отдельно
                return true;
            else
                return false;
        }

        //Вставка новых значений в конфиг и замена данных в объекте класса если новая запись случилась
        public bool CsvConfigRewrite(string cPath, string inputHeaders, string inputHasHeader, string inputTypes, string inputNullables, string inputCsvName)
        {
            if (!CsvConfigCheckValid(inputHeaders, inputTypes, inputNullables, inputCsvName))
            {
                return false;
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(cPath, false, Encoding.UTF8))
                {
                    sw.WriteLine(inputHeaders);
                    sw.WriteLine(inputHasHeader);
                    //Data types: 1 - str, 2 - int, 3 - bool, 4 - byte
                    sw.WriteLine(inputTypes);
                    //1 - nullable, 0 not nullable
                    sw.WriteLine(inputNullables);
                    sw.Write(inputCsvName);
                }

                //В идеале, каждая переменная записи должна представлять собой ряд листов. Для заголовков листы со строками, для типов данных byte и т.д.
                DataHeaders = inputHeaders;
                hasHeader = inputHasHeader;
                DataTypes = inputTypes;
                DataNullables = inputNullables;
                CsvName = inputCsvName;

                return true;
            };
        }

        //Чтение конфига и обновления данных в объекте класса
        public bool CsvConfigTryRead(string cPath)
        {
            //Вставляет данные по умолчанию, если файла не существует
            if (!File.Exists(cPath))
            {
                if (CsvConfigRewrite(cPath, "FirstName,LastName,Age,IsEmployed,LicenseID", "true", "1,1,4,3,2", "1,1,1,1,0", "lab1.csv"))
                    return true;
                else //else не обязательна, return закончит выполнение функции
                    return false;
            }
            else
            {
                string _DataHeaders = null, _HasHeader = null, _DataTypes = null, _DataNullables = null, _CsvName = null;

                using (StreamReader reader = new StreamReader(cPath, Encoding.UTF8))
                {
                    if (!reader.EndOfStream)
                    {
                        _DataHeaders = reader.ReadLine();
                        _HasHeader = reader.ReadLine();
                        _DataTypes = reader.ReadLine();
                        _DataNullables = reader.ReadLine();
                        _CsvName = reader.ReadLine();
                    }
                }

                if (!String.IsNullOrEmpty(_CsvName) && CsvConfigCheckValid(_DataHeaders, _DataTypes, _DataNullables, _CsvName))
                {
                    DataHeaders = _DataHeaders;
                    hasHeader = _HasHeader;
                    DataTypes = _DataTypes;
                    DataNullables = _DataNullables;
                    CsvName = _CsvName;

                    return true;
                }
                else
                    return false;
            }
        }
    }
}
