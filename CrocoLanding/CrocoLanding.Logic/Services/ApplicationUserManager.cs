using Croco.Core.Implementations.TransactionHandlers;
using CrocoLanding.Logic.Extensions;
using CrocoLanding.Model.Entities.Clt;
using CrocoLanding.Model.Entities.Clt.Default;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CrocoLanding.Logic.Services
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public Task<Client> GetClientAsync(ClaimsPrincipal claimsPrincipal)
        {
            if (!claimsPrincipal.Identity.IsAuthenticated)
            {
                return Task.FromResult<Client>(null);
            }

            var userId = claimsPrincipal.GetUserId();

            return CrocoTransactionHandler.System.ExecuteAndCloseTransaction(amb => amb.RepositoryFactory.Query<Client>().FirstOrDefaultAsync(x => x.Id == userId));
        }
    }
}