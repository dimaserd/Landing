using Croco.Core.Models;
using Croco.Core.Search.Models;
using Croco.WebApplication.Models.Exceptions;
using Croco.WebApplication.Models.Log;
using Croco.WebApplication.Models.Log.Search;
using Croco.WebApplication.Workers.Log;
using CrocoLanding.Logic;
using CrocoShop.Model.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zoo.Core;

namespace CrocoLanding.Api.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Контроллер предоставляющий методы логгирования
    /// </summary>
    [Route("Api/Log")]
    public class LogController : BaseApiController
    {
        /// <inheritdoc />
        public LogController(LandingDbContext context) : base(context)
        {
        }

        ExceptionWorker<LandingWebApplication> ExceptionWorker => new ExceptionWorker<LandingWebApplication>(AmbientContext);

        UserInterfaceLogWorker<LandingWebApplication> UserInterfaceLogWorker => new UserInterfaceLogWorker<LandingWebApplication>(AmbientContext);

        LogsSearcher<LandingWebApplication> LogsSearcher => new LogsSearcher<LandingWebApplication>(AmbientContext);

        /// <summary>
        /// Получить исключения на сервере
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ServerExceptions"), ProducesDefaultResponseType(typeof(GetListResult<LoggedServerException>))]
        public Task<GetListResult<LoggedServerException>> GetServerExceptions(SearchServerActions model)
        {
            return ExceptionWorker.GetServerExceptionsAsync(model);
        }

        /// <summary>
        /// Получить логи на сервере
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ServerLogs"), ProducesDefaultResponseType(typeof(GetListResult<ServerLog>))]
        public Task<GetListResult<ServerLog>> GetServerLogs(SearchServerActions model) => LogsSearcher.GetServerLogsAsync(model);

        /// <summary>
        /// Залогировать исключения
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Exceptions"), ProducesDefaultResponseType(typeof(BaseApiResponse))]
        public Task<BaseApiResponse> LogExceptions([FromForm]List<LogUserInterfaceException> model)
            => ExceptionWorker.LogUserInterfaceExceptionsAsync(model);

        /// <summary>
        /// Залогировать одно исключение
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Exception"), ProducesDefaultResponseType(typeof(BaseApiResponse))]
        public Task<BaseApiResponse> LogException([FromForm]LogUserInterfaceException model)
            => ExceptionWorker.LogUserInterfaceExceptionAsync(model);


        /// <summary>
        /// Залогировать одно событие или действие
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Action"), ProducesDefaultResponseType(typeof(BaseApiResponse))]
        public Task<BaseApiResponse> LogAction([FromForm]LoggedUserInterfaceActionModel model)
            => UserInterfaceLogWorker.LogActionAsync(model);
    }
}