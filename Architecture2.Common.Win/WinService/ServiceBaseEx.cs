using System.ServiceProcess;
using Architecture2.Common.Win.WinService.Interface;

namespace Architecture2.Common.Win.WinService
{
    public abstract class ServiceBaseEx : ServiceBase
    {
        private IAppRunner _appRunner;

        public void SetAppRunner(IAppRunner appRunner)
        {
            _appRunner = appRunner;
        }

        protected override void OnStart(string[] args)
        {
            _appRunner.OnStart(args);
        }

        protected override void OnStop()
        {
            _appRunner.OnStop();
        }

    }
}