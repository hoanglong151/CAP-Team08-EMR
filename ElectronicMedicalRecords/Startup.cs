using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ElectronicMedicalRecords.Startup))]
namespace ElectronicMedicalRecords
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
