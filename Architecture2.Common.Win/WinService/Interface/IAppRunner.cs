using System;

namespace Architecture2.Common.Win.WinService.Interface
{
    public interface IAppRunner : IDisposable
    {
        void OnStart(string[] args);
        void OnStop();
    }
}