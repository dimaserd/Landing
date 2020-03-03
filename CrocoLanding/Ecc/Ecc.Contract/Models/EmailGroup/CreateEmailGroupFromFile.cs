using System.ComponentModel.DataAnnotations;

namespace Ecc.Contract.Models.EmailGroup
{
    public class CreateEmailGroupFromFile : CreateEmailGroup
    {
        [Required(ErrorMessage = "Необходимо указать название файла")]
        public string FileName { get; set; }
    }
}