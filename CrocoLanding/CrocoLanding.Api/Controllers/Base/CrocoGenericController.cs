using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Data;
using Croco.Core.Implementations;
using Croco.Core.Implementations.AmbientContext;
using Croco.WebApplication.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Principal;
using Zoo.Core;

namespace CrocoLanding.Api.Controllers.Base
{
    /// <summary>
    /// Обобщенный веб-контроллер с основной логикой
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    public class CrocoGenericController<TContext> : Controller
        where TContext : DbContext
    {
        private readonly Func<IPrincipal, string> _getUserIdFunc;

        private ICrocoRequestContext _requestContext;

        private ICrocoAmbientContext _ambientContext;

        private ICrocoDataConnection _dataConnection;

        /// <inheritdoc />
        public CrocoGenericController(TContext context, Func<IPrincipal, string> getUserIdFunc)
        {
            Context = context;
            _getUserIdFunc = getUserIdFunc;
        }


        #region Свойства

        /// <summary>
        /// Контекст для работы с бд
        /// </summary>
        public TContext Context
        {
            get;
        }

        /// <summary>
        /// Контекст текущего пользователя
        /// </summary>
        protected ICrocoPrincipal CrocoPrincipal => new WebAppCrocoPrincipal(User, _getUserIdFunc);

        /// <summary>
        /// Контекст текущего запроса
        /// </summary>
        protected ICrocoRequestContext RequestContext
        {
            get
            {
                if (_requestContext == null)
                {
                    _requestContext = new MyWebAppCrocoRequestContext(CrocoPrincipal, Request.GetDisplayUrl());
                }

                return _requestContext;
            }
        }

        /// <summary>
        /// Обёртка для контекста окружения
        /// </summary>
        public ICrocoAmbientContext AmbientContext
        {
            get
            {
                if (_ambientContext == null)
                {
                    _ambientContext = new CrocoAmbientContext(Connection);
                }

                return _ambientContext;
            }
        }

        /// <summary>
        /// Соединение к удалённому источнику данных
        /// </summary>
        public ICrocoDataConnection Connection
        {
            get
            {
                if (_dataConnection == null)
                {
                    _dataConnection = new EntityFrameworkDataConnection(Context, RequestContext);
                }

                return _dataConnection;
            }
        }

        
        #endregion

        /// <inheritdoc />
        /// <summary>
        /// Удаление объекта из памяти
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            //Закрываю транзакцию, чтобы выполнились отложенные действия
            Connection.OnTransactionClosed().GetAwaiter().GetResult();

            if (disposing)
            {
                var toDisposes = new IDisposable[]
                {
                    _dataConnection
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