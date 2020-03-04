﻿using Croco.Core.Application;
using Croco.Core.Common.Enumerations;
using System.Linq;
using Clt.Contract.Models.Common;
using System.Linq.Expressions;
using Clt.Logic.Models;
using System;
using Clt.Contract.Models.Users;

namespace Clt.Logic.Extensions
{
    public static class ClientExtensions
    {
        public static Expression<Func<ClientJoinedWithApplicationUser, ApplicationUserBaseModel>> SelectExpression = x => new ApplicationUserBaseModel
        {
            Name = x.Client.Name,
            Surname = x.Client.Surname,
            ObjectJson = x.Client.ObjectJson,
            DeActivated = x.Client.DeActivated,
            Balance = x.Client.Balance,
            BirthDate = x.Client.BirthDate,
            Patronymic = x.Client.Patronymic,
            Sex = x.Client.Sex,
            AvatarFileId = x.Client.AvatarFileId,
            Id = x.User.Id,
            Email = x.User.Email,
            PhoneNumber = x.User.PhoneNumber,
            EmailConfirmed = x.User.EmailConfirmed,
            PhoneNumberConfirmed = x.User.PhoneNumberConfirmed,
            SecurityStamp = x.User.SecurityStamp,
            PasswordHash = x.User.PasswordHash,
            CreatedOn = x.User.CreatedOn,
            RoleNames = x.User.Roles.Select(t => t.Role.Name).ToList()
        };

        public static string GetAvatarLink(this ClientModel user, ImageSizeType imageSizeType)
        {
            var imageId = user?.AvatarFileId;

            return imageId.HasValue ? CrocoApp.Application.FileCopyWorker.GetVirtualResizedImageLocalPath(imageId.Value, imageSizeType) : null;
        }

        

        public static bool Compare(ApplicationUserBaseModel user1, ApplicationUserBaseModel user2)
        {
            var rightsChanged = user1.RoleNames.Count != user2.RoleNames.Count;

            if (!rightsChanged)
            {
                for (var i = 0; i < user1.RoleNames.Count; i++)
                {
                    if (user1.RoleNames.OrderBy(x => x).ToList()[i] == user2.RoleNames.OrderBy(x => x).ToList()[i])
                    {
                        continue;
                    }
                    rightsChanged = true;
                    break;
                }
            }

            return user1.Id == user2.Id &&
                !rightsChanged &&
                user1.Name == user2.Name &&
                user1.AvatarFileId == user2.AvatarFileId &&
                string.IsNullOrEmpty(user1.ObjectJson) == string.IsNullOrEmpty(user2.ObjectJson) &&
                string.IsNullOrEmpty(user1.PhoneNumber) == string.IsNullOrEmpty(user2.PhoneNumber);
        }
    }
}