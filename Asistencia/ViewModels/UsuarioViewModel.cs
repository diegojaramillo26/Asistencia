using Asistencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asistencia.ViewModels
{
    public class UsuarioViewModel
    {
        public Usuario Usuario { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}