using System.ServiceProcess;
using System.ServiceModel;

namespace RestWindowsService
{
    public partial class Server : ServiceBase
    {
        ServiceHost serviceHost = null;
        public Server()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            serviceHost = new ServiceHost(typeof(RestServiceLibrary.RestServiceLibrary));
            serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
}