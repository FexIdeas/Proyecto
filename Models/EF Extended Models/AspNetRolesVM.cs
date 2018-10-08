using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    [MetadataType(typeof(AspNetRolesMetadata))]
    public partial class AspNetRoles
    {
    }

    public class AspNetRolesMetadata
    {
        /// <summary>
        /// Valida Datos Personales
        /// </summary>

        [Required(AllowEmptyStrings = false, ErrorMessage ="El campo {0} debe contener algun valor.")]
        [Display(Name="Nombre")]
        public string Name { get; set; }


        /// <summary>
        /// Sin Validar
        /// </summary>
        public string Id { get; set; }



        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }

        public virtual ICollection<MenuAspNetRoles> MenuAspNetRoles { get; set; }


    }
}