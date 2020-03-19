﻿using Croco.Core.Abstractions.Models;
using Croco.Core.Application;
using Croco.Core.Utils;
using Ecc.Logic.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Ecc.Implementation.Services
{
    public class AppEccEmailListExtractor : IEccFileEmailsExtractor
    {
        public async Task<BaseApiResponse<List<string>>> ExtractEmailsListFromFile(string filePath)
        {
            filePath = CrocoApp.Application.MapPath(filePath);

            if (!File.Exists(filePath))
            {
                return new BaseApiResponse<List<string>>(false, "Файл не существует по указанному пути");
            }

            var res = Tool.JsonConverter.Deserialize<List<string>>(await File.ReadAllTextAsync(filePath));

            return new BaseApiResponse<List<string>>(true, "Ok", res);
        }
    }
}