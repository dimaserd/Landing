using System.ComponentModel.DataAnnotations.Schema;
using Croco.Core.Model.Abstractions.Entity;
using Croco.Core.Model.Models;
using Newtonsoft.Json;

namespace Zoo.Clt.Entities
{
    public class WebApplicationUser : AuditableEntityBase, ICrocoUser
    {
        public string Id { get; set; }
        public string Email { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic { get; set; }
    }

    public class WebApplicationUser<TAvatarFile> : WebApplicationUser, ICrocoUser<TAvatarFile> where TAvatarFile : class
    {
        /// <inheritdoc />
        /// <summary>
        /// Идентификатор файла с аватаром пользователя
        /// </summary>
        [ForeignKey(nameof(AvatarFile))]
        public int? AvatarFileId { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Аватар пользователя
        /// </summary>
        [JsonIgnore]
        public virtual TAvatarFile AvatarFile { get; set; }
    }
}