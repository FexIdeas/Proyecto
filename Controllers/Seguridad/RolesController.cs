using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;
using System.Transactions;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Proyecto.Controllers.Seguridad
{
    [Authorize]
    public class RolesController : Controller
    {
        private ComputosPJEntities db = new ComputosPJEntities();

        // GET: Roles
        public ActionResult Index()
        {
            return View(db.AspNetRoles.OrderBy(r=>r.Name).ToList());
        }

        // GET: Roles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            if (aspNetRoles == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRoles);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] AspNetRoles aspNetRoles)
        {
            if (ModelState.IsValid)
            {

                using (var context = new ApplicationDbContext())
                {
                    var roleStore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);

                    await roleManager.CreateAsync(new IdentityRole { Name = aspNetRoles.Name });

                    return RedirectToAction("Index");

                }
            }

            return View(aspNetRoles);
        }



        /// <summary>
        /// Metodo q permite la edicion de un Rol
        /// </summary>
        /// <param name="id">ID del Rol a editar</param>
        /// <returns></returns>
        // GET: Roles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            if (aspNetRoles == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRoles);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] AspNetRoles aspNetRoles)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(aspNetRoles).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");

                using (var context = new ApplicationDbContext())
                {
                    var roleStore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);

                    var currentRol = roleManager.FindById(aspNetRoles.Id);
                    currentRol.Name = aspNetRoles.Name;

                    await roleManager.UpdateAsync(currentRol);

                    return RedirectToAction("Index");
                }
            }
            return View(aspNetRoles);
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            if (aspNetRoles == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRoles);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            db.MenuAspNetRoles.RemoveRange(db.MenuAspNetRoles.Where(r => r.AspNetRolesId == id));
            db.AspNetRoles.Remove(aspNetRoles);                
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }







        // GET: Roles/Permisos/5
        public ActionResult Permisos(string id)
        {
            var rol = db.AspNetRoles.Find(id);
            var menu = db.Menu.Where(r => r.Activo).ToList();

            ViewBag.NombreRol = rol.Name;

            PermisosVM model = new PermisosVM
            {
                Menu = menu,
                idRol = id
            };


            return View(model);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Permisos(string idRol, string[] permisos)
        {            
            #region Elimina permisos previamente cargados
            try
            {
                using (TransactionScope tran = new TransactionScope())
                {
                    //db.Tag.Where(x => x.PostId == i && name == "Brand New!").ToList().ForEach(Tag.Tag.DeleteObject);

                    var MenuRol = db.MenuAspNetRoles.Where(r => r.AspNetRolesId == idRol);

                    foreach (var item in MenuRol)
                    {
                        db.MenuAspNetRolesAccion.RemoveRange(item.MenuAspNetRolesAccion);
                    }

                    db.MenuAspNetRoles.RemoveRange(MenuRol);

                    db.SaveChanges();

                    tran.Complete();

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            #endregion


            //Si no hay permisos seleccionados => se borran todos los permisos asociados al Rol.
            if (permisos == null)
            {
                return Json(new
                {
                    ok = 1,
                    mensaje = ""
                });
            }


            var idMenus = permisos.Where(r => r.Contains("menu")).Select(r => new { id = r.Split('_')[1] }).Distinct().ToList();
            var idMenusAccion = permisos.Where(r => r.Contains("accion")).Select(r => new
            {
                idMenu = r.Split('_')[1],
                idAccion = r.Split('_')[2]
            }).Distinct().ToList();



            try
            {

                foreach (var item in idMenusAccion)
                {
                    if (!idMenus.Any(r => r.id == item.idMenu))
                    {
                        idMenus.Add(new { id = item.idMenu });
                    }
                }


                using (TransactionScope tran = new TransactionScope())
                {
                    #region Alta de registros en "MenuAspNetRoles" y "MenuAspNetRolesAccion"
                    foreach (var idMenu in idMenus)
                    {
                        //Creo el nuevo registro "MenuAspNetRoles"
                        var itemMenuRol = new MenuAspNetRoles
                        {
                            MenuId = int.Parse(idMenu.id),
                            AspNetRolesId = idRol
                        };

                        //Guardo el nuevo registro "MenuAspNetRoles"
                        db.MenuAspNetRoles.Add(itemMenuRol);
                        db.SaveChanges();


                        //Si existe alguna accion asociada al menu registrado, se crean los registros
                        //correspondientes en "MenuAspNetRolesAccion"
                        if (idMenusAccion.Any(r => r.idMenu == idMenu.id))
                        {
                            foreach (var accion in idMenusAccion.Where(r => r.idMenu == idMenu.id))
                            {
                                //Creo el nuevo registro "MenuAspNetRolesAccion"
                                var itemMenuRolAccion = new MenuAspNetRolesAccion
                                {
                                    MenuAspNetRolesId = itemMenuRol.ID,
                                    AccionId = int.Parse(accion.idAccion)
                                };

                                //Guardo el nuevo registro "MenuAspNetRolesAccion"
                                db.MenuAspNetRolesAccion.Add(itemMenuRolAccion);
                            }
                            db.SaveChanges();
                        }

                    }

                    tran.Complete();

                    #endregion

                }
                return Json(new
                {
                    ok = 1,
                    mensaje = ""
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ok = 0,
                    mensaje = ex.InnerException.Message
                });
            }

        }



        // GET: Roles/GetPermisosRol/5
        public ActionResult GetPermisosRol(string id)
        {
            string[] x = new string[] { "accion_11_1", "accion_11_2" };

            List<string> permisos = new List<string>();

            var MenuRol = db.MenuAspNetRoles.Where(r => r.AspNetRolesId == id).ToList();


            foreach (var menu in MenuRol)
            {
                if (menu.MenuAspNetRolesAccion != null && menu.MenuAspNetRolesAccion.Count > 0)
                {
                    foreach (var accion in menu.MenuAspNetRolesAccion)
                    {
                        permisos.Add("accion_" + menu.MenuId + "_" + accion.AccionId);
                    }
                }
                else
                {
                    permisos.Add("menu_" + menu.MenuId);
                }
            }




            return Json(permisos.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }

}