namespace RestWindowsService
{
    #region Namespaces
    using System.ComponentModel;
    using System.ServiceProcess;
    #endregion

    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public Installer()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.ServiceName = "JokesWorldServer";
            service.Description = "WCF REST API Hosting Jokes on Windows Service";
            service.DelayedAutoStart = true;
            Installers.Add(process);
            Installers.Add(service);
        }

    }
}








