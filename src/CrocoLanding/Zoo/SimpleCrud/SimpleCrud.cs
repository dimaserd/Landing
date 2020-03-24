using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Data.Entities.HaveId;
using Croco.Core.Abstractions.Models.Search;
using Croco.Core.Logic.Workers;
using Croco.Core.Search.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Zoo.SimpleCrud
{
    public class SimpleCrudServiceForStringId<TEntity, TModel, TCreateOrUpdateModel> : BaseCrocoWorker 
        where TEntity : class, IHaveStringId
        where TModel : class
    {
        public SimpleCrudServiceForStringId(Expression<Func<TEntity, TModel>> selectExpression, ICrocoAmbientContext context) : base(context)
        {
            SelectExpression = selectExpression;
        }

        public Expression<Func<TEntity, TModel>> SelectExpression { get; }

        public Task<GetListResult<TModel>> GetList(GetListSearchModel model)
        {
            return EFCoreExtensions.GetAsync(model, Query<TEntity>().OrderBy(x => x.Id), SelectExpression);
        }
    }
}