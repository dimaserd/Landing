using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Extensions;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using CrocoLanding.Model.Entities.Clt.Default;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CrocoLanding.Controllers.Base
{
    /// <inheritdoc />
    /// <summary>
    /// Базовый Mvc-контроллер
    /// </summary>
    public class BaseController : CrocoGenericController<LandingDbContext, ApplicationUser>
    {
        #region Конструкторы

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public BaseController(LandingDbContext context, ApplicationUserManager userManager, ApplicationSignInManager signInManager, IHttpContextAccessor httpContextAccessor)
            : base(context, signInManager, userManager, x => x.GetUserId(), httpContextAccessor)
        {
        }

        #endregion

        #region Поля

        /// <summary>
        /// Поле для менеджера ролей
        /// </summary>
        private RoleManager<ApplicationRole> _roleManager;

        #endregion

        #region Свойства

        /// <summary>
        /// Менеджер для работы с ролями пользователей
        /// </summary>
        protected RoleManager<ApplicationRole> RoleManager => _roleManager ??
                                                           (_roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(Context), null, null, null, null));

        private ApplicationUser _currentUser;

        protected async Task<ApplicationUser> GetCurrentUserAsync()
        {
            if (_currentUser == null)
            {
                _currentUser = await UserManager.GetUserAsync(User);
            }

            return _currentUser;
        }

        #endregion

        /// <inheritdoc />
        /// <summary>
        /// Метод уничтожения объекта Controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var toDisposes = new IDisposable[]
                {
                    UserManager, Context, _roleManager
                };

                for (var i = 0; i < toDisposes.Length; i++)
                {
                    if (toDisposes[i] == null)
                    {
                        continue;
                    }
                    toDisposes[i].Dispose();
                    toDisposes[i] = null;

                }
            }

            base.Dispose(disposing);
        }
    }
}