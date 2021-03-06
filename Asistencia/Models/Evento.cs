//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Asistencia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Evento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Evento()
        {
            this.Asistentes = new HashSet<Asistente>();
        }
    
        public int intIdEvento { get; set; }
        [Required(ErrorMessage = "El tema es requerido")]
        [StringLength(200, ErrorMessage = "M�ximo 200 caracteres")]
        public string strTema { get; set; }
        [Required(ErrorMessage = "El facilitador es requerido")]
        [StringLength(100, ErrorMessage = "M�ximo 100 caracteres")]
        public string strFacilitador { get; set; }
        [Required(ErrorMessage = "El lugar es requerido")]
        [StringLength(100, ErrorMessage = "M�ximo 100 caracteres")]
        public string strLugar { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "La fecha es requerida")]
        public System.DateTime dtmFecha { get; set; }
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "La duraci�n es requerida")] 
        public System.TimeSpan dtmDuracion { get; set; }
        public bool bitEstado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Asistente> Asistentes { get; set; }
    }
}
