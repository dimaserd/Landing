using Croco.Core.Abstractions.Files;

namespace Ecc.Contract.Models
{
    public class EccFileData : IFileData
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public byte[] Data { get; set; }
    }
}