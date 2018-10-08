using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Proyecto.Models
{
    public class MenuAccionVM
    {

        public MenuAccionVM()
        {
            menu = new MenuVM();
            accion = new List<Accion>();
        }

        public MenuVM menu{ get; set; }

        public List<Accion> accion{ get; set; }


    }

   
}