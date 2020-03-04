using System;
using System.Threading.Tasks;

namespace Clt.Logic.Models.Users
{
    public class AsyncCriteria<TResultItem>
    {
        public AsyncCriteria(Func<Task<TResultItem[]>> taskFunc)
        {
            TaskFunc = taskFunc;
        }

        Func<Task<TResultItem[]>> TaskFunc { get; }

        public Task<TResultItem[]> Execute()
        {
            return TaskFunc();
        }
    }
}