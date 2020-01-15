//using Croco.Core.Abstractions;
//using Croco.Core.Abstractions.Application;
//using Croco.Core.Logic.Models.Users;
//using Croco.Core.Logic.Workers;
//using Croco.Core.Model.Abstractions.Entity;
//using Croco.Core.Model.Entities.Application;
//using Croco.Core.Models;
//using Croco.Core.Search.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Zoo.Ecc.Entities.Chat;
//using Zoo.Ecc.Models.Chat;

//namespace Zoo.Ecc.Services
//{
//    public class ChatService<TApplication, TChat, TChatMessage, TChatUserRelation, TUser, TChatMessageAttachment, TFile> : BaseCrocoWorker<TApplication>
//        where TApplication : class, ICrocoApplication
//        where TChat : Chat<TChatUserRelation, TUser, TChatMessage, TChatMessageAttachment, TFile>
//        where TChatUserRelation : ChatUserRelation<TUser, TChat>
//        where TChatMessage : ChatMessage<TChat, TUser, TChatMessageAttachment, TFile>, new()
//        where TChatMessageAttachment : ChatMessageAttachment<TChatMessage, TFile>
//        where TFile : DbFileIntId
//        where TUser : class, ICrocoUser

//    {
//        public ChatService(ICrocoAmbientContext context) : base(context)
//        {
//        }

//        

//}