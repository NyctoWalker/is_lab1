using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace is_lab9
{
    public class WordManipulator
    {
        private readonly string projPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName+"\\";
        private readonly string[] defaultVals =
        {
            "лабораторной работе №9",
            "COM-объекты",
            "Архитектура ИС",
            "6И111",
            "Иванов А.А.",
            "Доцент (ОИТ, ИШИТР)",
            "Петров В.П.",
            "2023"
        };
        //private readonly string patternName = "lab9_pattern.docx";

        public bool ModifyWordDocument(string newName, List<string> insertValues)
        {
            if (insertValues.Count() != 8)
                return false;
            string path = projPath + "lab9_pattern.docx";

            if (newName == "")
                newName = "new.docx";

            if (!newName.EndsWith(".docx"))
                newName += ".docx";

            string newDoc = projPath + newName;


            if (!File.Exists(path))
                return false;

            File.Copy(path, newDoc, true);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(newDoc, true))
            {
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                int i = 0;

                foreach (BookmarkStart bookmarkStart in mainPart.Document.Descendants<BookmarkStart>())
                {
                    //string bookmarkName = bookmarkStart.Name;
                    BookmarkEnd bookmarkEnd = bookmarkStart.NextSibling<BookmarkEnd>();

                    if (bookmarkEnd != null)
                    {
                        OpenXmlElement elem = bookmarkStart.NextSibling();

                        while (elem != null && !(elem is BookmarkEnd))
                        {
                            OpenXmlElement nextElem = elem.NextSibling();
                            elem.Remove();
                            elem = nextElem;
                        }

                        if (insertValues[i] == "")
                            insertValues[i] = defaultVals[i];
                        insertValues[i] = CleanInvalidXmlChars(insertValues[i]);

                        Run newRun = new Run(new Text($"{insertValues[i]}"));
                        bookmarkStart.Parent.InsertAfter(newRun, bookmarkStart);
                        i++;
                    }
                }

                wordDoc.Dispose();
            }

            return true;
                
        }

        public static string CleanInvalidXmlChars(string text)
        {
            string re = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF\x0400-\x04FF]";
            return Regex.Replace(text, re, "");
        }
    }
}
