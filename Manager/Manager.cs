using DataModelLib;

namespace Manager
{
    public class Manager
    {
        static Manager _instance;
        private readonly SystemWatcher _systemWatcher;

        protected Manager()
        {
            _systemWatcher = new SystemWatcher();            
        }

        public static Manager Instance()
        {
            return _instance ?? (_instance = new Manager());
        }

        public void Run()
        {
            _systemWatcher.StartWatch();   
        }

        public void Stop()
        {
            _systemWatcher.StopWatch();
        }
    }
}
