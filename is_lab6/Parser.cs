using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace is_lab6
{
    //dotnet ef dbcontext scaffold "server=localhost;port=3306;user=root;password=<password>;database=<dbname>" Pomelo.EntityFrameworkCore.MySql -o Models
    public class Parser
    {
        const string siteUrl = "https://2droida.ru/collection/";
        ChromeOptions options;

        public Parser()
        {
            options = new ChromeOptions();
            options.AddArgument("--ignore-certificate-errors");
            //options.AddArgument("log-level=3");
            options.AddArgument("--ignore-certificate-errors-spki-list");

        }

        public/* async*/ List<string> Parse(string relationalUrl)
        {
            List<string> output = new();
            string stts = "";
            var driver = new ChromeDriver(options);
            if (relationalUrl.Contains("https://2droida.ru"))
            {
                driver.Navigate().GoToUrl(relationalUrl);
            }
            else
            {
                driver.Navigate().GoToUrl(siteUrl + relationalUrl);
            }

            var title = driver.FindElement(By.XPath("/html/body/div[2]/main/div[2]/div/form/div[2]/h1"));
            var stats = driver.FindElements(By.XPath("/html/body/div[2]/main/div[2]/div/form/div[8]/div[2]/div[2]/div")).Take(10);
            var price = driver.FindElement(By.ClassName("product__price-cur"));

            string priceNumber = Regex.Replace(price.Text, @"\D", "");

            output.Add(title.Text);
            output.Add(priceNumber);


            foreach (var stat in stats)
            {
                var name = stat.FindElement(By.ClassName("product__property-name")).Text;
                var value = stat.FindElement(By.ClassName("product__property-value")).Text;
                stts = stts + $"{name} {value}; ";
                
            }
            output.Add(stts);

            driver.Quit();
            return output;
        }
        

    }
}

/*IWebElement element = driver.FindElement(By.ClassName("gLFyf"));
element.SendKeys("DNS");
element.Submit();*/

//var titles = driver.FindElements(By.XPath("//*[@id=\"rso\"]/div[2]/div/div/div/div/div[1]/div/div/span/a/h3"));
/*var titles = driver.FindElements(By.CssSelector(".MBeuO"));
var descs = driver.FindElements(By.CssSelector(".lEBKkf"));
for (int i = 0; i < titles.Count; i++)
{
    try
    {
        if (i < descs.Count)
        {
            Console.WriteLine($"Название: {titles[i].Text};\nОписание: {descs[i].Text};\n");
        }
        else
        {
            Console.WriteLine($"Название: {titles[i].Text};\nОписание: недоступно;\n");
        }
        //Console.WriteLine($"Название: {titles[i].Text};\nОписание: {descs[i].Text};\n"); 
    }
    catch (Exception e) { Console.WriteLine($"Ошибка. Подробности: {e.Message}\n"); }
}*/
