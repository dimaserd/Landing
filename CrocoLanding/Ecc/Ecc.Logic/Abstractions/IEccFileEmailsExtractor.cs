using Croco.Core.Abstractions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecc.Logic.Abstractions
{
    public interface IEccFileEmailsExtractor
    {
        /// <summary>
        /// Получить список emailов из файла 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<BaseApiResponse<List<string>>> ExtractEmailsListFromFile(string filePath);
    }
}