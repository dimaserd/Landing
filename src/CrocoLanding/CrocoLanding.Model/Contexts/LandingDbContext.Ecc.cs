using Ecc.Model.Abstractions;
using Ecc.Model.Entities.Chats;
using Ecc.Model.Entities.Email;
using Ecc.Model.Entities.IntegratedApps;
using Microsoft.EntityFrameworkCore;

namespace CrocoLanding.Model.Contexts
{
    public partial class LandingDbContext : IEccDbContext
    {
        public DbSet<IntegratedApp> IntegratedApps { get; set; }

        public DbSet<IntegratedAppUserSetting> IntegratedAppUserSettings { get; set; }

        public DbSet<EmailGroup> EmailGroups { get; set; }

        public DbSet<EmailInEmailGroupRelation> EmailInEmailGroupRelations { get; set; }

        /// <summary>
        /// Шаблоны Email сообщений
        /// </summary>
        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        #region Сообщения и чаты
        public DbSet<EccChat> Chats { get; set; }

        public DbSet<EccChatMessage> ChatMessages { get; set; }

        public DbSet<EccChatUserRelation> ChatUserRelations { get; set; }

        public DbSet<EccChatMessageAttachment> ChatMessageAttachments { get; set; }
        #endregion

    }
}