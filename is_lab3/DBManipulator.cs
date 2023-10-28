using is_lab3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace is_lab3
{
    public class DBManipulator
    {

        public List<string> GetDBValues()
        {
            List<string> output = new();
            int i = 1;
            using (is_archContext db = new())
            {
                var people = db.PersonLicenses;
                foreach (PersonLicense p in people)
                {
                    string msg = $"{p.FirstName} {p.LastName} {p.Age} {p.HasDrivingLicense} {p.IdCargoLicense}";
                    output.Add(msg);
                    //ServerSendMessage(msg);
                    i++;
                }
            }

            return output;
        }

        public List<string> GetDBValuesByNumber(string input)
        {
            List<string> output = new();
            int i = 1;
            using (is_archContext db = new())
            {
                var people = db.PersonLicenses;
                foreach (PersonLicense p in people)
                {
                    if (GetValidNumbers(input).Contains(i))
                    {
                        string msg = i.ToString() + ". "
                            + p.FirstName + " "
                            + p.LastName + " "
                            + p.Age + " "
                            + p.HasDrivingLicense + " "
                            + p.IdCargoLicense;
                        output.Add(msg);
                    }
                    //ServerSendMessage(msg);
                    i++;
                }
            }

            return output;
        }

        public List<string> DeleteDBValuesByNumber(string input)
        {
            List<string> output = new();
            //List<string> 
            int i = 1;

            using (is_archContext db = new())
            {
                var people = db.PersonLicenses;
                foreach (PersonLicense p in people)
                {
                    if (GetValidNumbers(input).Contains(i))
                    {
                        try
                        {
                            db.PersonLicenses.Remove(p);
                        }
                        catch (Exception e) { Console.WriteLine("Ошибка удаления из базы данных:" + e.Message); }
                        
                        string msg = "Удалено: " +
                            i.ToString() + ". "
                            + p.FirstName + " "
                            + p.LastName + " "
                            + p.Age + " "
                            + p.HasDrivingLicense + " "
                            + p.IdCargoLicense;
                        output.Add(msg);
                    }
                    //ServerSendMessage(msg);
                    i++;
                }
                db.SaveChanges();
            }

            return output;
        }

        public void DeleteValueByID(string input)
        {
            int id;
            if (!Int32.TryParse(input, out id))
                return;
            else
            {
                using (is_archContext db = new())
                {
                    PersonLicense personToDelete = db.PersonLicenses.Find(id);
                    db.Remove(personToDelete);
                    db.SaveChanges();
                }
            }    
        }

        public void UpdateDB(List<PersonLicense> people)
        {
            using (is_archContext db = new())
            {
                var existingPeople = db.PersonLicenses.ToList();

                // Удаление людей которых нет в списке
                foreach (PersonLicense person in existingPeople)
                {
                    if (!people.Any(p => p.IdCargoLicense == person.IdCargoLicense))
                    {
                        db.PersonLicenses.Remove(person);
                    }
                }

                // Обновить существующих и вставить новых
                foreach (PersonLicense person in people)
                {
                    PersonLicense existingPerson = existingPeople.FirstOrDefault(p => p.IdCargoLicense == person.IdCargoLicense);
                    if (existingPerson != null)
                    {
                        // Обновление
                        existingPerson.FirstName = person.FirstName;
                        existingPerson.LastName = person.LastName;
                        existingPerson.Age = person.Age;
                        existingPerson.HasDrivingLicense = person.HasDrivingLicense;
                        existingPerson.IdCargoLicense = person.IdCargoLicense;
                    }
                    else
                    {
                        // Вставка
                        db.PersonLicenses.Add(person);
                    }
                }

                db.SaveChanges();
            }
        }

        public bool DBAddNew(string input)
        {
            try
            {
                char[] delimiters = { ',', ' ', ';' };
                string[] values = input.Split(delimiters);
                string fName = values[0];
                string lName = values[1];
                string age = values[2];
                string hasLicense = values[3];

                using (is_archContext db = new())
                {
                    PersonLicense p = new()
                    {
                        FirstName = fName,
                        LastName = lName,
                        Age = UInt32.Parse(age),
                        HasDrivingLicense = SByte.Parse(hasLicense)
                    };
                    db.PersonLicenses.Add(p);
                    db.SaveChanges();
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }

            return true;
        }

        private HashSet<int> GetValidNumbers(string input)
        {
            char[] delimiters = { ',', ' ', ';' };
            string[] values = input.Split(delimiters);
            HashSet<int> lineNumbs = new();
            //isUnique = true ? List<int> lineNums = new List<int>() : HashSet<int> lineNums = new HashSet<int>();

            foreach (string v in values)
            {
                if (!Int32.TryParse(v, out int lineToCheck))
                { }//Console.WriteLine("Некорректный ввод \"{0}\"! Ввод должен содержать число. Значение пропущено.", v);
                else if (lineToCheck > 0)
                {
                    lineNumbs.Add(lineToCheck);
                }
            }

            return lineNumbs;
        }
    }
}
