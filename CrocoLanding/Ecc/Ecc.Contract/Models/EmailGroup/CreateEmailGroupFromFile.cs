using System.ComponentModel.DataAnnotations;

namespace Ecc.Contract.Models.EmailGroup
{
    public class CreateEmailGroupFromFile : CreateEmailGroup
    {
        [Required(ErrorMessage = "Необходимо указать название файла")]
        public string FilePath { get; set; }
    }
}