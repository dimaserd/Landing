using Ecc.Contract.Models;
using Ecc.Logic.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecc.Implementation.Services
{
    public class AppEccFileService : IEccFileService
    {
        public Task<List<EccFileData>> GetFileDatas(int[] fileIds)
        {
            return Task.FromResult(new List<EccFileData>());
        }
    }
}