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
    
    public partial class Evento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Evento()
        {
            this.Asistentes = new HashSet<Asistente>();
        }
    
        public int intIdEvento { get; set; }
        public string strTema { get; set; }
        public string strFacilitador { get; set; }
        public string strLugar { get; set; }
        public Nullable<System.DateTime> dtmFecha { get; set; }
        public Nullable<System.TimeSpan> dtmDuracion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Asistente> Asistentes { get; set; }
    }
}