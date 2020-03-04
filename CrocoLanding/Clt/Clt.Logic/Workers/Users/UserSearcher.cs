using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CrocoShop.Logic.Workers.Base;
using Microsoft.EntityFrameworkCore;
using Clt.Logic.Models.Users;
using Croco.Core.Abstractions;
using Croco.Core.Search.Models;
using Clt.Contract.Models.Common;
using Clt.Logic.Models;
using CrocoShop.Model.Entities.Clt.Default;
using CrocoShop.Model.Entities.Clt;
using Clt.Logic.Extensions;

namespace Clt.Logic.Workers.Users
{
    /// <summary>
    /// Класс предоставляющий методы для поиска пользователей
    /// </summary>
    public class UserSearcher : BaseWorker
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

        #region Метод получения списка пользователей

        public async Task<GetListResult<ApplicationUserBaseModel>> GetUsersAsync(UserSearch model)
        {
            //TODO проверить как эта лобуда работает

            var clientAsyncCriterias = model.GetAsyncClientCriterias(RequestContext.RequestId);

            var asyncResults = await Task.WhenAll(clientAsyncCriterias.Select(x => x.Execute()));

            var clientIds = asyncResults.SelectMany(x => x).Distinct().OrderBy(x => x).ToList();

            if(clientIds.Count == 0)
            {
                return new GetListResult<ApplicationUserBaseModel>
                {
                    Count = model.Count,
                    OffSet = model.OffSet,
                    List = new System.Collections.Generic.List<ApplicationUserBaseModel>(),
                    TotalCount = 0
                };
            }

            var clientQuery = Query<Client>().Where(x => clientIds.Contains(x.Id));

            var q = ClientExtensions.GetInitialJoinedQuery(Query<ApplicationUser>(), clientQuery);

            var resultList = await q.Select(ClientExtensions.SelectExpression).ToListAsync();

            return new GetListResult<ApplicationUserBaseModel>
            {
                Count = model.Count,
                OffSet = model.OffSet,
                List = resultList,
                TotalCount = clientIds.Count
            };
        }
        #endregion

        public UserSearcher(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }
    }
}