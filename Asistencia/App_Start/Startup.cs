using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using System.Configuration;

[assembly: OwinStartup(typeof(Asistencia.App_Start.Startup))]

namespace Asistencia.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Para obtener más información sobre cómo configurar la aplicación, visite https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseGoogleAuthentication(ConfigurationManager.AppSettings["GoogleClientId"], ConfigurationManager.AppSettings["GoogleClientSecret"]);
        }
    }
}
