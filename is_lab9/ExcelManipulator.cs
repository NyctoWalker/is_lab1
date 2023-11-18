//using Microsoft.Office.Interop.Excel;
using Aspose.Cells;
using Aspose.Cells.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using ClosedXML.Excel;
//using DocumentFormat.OpenXml;
//using DocumentFormat.OpenXml.Drawing.Charts;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Spreadsheet;

namespace is_lab9
{
    public class ExcelManipulator
    {
        private readonly string projPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\";

        //Bool может быть расширен до byte, вместо аргументов с фиксированным типом может передаваться список значений
        public void ExcelCreateFile(string newFile, int num, float flt, bool isBankPerc) 
        {
            if (newFile is "")
                newFile = "new.xlsx";

            if (!newFile.EndsWith(".xlsx"))
                newFile += ".xlsx";

            string path = projPath + newFile;

            // Создание экземпляра приложения
            Workbook workbook = new Workbook();
            // Создание книги и листа
            Worksheet worksheet = workbook.Worksheets[0];

            if (!isBankPerc)
                ExcelGraph(worksheet, num, flt);
            else
                ExcelBankPercents(worksheet, num, flt);

            // Сохранение файла
            workbook.Save(path);
        }

        private void ExcelGraph(Worksheet worksheet, int initialNumber, float power)
        {
            /*worksheet.Cells[0, 0].PutValue(1);
            worksheet.Cells[0, 0].Value = initialNumber;*/
            double val = (double)initialNumber;

            // Заполнение ячеек
            for (int i = 1; i <= 10; i++)
            {
                worksheet.Cells[i - 1, 0].PutValue(i);
                worksheet.Cells[i - 1, 1].Value = val;
                val = Math.Pow(val, power);
                
            }

            // Построение диаграммы
            int chartIndex = worksheet.Charts.Add(ChartType.Scatter, 1, 3, 31, 18);
            Chart chart = worksheet.Charts[chartIndex];
            chart.Title.Text = "Построенный график";
            chart.ShowLegend = false;
            chart.ValueAxis.AxisLine.IsVisible = true;
            chart.CategoryAxis.AxisLine.IsVisible = true;

            chart.SetChartDataRange("A1:B10", true);

            // Соединение точек графика линией
            chart.NSeries[0].Type = ChartType.Line;
            
        }

        private void ExcelBankPercents(Worksheet worksheet, int initialSum, float annualPercent)
        {
            worksheet.Cells[0, 0].Value = "Месяц";
            worksheet.Cells[0, 1].Value = "Сумма";
            worksheet.Cells[0, 3].Value = "Годовой процент";
            worksheet.Cells[0, 4].Value = "Месячный процент(простой)";

            worksheet.Cells[1, 0].PutValue(1);
            worksheet.Cells[1, 1].PutValue(initialSum);
            worksheet.Cells[1, 3].Value = $"{annualPercent}%";
            worksheet.Cells[1, 4].Value = $"{annualPercent / 12}%";

            float coef = annualPercent / 100 / 12 + 1;

            for (int i = 2; i <= 12; i++)
            {
                worksheet.Cells[i, 0].PutValue(i);
                worksheet.Cells[i, 1].Value = worksheet.Cells[i - 1, 1].FloatValue * coef;
            }

            worksheet.Cells[3, 3].Value = "Прибавка за год";
            worksheet.Cells[4, 3].Value = worksheet.Cells[12, 1].FloatValue - worksheet.Cells[1, 1].FloatValue;
        }

    }
}

/*
            public void ExcelCreateGraph(string newFile)
            {
                if (!newFile.EndsWith(".xlsx"))
                    newFile += ".xlsx";

                string path = projPath + newFile;
                Dictionary<string, int> data = new Dictionary<string, int>();
                data.Add("abc", 1);


                SpreadsheetDocument document = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);

                //Создание структуры excel-файла
                WorkbookPart workbookpart = document.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                //Добавление объекта листа
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                //Добавления списка листов
                Sheets sheets = document.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                //Добавление листа с названием Name
                Sheet sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Лист 1" };
                sheets.Append(sheet);

                #region test_code

                // Add a new drawing to the worksheet.
                DrawingsPart drawingsPart = worksheetPart.AddNewPart<DrawingsPart>();
                worksheetPart.Worksheet.Append(new Drawing()
                { Id = worksheetPart.GetIdOfPart(drawingsPart) });
                worksheetPart.Worksheet.Save();

                // Add a new chart and set the chart language to English-US.
                ChartPart chartPart = drawingsPart.AddNewPart<ChartPart>();
                chartPart.ChartSpace = new ChartSpace();
                chartPart.ChartSpace.Append(new EditingLanguage() { Val = new StringValue("en-US") });
                Chart chart = chartPart.ChartSpace.AppendChild(new Chart());

                // Create a new clustered column chart.
                PlotArea plotArea = chart.AppendChild<PlotArea>(new PlotArea());
                Layout layout = plotArea.AppendChild<Layout>(new Layout());
                BarChart barChart = plotArea.AppendChild<BarChart>(new BarChart(new BarDirection()
                { Val = new EnumValue<BarDirectionValues>(BarDirectionValues.Column) },
                    new BarGrouping() { Val = new EnumValue<BarGroupingValues>(BarGroupingValues.Clustered) }));

                uint i = 0;

                // Iterate through each key in the Dictionary collection and add the key to the chart Series
                // and add the corresponding value to the chart Values.
                foreach (string key in data.Keys)
                {
                    BarChartSeries barChartSeries = barChart.AppendChild<BarChartSeries>
                        (new BarChartSeries(new DocumentFormat.OpenXml.Drawing.Charts.Index() { Val = new UInt32Value(i) },
                        new Order() { Val = new UInt32Value(i) },
                        new SeriesText(new NumericValue() { Text = key })));

                    StringLiteral strLit = barChartSeries.AppendChild<CategoryAxisData>
                    (new CategoryAxisData()).AppendChild<StringLiteral>(new StringLiteral());
                    strLit.Append(new PointCount() { Val = new UInt32Value(1U) });
                    strLit.AppendChild<StringPoint>(new StringPoint() { Index = new UInt32Value(0U) })
                .Append(new NumericValue("Лист 1"));

                    NumberLiteral numLit = barChartSeries.AppendChild<DocumentFormat.
                OpenXml.Drawing.Charts.Values>(new DocumentFormat.OpenXml.Drawing.Charts.Values()).AppendChild<NumberLiteral>
                    (new NumberLiteral());
                    numLit.Append(new FormatCode("General"));
                    numLit.Append(new PointCount() { Val = new UInt32Value(1U) });
                    numLit.AppendChild<NumericPoint>(new NumericPoint() { Index = new UInt32Value(0u) })
                    .Append(new NumericValue(data[key].ToString()));

                    i++;
                }
                    #endregion
                    //filling the row is complicated without another library
                    //https://learn.microsoft.com/ru-ru/office/open-xml/how-to-insert-a-chart-into-a-spreadsheet

                    //Сохранение листа и сохранение-закрытие файла
                    workbookpart.Workbook.Save();
                document.Dispose();

            }
            */
