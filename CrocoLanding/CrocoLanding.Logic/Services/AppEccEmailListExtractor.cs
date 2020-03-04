using ClosedXML.Excel;
using Ecc.Logic.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CrocoLanding.Logic.Services
{
    public class AppEccEmailListExtractor : IEccFileEmailsExtractor
    {
        public List<string> ExtractEmailsListFromFile(string filePath)
        {
            using var workBook = new XLWorkbook(filePath);

            var sheet = workBook.Worksheets.First();

            var maybeEmails = new List<string>();

            foreach (var row in sheet.Rows())
            {
                var cell = row.Cell(2);

                var email = cell.GetString();

                maybeEmails.Add(email);
            }

            return maybeEmails.Select(x =>
            {
                if (x.Contains(","))
                {
                    return x.Split(",", StringSplitOptions.RemoveEmptyEntries);
                }

                return new[] { x };
            }).SelectMany(x => x)
            .Select(x => x.Trim())
            .Where(x => new EmailAddressAttribute().IsValid(x)).ToList();
        }
    }
}