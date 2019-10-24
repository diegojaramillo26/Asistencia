using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asistencia.Models
{
    public class Asistencia
    {
        [UIHint("SignaturePad")]
        public byte[] MySignature { get; set; }
    }
}