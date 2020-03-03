using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using Ecc.Logic.Models.IntegratedApps;
using Ecc.Model.Entities.IntegratedApps;
using Ecc.Model.Entities.Interactions;
using Ecc.Model.Enumerations;
using Ecc.Logic.Workers.Base;

namespace Ecc.Logic.Workers.IntegratedApps
{
    public class IntegratedAppWorker : BaseEccWorker
    {
        public IntegratedAppWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }

        
        private BaseApiResponse RaiseAndReturnException(Exception ex)
        {
            Logger.LogException(ex);

            return new BaseApiResponse(false, ex.Message);
        }

        public async Task<BaseApiResponse<List<CreateOrEditApplication>>> GetApplicationsAsync()
        {
            var result = await Query<IntegratedApp>().Select(x => new CreateOrEditApplication
            {
                AppType = x.AppType,
                ConfigurationJson = x.ConfigurationJson,
                Description = x.Description,
                Name = x.Name,
                Uid = x.Uid
            }).ToListAsync();

            return new BaseApiResponse<List<CreateOrEditApplication>>(true, "Успешно", result);
        }

        public async Task<BaseApiResponse> AddApplicationAsync(CreateOrEditApplication model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var repo = GetRepository<IntegratedApp>();

            if (await repo.Query().AnyAsync(x => x.Uid == model.Uid))
            {
                return new BaseApiResponse(false, "Приложение с данным уникальным идентификатором уже существует");
            }

            var app = new IntegratedApp
            {
                AppType = model.AppType,
                ConfigurationJson = model.ConfigurationJson,
                Description = model.Description,
                Name = model.Name,
                Uid = model.Uid
            };

            repo.CreateHandled(app);
            
            return await TrySaveChangesAndReturnResultAsync("Создано интегрированное приложение");
        }

        public async Task<BaseApiResponse> EditApplicationAsync(CreateOrEditApplication model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var repo = GetRepository<IntegratedApp>();

            var app = await repo.Query().FirstOrDefaultAsync(x => x.Uid == model.Uid);

            if (app == null)
            {
                return new BaseApiResponse(false, "Приложение с данным уникальным идентификатором не существует");
            }

            
            app.Name = model.Name;
            app.Description = model.Description;
            app.AppType = model.AppType;
            app.ConfigurationJson = model.ConfigurationJson;

            repo.UpdateHandled(app);

            return await TrySaveChangesAndReturnResultAsync("Настройки интегрированного приложения отредактированы");
        }

        public async Task<BaseApiResponse> AddUserAppSettingIdAsync(AddUserAppSetting model)
        {
            if (!IsAuthenticated)
            {
                return new BaseApiResponse(false, "Вы не авторизованы");
            }
            
            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }


            var app = await Query<IntegratedApp>().FirstOrDefaultAsync(x => x.Uid == model.AppUId);

            if (app == null)
            {
                return new BaseApiResponse(false, "Не найдено интегрированного приложения с указанным идентификатором");
            }

            var repo = GetRepository<IntegratedAppUserSetting>();

            //Нахожу настройки по данному приложению и идентификатору пользователя
            var settings = await repo.Query().Where(x => x.UserId == UserId && x.AppId == app.Id)
                .ToListAsync();

            //Если настройки нет то добавляем ее
            if (settings.Count == 0 || settings.Count > 0 && settings.All(x => x.UserUidInApp != model.UserUidInApp))
            {
                settings.ForEach(x => x.Active = false);

                var setting = new IntegratedAppUserSetting
                {
                    Active = true,
                    AppId = app.Id,
                    UserId = UserId,
                    UserUidInApp = model.UserUidInApp
                };

                repo.CreateHandled(setting);

                return await TrySaveChangesAndReturnResultAsync($"Настройка для пользователя интегрированного приложения '{app.Name}' сохранена");
            }

            var oldSetting = settings.FirstOrDefault(x => x.Active);

            if (oldSetting == null)
            {
                return RaiseAndReturnException(new Exception("Произошла какая то фигня"));
            }

            oldSetting.Active = false;

            repo.UpdateHandled(oldSetting);

            var newSetting = new IntegratedAppUserSetting
            {
                Id = Guid.NewGuid().ToString(),
                Active = true,
                AppId = app.Id,
                UserId = UserId,
                UserUidInApp = model.UserUidInApp
            };

            repo.CreateHandled(newSetting);

            return await TrySaveChangesAndReturnResultAsync($"Добавлена новая настройка для пользователя интегрированного приложения '{app.Name}'");
        }

        public async Task<BaseApiResponse> SendUserNotificationsAsync(List<Interaction> model)
        {
            if (model.Count == 0)
            {
                return new BaseApiResponse(false, "Пустой массив взаимодействий");
            }

            var settingsQuery = Query<IntegratedAppUserSetting>()
                .Include(x => x.App)
                .Where(x => x.Active);

            var userIds = model.Select(x => x.UserId).ToList();

            var minUserId = userIds.Min();

            var maxUserId = userIds.Max();

            var settings = await settingsQuery.Include(x => x.App)
                .Where(x => string.Compare(x.UserId, minUserId, StringComparison.OrdinalIgnoreCase) == 1 &&
                                                          string.Compare(x.UserId, maxUserId, StringComparison.OrdinalIgnoreCase) == -1)
                .GroupBy(x => x.App.Id).ToListAsync();

            return null;
        }
        
        public async Task<BaseApiResponse> SendUserNotificationViaIntegratedApplicationUnsafeAsync(SendUserNotificationViaApplication model)
        {
            var settings = await Query<IntegratedAppUserSetting>()
                .Include(x => x.App)
                .Where(x => x.Active && x.UserId == model.UserId)
                .ToListAsync();

            if (settings.Count == 0)
            {
                return new BaseApiResponse(true, "Уведомления не были отправлены, так как не было найдено подходящих настроек с интегрированными приложениями");
            }

            var tasks = settings.Select(x => GetSendNotificationTask(x, model));

            await Task.WhenAll(tasks);

            return new BaseApiResponse(true, "Уведомления были отправлены");
        }

        private Task GetSendNotificationTask(IntegratedAppUserSetting setting, SendUserNotificationViaApplication model)
        {
            switch (setting.App.AppType)
            {
                case IntegratedAppType.IosApplication:
                    GetSendNotificationTaskForIosApp(setting, model);
                    return Task.CompletedTask;
                case IntegratedAppType.AndroidApplication:
                    return Task.CompletedTask;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void GetSendNotificationTaskForIosApp(IntegratedAppUserSetting setting, SendUserNotificationViaApplication reqModel)
        {
            var client = new RestClient("https://onesignal.com");

            var request = new RestRequest("/api/v1/notifications", Method.POST);

            
            var model = new SendNotificationViaOneSignal
            {
                AppId = setting.App.Uid,
                Data = new Dictionary<string, string>
                {
                    ["foo"] = "bar"
                },
                Contents = new Dictionary<string, string>
                {
                    ["en"] = "English Message",
                    ["ru"] = reqModel.Text
                },
                Headings = new Dictionary<string, string>
                {
                    ["en"] = "111",
                    ["ru"] = reqModel.Title
                },
                IncludePlayerIds = new[]
                {
                    setting.UserUidInApp
                }
            };

            // Json to post.
            var jsonToSend = JsonConvert.SerializeObject(model);

            request.AddHeader("Authorization", $"Basic {setting.App.ConfigurationJson}");
            
            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            try
            {
                client.ExecuteAsync(request, response =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        
                    }
                });
            }
            catch (Exception error)
            {
                Logger.LogException(error);
            }
        }
    }
}