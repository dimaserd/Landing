using Croco.Core.Abstractions.Models;
using Croco.Core.Search.Models;
using Croco.WebApplication.Models.Exceptions;
using Croco.WebApplication.Models.Log;
using Croco.WebApplication.Models.Log.Search;
using Croco.WebApplication.Workers.Log;
using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Контроллер предоставляющий методы логгирования
    /// </summary>
    [Route("Api/Log")]
    public class LogController : BaseApiController
    {
        public LogController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor) : base(context, signInManager, userManager, httpContextAccessor)
        {
        }

        ExceptionWorker ExceptionWorker => new ExceptionWorker(AmbientContext);

        UserInterfaceLogWorker UserInterfaceLogWorker => new UserInterfaceLogWorker(AmbientContext);

        LogsSearcher LogsSearcher => new LogsSearcher(AmbientContext);

        /// <summary>
        /// Получить исключения на сервере
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ServerExceptions")]
        public Task<GetListResult<LoggedServerException>> GetServerExceptions(SearchServerActions model)
        {
            return ExceptionWorker.GetServerExceptionsAsync(model);
        }

        /// <summary>
        /// Получить логи на сервере
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ServerLogs")]
        public Task<GetListResult<ServerLog>> GetServerLogs(SearchServerActions model) => LogsSearcher.GetServerLogsAsync(model);

        /// <summary>
        /// Залогировать исключения
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Exceptions")]
        public Task<BaseApiResponse> LogExceptions([FromForm]List<LogUserInterfaceException> model)
            => ExceptionWorker.LogUserInterfaceExceptionsAsync(model);

        /// <summary>
        /// Залогировать одно исключение
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Exception")]
        public Task<BaseApiResponse> LogException([FromForm]LogUserInterfaceException model)
            => ExceptionWorker.LogUserInterfaceExceptionAsync(model);


        /// <summary>
        /// Залогировать одно событие или действие
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Action")]
        public Task<BaseApiResponse> LogAction([FromForm]LoggedUserInterfaceActionModel model)
            => UserInterfaceLogWorker.LogActionAsync(model);
    }
}