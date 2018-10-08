using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;

namespace Proyecto.Controllers
{

    public class MenuController : Controller
    {
        ComputosPJEntities Context = new ComputosPJEntities();

        // GET: Menu
        public List<MenuVM> GetMenu()
        {
            var datos = Context.Menu.Where(r => r.Activo).Select(r => new MenuVM()
            {
                ID = r.ID,
                mnuId = r.mnuId,
                PadreID = r.PadreID,
                Nombre = r.Nombre,
                Accion = r.Accion,
                Controlador = r.Controlador,
                Icono = r.Icono
            }).ToList();

            return datos;
        }

        public List<MenuVM> GetMenu(String UserName)
        {

            List<MenuVM> datos = Context.GetPermisosPorNombreDeUsuario(UserName).Select(r => new MenuVM()
            {
                ID = r.ID,
                mnuId = r.mnuId,
                PadreID = r.PadreID,
                Nombre = r.Nombre,
                Accion = r.Accion,
                Controlador = r.Controlador,
                Icono = r.Icono,
                Orden = r.Orden
            }).ToList();

            List<MenuVM> datos_aux = datos.Where(r => r.PadreID != null).ToList();

            foreach (var item in datos_aux)
            {
                if (!datos.Any(r => r.ID == item.PadreID))
                {
                    datos.Add(
                        Context.Menu.Where(r => r.ID == item.PadreID).Select(r => new MenuVM()
                        {
                            ID = r.ID,
                            mnuId = r.mnuId,
                            PadreID = r.PadreID,
                            Nombre = r.Nombre,
                            Accion = r.Accion,
                            Controlador = r.Controlador,
                            Icono = r.Icono,
                            Orden = r.Orden
                        }).FirstOrDefault());
                }
            }

            return datos.OrderBy(r => r.Orden).ToList();
        }
        

        public List<MenuAccionVM> GetPermisos(String UserName)
        {
            ComputosPJEntities db = new ComputosPJEntities();
            List<MenuAccionVM> Permisos = new List<MenuAccionVM>();

            var rolesUser = db.AspNetUsers.FirstOrDefault(r => r.UserName == UserName).AspNetRoles;
            //var rolesUser = db.AspNetUsers.FirstOrDefault(r => r.UserName == UserName).AspNetUserRoles;//.AspNetRoles;

            //Recorro todos los Roles asociados al Usuario
            foreach (var rol in rolesUser)
            {
                //Recorro todos los Menus asociados al Rol
                foreach (var menu in rol.MenuAspNetRoles)
                {
                    MenuAccionVM item = new MenuAccionVM();

                    item.menu.ID = menu.Menu.ID;
                    item.menu.Nombre = menu.Menu.Nombre;

                    //Recorro todas los Acciones asociados al Menu
                    foreach (var accion in menu.MenuAspNetRolesAccion)
                    {
                        item.accion.Add(new Accion
                        {
                            ID = accion.Accion.ID,
                            Nombre = accion.Accion.Nombre
                        });
                    }

                    Permisos.Add(item);
                }
            }

            return Permisos;
        }

    }
}
