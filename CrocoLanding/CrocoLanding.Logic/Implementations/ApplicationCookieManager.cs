using Croco.Core.Utils;
using Croco.WebApplication.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrocoLanding.Logic.Implementations
{
    public class ApplicationCookieManager : ICookieManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationCookieManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<string> Keys => _httpContextAccessor.HttpContext.Request.Cookies.Keys;

        public void Append(string name, string value)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(name, value);
        }

        public void Append<T>(string name, T value)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(name, Tool.JsonConverter.Serialize(value));
        }

        public bool ContainsKey(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies.Any(x => x.Key == key);
        }

        public T GetValue<T>(string key) where T : class
        {
            if (typeof(T) == typeof(string))
            {
                throw new ApplicationException("нельзя использовать перезагрузку с типом string");
            }

            var cookie = GetValue(key);

            if (cookie == null)
            {
                return null;
            }

            return Tool.JsonConverter.Deserialize<T>(cookie);
        }

        public string GetValue(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[key];
        }

        public void Remove(string key)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }
    }
}
