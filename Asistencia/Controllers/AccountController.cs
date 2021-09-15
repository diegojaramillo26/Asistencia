using Asistencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

namespace Asistencia.Controllers
{
    public class AccountController : Controller
    {
        private dbAppsEntities db = new dbAppsEntities();
        private const string XsrfKey = "XsrfId";
        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            ActionResult result = View(model);
            if (ModelState.IsValid)
            {
                string password = Common.EncryptAes.Encrypt(model.Password);
                var user = (Usuario)db.Usuarios.Include("Usuario_Rol").Include("Usuario_Rol.Rol")
                                   .Where(x => x.Email == model.Email && x.Clave == password).FirstOrDefault();
                if (user != null)
                {
                    result = SignInUser(user, model.RememberMe, returnUrl);
                }
                else
                {
                    ViewBag.Message = "Usuario o clave inconrrecta";
                }
                
            }

            return result;
        }

        private ActionResult SignInUser(Usuario user, bool rememberMe, string returnUrl)
        {
            ActionResult result;

            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Name, user.Nombres));
            claims.Add(new Claim("FullName", $"{user.Nombres}{user.Apellidos}"));

            if (user.Usuario_Rol != null && user.Usuario_Rol.Any())
            {
                claims.AddRange(user.Usuario_Rol.Select(x => new Claim(ClaimTypes.Role, x.Rol.Nombre)));
            }

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignIn(new AuthenticationProperties() {IsPersistent = rememberMe },identity);

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Url.Action("Index", "Eventos");
            }
            result = Redirect(returnUrl);

            return result;
        }

       public ActionResult SignOutUser()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
  
            // Solicitar redireccionamiento al proveedor de inicio de sesión externo
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            ActionResult Result;

            // Obtener la información devuelta por el proveedor externo
            var LoginInfo = await HttpContext.GetOwinContext().
                Authentication.GetExternalLoginInfoAsync();

            if (LoginInfo == null)
                // No se pudo autenticar.
                Result = RedirectToAction("Login");
            else
            {
                // El usuario ha sido autenticado por el proveedor externo!
                // Obtener la llave del proveedor que identifica al usuario.
                string ProviderKey = LoginInfo.Login.ProviderKey;
                // Buscar al usuario

                var User = db.Usuarios.FirstOrDefault(x => x.ProviderKey == ProviderKey);
                if (User != null)// Se ha encontrado al usuario. Iniciar la sesión del usuario.                    
                    Result = SignInUser(User, false, returnUrl);
                else { 
                    Usuario newUser = new Usuario {Nombres =LoginInfo.DefaultUserName,
                                                    Apellidos = LoginInfo.DefaultUserName,
                                                    Clave= Common.EncryptAes.Encrypt("@s1st3nc1@"),
                                                    Email =LoginInfo.Email,
                                                    ProviderKey = ProviderKey,
                                                    ProviderName = LoginInfo.Login.LoginProvider
                                                  
                                                    };
                    newUser.Usuario_Rol.Add(new Usuario_Rol { Id_rol = 2 });
                    db.Usuarios.Add(newUser);
                    db.SaveChanges();
                    Result = Content($"Imposible iniciar sesión con {LoginInfo.Login.LoginProvider}");
                }
            }
            return Result;
        }
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        //public void CreateUser() { 
        //     Usuario user = new Usuario
        //     { Nombres = "diego"
        //       , Apellidos = "jaramillo"
        //        , Email = "djevolucion@gmail.com"
        //     , Clave = Common.EncryptAes.Encrypt("12345") };


        //         db.Usuarios.Add(user);
        //         db.SaveChanges();



        // }
    }
}