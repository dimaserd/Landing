﻿using Ecc.Model.Abstractions;
using Ecc.Model.Consts;
using Ecc.Model.Entities.Chats;
using Ecc.Model.Entities.Email;
using Ecc.Model.Entities.External;
using Ecc.Model.Entities.IntegratedApps;
using Ecc.Model.Entities.Interactions;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Model.Contexts
{
    public class EccDbContext : IEccDbContext
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

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Interaction>()
                .Property(x => x.Id)
                .ValueGeneratedNever();

            InteractionAttachment.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Interaction>()
                .HasIndex(x => new { x.Type })
                .IsUnique(false);

            modelBuilder.Entity<Interaction>()
                .HasDiscriminator<string>(nameof(Interaction.Type))
                .HasValue<UserNotificationInteraction>(EccConsts.InAppNotificationType)
                .HasValue<MailMessageInteraction>(EccConsts.EmailType)
                .HasValue<SmsMessageInteraction>(EccConsts.SmsType);

            EccFile.OnModelCreating(modelBuilder);
            EccUserGroup.OnModelCreating(modelBuilder);
            EccUserInUserGroupRelation.OnModelCreating(modelBuilder);

            EmailInEmailGroupRelation.OnModelCreating(modelBuilder);
            EccChatUserRelation.OnModelCreating(modelBuilder);
            IntegratedApp.OnModelCreating(modelBuilder);
        }
    }
}