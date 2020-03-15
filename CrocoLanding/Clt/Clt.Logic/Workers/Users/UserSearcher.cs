using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Croco.Core.Abstractions;
using Clt.Contract.Models.Common;
using Clt.Logic.Models;
using Clt.Logic.Extensions;
using Croco.Core.Logic.Workers;
using CrocoLanding.Model.Entities.Clt.Default;
using CrocoLanding.Model.Entities.Clt;

namespace Clt.Logic.Workers.Users
{
    /// <summary>
    /// Класс предоставляющий методы для поиска пользователей
    /// </summary>
    public class UserSearcher : BaseCrocoWorker
    {
        #region Методы получения одного пользователя

        public Task<ApplicationUserBaseModel> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return GetUserByPredicateExpression(x => x.PhoneNumber == phoneNumber);
        }

        public Task<ApplicationUserBaseModel> GetUserByIdAsync(string userId)
        {   
            return GetUserByPredicateExpression(x => x.Id == userId);
        }


        public Task<ApplicationUserBaseModel> GetUserByEmailAsync(string email)
        {
            return GetUserByPredicateExpression(x => x.Email == email);
        }

        private IQueryable<ClientJoinedWithApplicationUser> GetInitialJoinedQuery()
        {
            return ClientExtensions
                .GetInitialJoinedQuery(Query<ApplicationUser>(), Query<Client>());
        }

        private Task<ApplicationUserBaseModel> GetUserByPredicateExpression(Expression<Func<ApplicationUserBaseModel, bool>> predicate)
        {
            return GetInitialJoinedQuery().Select(ClientExtensions.SelectExpression).FirstOrDefaultAsync(predicate);
        }

        #endregion

        public UserSearcher(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }
    }
}