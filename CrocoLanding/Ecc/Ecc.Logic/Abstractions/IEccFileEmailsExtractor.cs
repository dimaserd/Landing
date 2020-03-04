using System.Collections.Generic;

namespace Ecc.Logic.Abstractions
{
    public interface IEccFileEmailsExtractor
    {
        /// <summary>
        /// Получить список emailов из файла 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        List<string> ExtractEmailsListFromFile(string filePath);
    }
}