using ClosedXML.Excel;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Landing.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = $"C:\\Users\\Дмитрий\\Source\\Repos\\Landing\\Tests\\Landing.ConsoleApp\\Landing.ConsoleApp\\emails.txt";

            var text = File.ReadAllText(filePath);

            var emailAttr = new EmailAddressAttribute();

            var strs = text.Split(',', StringSplitOptions.RemoveEmptyEntries);

            var emails = strs.Where(x => emailAttr.IsValid(x)).Select(x => x.Trim(' ', '"')).Distinct().ToList();

            File.WriteAllText(filePath, string.Join(',', emails));

            Console.WriteLine("Hello World!");
        }
    }
}