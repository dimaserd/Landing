using Ecc.Model.Entities.Chats;
using Ecc.Model.Entities.Email;
using Ecc.Model.Entities.IntegratedApps;
using Ecc.Model.Entities.LinkCatch;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Model.Abstractions
{
    public interface IEccDbContext
    {
        DbSet<IntegratedApp> IntegratedApps { get; set; }

        DbSet<IntegratedAppUserSetting> IntegratedAppUserSettings { get; set; }

        DbSet<EmailGroup> EmailGroups { get; set; }

        DbSet<EmailInEmailGroupRelation> EmailInEmailGroupRelations { get; set; }

        /// <summary>
        /// Шаблоны Email сообщений
        /// </summary>
        DbSet<EmailTemplate> EmailTemplates { get; set; }

        #region Сообщения и чаты
        DbSet<EccChat> Chats { get; set; }

        DbSet<EccChatMessage> ChatMessages { get; set; }

        DbSet<EccChatUserRelation> ChatUserRelations { get; set; }

        DbSet<EccChatMessageAttachment> ChatMessageAttachments { get; set; }
        #endregion

        DbSet<EmailLinkCatch> EmailLinkCatches { get; set; }

        DbSet<EmailLinkCatchRedirect> EmailLinkCatchRedirects { get; set; }
    }
}
