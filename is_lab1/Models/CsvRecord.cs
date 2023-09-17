using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace is_lab1
{
    public class CsvRecord
    {
        #region Variables
        private List<string> records;
        public List<string> Records
        {
            get { return records; }
            set { records = value; }
        }

        private bool isValid;
        public bool IsValid
        {
            get { return isValid; }
        }
        #endregion

        public bool CsvRecordInput(string inputRecord, string inputTypes, string inputNullables)
        {
            char[] delimiters = { ',', ' ', ';' };
            List<string> values = inputRecord.Split(delimiters).ToList();
            List<string> types = inputTypes.Split(delimiters).ToList();
            List<string> nullables = inputNullables.Split(delimiters).ToList();

            List<int> equalityList = new List<int>()
            {
                values.Count(),
                types.Count(),
                nullables.Count()
            };

            bool debug1 = equalityList.Distinct().Skip(1).Any();
            bool debug2 = CsvRecordValidateBools(nullables);
            bool debug3 = CsvRecordValidateDataTypes(types);

            //Проверка на то что каждой записи соответствует параметр
            if (equalityList.Distinct().Skip(1).Any() || //Отличается ли длина массивов с элементами
                !CsvRecordValidateBools(nullables) || //Соответствуют ли данные о null-переменных 1 или 0 
                !CsvRecordValidateDataTypes(types)) //Соответствуют ли типы данных разрешённым(1 - str, 2 - int, 3 - bool, 4 - byte)
            {
                isValid = false;
                return isValid;
            };

            //Валидация по типам данных и заполнение null'ов если присутствуют
            for (int i = 0; i < values.Count(); i++)
            {
                if (!(nullables[i] == "1" && values[i].ToLower() == "null")) //Если переменная nullable и называется null
                {
                    if (!CsvRecordDataTypeValidateParse(values[i], types[i]))
                    {
                        isValid = false;
                        return isValid;
                    }
                }
                else
                    values[i] = "null";
            }

            Records = new List<string>(values);

            isValid = true;
            return isValid;
        }

        private bool CsvRecordDataTypeValidateParse(string inputRecord, string inputType)
        {
            if (inputType == "1") //String
            {
                return true;
            }
            if (inputType == "2") //Integer
            {
                if (Int32.TryParse(inputRecord, out var debugVar))
                    return true;
                return false;
            }
            if (inputType == "3") //Boolean
            {
                if ((inputRecord.ToLower() == "t") || (inputRecord == "1") || (inputRecord.ToLower() == "true"))
                    inputRecord = "true";
                else if ((inputRecord.ToLower() == "f") || (inputRecord == "0") || (inputRecord.ToLower() == "false"))
                    inputRecord = "false";

                if (Boolean.TryParse(inputRecord, out var debugVar))
                    return true;
                return false;
            }
            if (inputType == "4") //Byte
            {
                if (Byte.TryParse(inputRecord, out var debugVar))
                    return true;
                return false;
            }

            return true;
        }

        private bool CsvRecordValidateBools(List<string> inputBooleanList)
        {
            if (inputBooleanList.Any(t => (t == "1" || t == "0")))
                return true;
            return false;
        }

        private bool CsvRecordValidateDataTypes(List<string> inputDTypesList)
        {
            if (inputDTypesList.Any(t => ( t=="4" || t=="3" || t == "2" || t == "1")))
                return true;
            return false;
        }
    }
}
