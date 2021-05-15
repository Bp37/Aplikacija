using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SigurnostAplikacija.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SigurnostAplikacija.Controllers
{
    [Authorize(Roles = RoleHelper.Administrator)]
    public class RoleController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Role
        public ActionResult Index()
        {
            var allRoles = _db.Roles.ToList();

            return View(allRoles);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var roleName = collection["RoleName"];
                IdentityRole role = new IdentityRole(roleName);
                _db.Roles.Add(role);
                _db.SaveChanges();

                TempData["Message"] = "Uspješno kreirana rola";

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }

        public ActionResult Edit(string roleName)
        {
            try
            {
                var role = _db.Roles.
                    Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase))
                    .FirstOrDefault();

                return View(role);
            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Edit(IdentityRole role)
        {
            try
            {
                _db.Entry(role).State = EntityState.Modified;
                _db.SaveChanges();

                TempData["Message"] = "Uspješno ste promijenili naziv role";

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }

        public ActionResult Delete(string roleName)
        {
            try
            {
                var role = _db.Roles.
                     Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase))
                     .FirstOrDefault();
                _db.Roles.Remove(role);
                _db.SaveChanges();

                TempData["Message"] = "Uspješno ste obrisali rolu";

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }

        public ActionResult ManageUserRoles()
        {
            ViewBag.Roles = GetAllRoles();

            return View();
        }

        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            ApplicationUser user = _db.Users.Where(u => u.UserName == UserName).FirstOrDefault();
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            um.AddToRole(user.Id, RoleName);

            TempData["Message"] = "Uspješno ste kreirali rolu  za korisnika";


            return RedirectToAction("ManageUserRoles");
        }

        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = _db.Users.Where(u => u.UserName == UserName).FirstOrDefault();
                var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                ViewBag.RolesForThisUser = um.GetRoles(user.Id);

                ViewBag.Roles = GetAllRoles();
            }

            return View("ManageUserRoles");
        }

        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            ApplicationUser user = _db.Users.Where(u => u.UserName == UserName).FirstOrDefault();
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            if (um.IsInRole(user.Id, RoleName))
            {
                um.RemoveFromRole(user.Id, RoleName);
                TempData["Message"] = "Uspješno ste obrisali korisnika iz role";
            }
            else
            {
                TempData["ErrorMessage"] = "Korisnik nema tu rolu";
            }

            ViewBag.Roles = GetAllRoles();


            return View("ManageUserRoles");
        }

        private List<SelectListItem> GetAllRoles()
        {
            var allRoles = _db.Roles.OrderBy(m => m.Name).ToList()
                .Select
                (
                    sl => new SelectListItem()
                    {
                        Value = sl.Name,
                        Text = sl.Name
                    }
                ).ToList();

            return allRoles;
        }
    }
}
