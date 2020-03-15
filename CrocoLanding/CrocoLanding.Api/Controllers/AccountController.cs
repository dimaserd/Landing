using Clt.Contract.Models.Account;
using Clt.Contract.Models.Users;
using Clt.Logic.Models.Account;
using Clt.Logic.Workers.Account;
using Croco.Core.Abstractions.Models;
using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using CrocoLanding.Model.Entities.Clt.Default;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class AccountController : BaseApiController
    {
        public AccountController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor) : base(context, signInManager, userManager, httpContextAccessor)
        {
        }

        private AccountManager AccountManager => new AccountManager(AmbientContext);

        private AccountLoginWorker AccountLoginWorker => new AccountLoginWorker(AmbientContext);

        private AccountRegistrationWorker AccountRegistrationWorker => new AccountRegistrationWorker(AmbientContext);

        [HttpPost("Init")]
        public Task<BaseApiResponse> Init()
        {
            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(Context), null, null, null, null);

            return AccountManager.InitAsync(roleManager, UserManager);
        }

        #region Методы логинирования

        /// <summary>
        /// Войти по Email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Login/ByEmail")]
        public Task<BaseApiResponse<LoginResultModel>> Login([FromForm]LoginModel model)
        {
            return AccountLoginWorker.LoginAsync(model, SignInManager);
        }

        /// <summary>
        /// Войти по номеру телефона
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Login/ByPhone")]
        public Task<BaseApiResponse<LoginResultModel>> LoginByPhone([FromForm]LoginByPhoneNumberModel model)
        {
            return AccountLoginWorker.LoginByPhoneNumberAsync(model, SignInManager);
        }

        #endregion

        #region Методы регистрации

        /// <summary>
        /// Зарегистрироваться и войти
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("RegisterAndSignIn")]
        public Task<BaseApiResponse<RegisteredUser>> RegisterAndSignIn([FromForm]RegisterModel model)
        {
            return AccountRegistrationWorker.RegisterAndSignInAsync(model, false, UserManager, SignInManager);
        }

        /// <summary>
        /// Зарегистрироваться
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public Task<BaseApiResponse<RegisteredUser>> Register([FromForm]RegisterModel model)
        {
            return AccountRegistrationWorker.RegisterAsync(model, false, UserManager);
        }
        #endregion
    }
}