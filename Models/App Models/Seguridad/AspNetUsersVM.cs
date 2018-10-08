using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Proyecto.Models
{
    [MetadataType(typeof(AspNetUsersMetadata))]
    public partial class AspNetUsers
    {
    }

    public class AspNetUsersMetadata
    {
        //[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es requerido.")]
        public string Email { get; set; }

        [DisplayName("Usuario")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es requerido.")]
        public string UserName { get; set; }

        //[DisplayName("Contraseña")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es requerido.")]
        //[StringLength(20, MinimumLength = 5)]
        //public string Password { get; set; }

        //public string PasswordHash { get; set; }
    }
}