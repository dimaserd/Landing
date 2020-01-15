using System;
using System.Threading.Tasks;

namespace CrocoLanding.Abstractions
{
    public interface ILoggerManager
    {
        Task LogExceptionAsync(Exception ex);
    }
}