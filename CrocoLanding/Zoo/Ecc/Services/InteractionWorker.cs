using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Croco.Core.Abstractions.Application;
using Croco.Core.Abstractions.Data.Repository;
using Croco.Core.Application;
using Croco.Core.Logic.Models.Users;
using Croco.Core.Model.Abstractions.Entity;
using Croco.Core.Model.Entities.Application;
using Zoo.Ecc.Entities;
using Zoo.Ecc.Models;
using Zoo.Entities.Messaging;

namespace Zoo.Ecc.Services
{
    public class InteractionWorker<TInteraction, TUser, TAttachment, TInteractionStatusLog, TFile, TInteractionType>
        where TFile : DbFileIntId
        where TAttachment : InteractionAttachment<TInteraction, TFile>, new()
        where TInteractionStatusLog : InteractionStatusLog<TInteraction>, new()
        where TInteraction : Interaction<TUser, TAttachment, TInteractionStatusLog, TInteractionType>, new()
        where TUser : class, ICrocoUser
        where TInteractionType : Enum
    {
        private readonly IRepositoryFactory _repositoryFactory;

        ICrocoApplication App => CrocoApp.Application;

        public InteractionWorker(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public void CreateInteractionsHandled(IEnumerable<CreateInteraction<TInteractionType>> models)
        {
            var newModel = models.ToList();

            newModel.ForEach(x =>
            {
                if (x.AttachmentFileIds == null)
                {
                    x.AttachmentFileIds = new List<int>();
                }
            });

            var now = App.DateTimeProvider.Now;

            var interactions = newModel.Select(model =>
            {
                var id = Guid.NewGuid().ToString();

                var inter = new TInteraction
                {
                    Id = id,
                    TitleText = model.TitleText,
                    MessageText = model.MessageText,
                    MaskItemsJson = model.MaskItemsJson,
                    SendNow = model.SendNow,
                    SendOn = model.SendOn,
                    Type = model.Type,
                    UserId = model.UserId,
                    Attachments = model.AttachmentFileIds.Select(x => new TAttachment
                    {
                        FileId = x
                    }).ToList(),
                };

                var attachments = model.AttachmentFileIds.Select(x => new TAttachment
                {
                    FileId = x,
                    InteractionId = id
                });

                var status = new TInteractionStatusLog
                {
                    Id = Guid.NewGuid().ToString(),
                    InteractionId = id,
                    StartedOn = now,
                    Status = InteractionStatus.Created
                };

                return new Tuple<TInteraction, IEnumerable<TAttachment>, TInteractionStatusLog>(inter, attachments, status);
             }).ToList();

            _repositoryFactory.GetRepository<TInteraction>().CreateHandled(interactions.Select(x => x.Item1));
            _repositoryFactory.GetRepository<TAttachment>().CreateHandled(interactions.SelectMany(x => x.Item2));
            _repositoryFactory.GetRepository<TInteractionStatusLog>().CreateHandled(interactions.Select(x => x.Item3));
        }

        public Task SetStatusForInteractions(IEnumerable<string> interactionIds, InteractionStatus status, string statusDescription)
        {
            return UpdateInteractionStatusesAsync(interactionIds.Select(x => new UpdateInteractionStatus
            {
                Id = x,
                Status = status,
                StatusDescription = statusDescription
            }));
        }


        public Task UpdateInteractionStatusesAsync(IEnumerable<UpdateInteractionStatus> statuses)
        {
            var statusLogRepo = _repositoryFactory.GetRepository<TInteractionStatusLog>();

            var now = App.DateTimeProvider.Now;

            foreach (var status in statuses)
            {
                statusLogRepo.CreateHandled(new TInteractionStatusLog
                {
                    InteractionId = status.Id,
                    StartedOn = now,
                    StatusDescription = status.StatusDescription,
                    Status = status.Status
                });
            }

            return _repositoryFactory.SaveChangesAsync();
        }

        public IQueryable<InteractionDetailedModel<TInteractionType>> GetDetailedQuery()
        {
            return _repositoryFactory.Query<TInteraction>().Select(x => new InteractionDetailedModel<TInteractionType>
            {
                Id = x.Id,
                MaskItemsJson = x.MaskItemsJson,
                MessageText = x.MessageText,
                SendNow = x.SendNow,
                SendOn = x.SendOn,
                Statuses = x.Statuses.OrderByDescending(t => t.StartedOn).Select(t => new InteractionStatusLogModel
                {
                    StartedOn = t.StartedOn,
                    Status = t.Status,
                    StatusDescription = t.StatusDescription
                }).ToList(),
                TitleText = x.TitleText,
                Type = x.Type,
                User = new UserNameAndEmailModel
                {
                    Email = x.User.Email,
                    Id = x.User.Id,
                    Name = x.User.Name
                },
                AttachmentFileIds = x.Attachments.Select(t => t.FileId).ToList()
            });
        }

        public IQueryable<InteractionModel<TInteractionType>> GetInitQuery()
        {
            return _repositoryFactory.Query<TInteraction>().Select(x => new InteractionModel<TInteractionType>
            {
                Id = x.Id,
                MaskItemsJson = x.MaskItemsJson,
                MessageText = x.MessageText,
                SendNow = x.SendNow,
                SendOn = x.SendOn,
                Status = x.Statuses.OrderByDescending(t => t.StartedOn).Select(t => new InteractionStatusLogModel
                {
                    StartedOn = t.StartedOn,
                    Status = t.Status,
                    StatusDescription = t.StatusDescription
                }).FirstOrDefault(),
                TitleText = x.TitleText,
                Type = x.Type,
                User = new UserNameAndEmailModel
                {
                    Email = x.User.Email,
                    Id = x.User.Id,
                    Name = x.User.Name
                },
                AttachmentFileIds = x.Attachments.Select(t => t.FileId).ToList()
            });
        }

        public IQueryable<InteractionModel<TInteractionType>> GetInitQueryToSend()
        {
            var dateNow = CrocoApp.Application.DateTimeProvider.Now;

            return GetInitQuery().Where(x => !x.SendNow && x.SendOn >= dateNow || x.SendNow);
        }
    }
}