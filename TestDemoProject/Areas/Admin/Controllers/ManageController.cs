using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using InterviewSystem.Models;
using System.Data.Entity;

namespace InterviewSystem.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ManageController()
        {

        }
        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationDbContext context = new ApplicationDbContext();
        // GET: Admin/Manage
        public ActionResult Index()
        {
            var roles = context.Roles.ToList();
            return View(roles);
        }
        // GET: /Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                context.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully !";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // Admin/Delete
        public ActionResult Delete(string RoleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        //
        // GET: /Admin/Edit/id
        public ActionResult Edit(string roleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisRole);
        }

        //
        // POST: /Admin/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            try
            {
                context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ManageUserRoles()
        {
            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>

new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            //var account = new AccountController();
            UserManager.AddToRole(user.Id, RoleName);
            var entryuser = context.Users.FirstOrDefault(x => x.UserName == user.UserName);
            if(RoleName=="Candidate")
            {
                 var candidate = new CandidateDetails
               {
                   //FirstName = model.FirstName,
                   //LastName = model.LastName,
                   User = entryuser,

               };
                 context.CandidateDetails.Add(candidate);
                 context.SaveChanges();
            }
            else if(RoleName=="Interviewer")
            {
                 var interviewer = new InterviewerDetails
            {
                //FirstName = model.FirstName,
                //LastName = model.LastName,
                User = entryuser,

            };
                 context.InterviewerDetails.Add(interviewer);
                 context.SaveChanges();
            }         
            ViewBag.ResultMessage = "Role created successfully !";

            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageUserRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                //var account = new AccountController();
                ViewBag.RolesForThisUser = UserManager.GetRoles(user.Id);
               // ViewBag.RolesForThisUser = account.UserManager.GetRoles(user.Id);

                // prepopulat roles for the view dropdown
                var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;
            }

            return View("ManageUserRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            //var account = new AccountController();
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (UserManager.IsInRole(user.Id, RoleName))
            {
                UserManager.RemoveFromRole(user.Id, RoleName);
                ViewBag.ResultMessage = "Role removed from this user successfully !";
            }
            else
            {
                ViewBag.ResultMessage = "This user doesn't belong to selected role.";
            }
            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageUserRoles");
        }

        [HttpGet]
        public ActionResult ManageRounds()
        {
            ViewBag.InterviewRounds = context.Departments.Select(x => new SelectListItem
            {
                Value = x.DepartmentName,
                Text = x.DepartmentName,
            });
            return View(new InterviewRounds());
        }

        [HttpPost]
        public ActionResult SaveRounds(InterviewRounds rounds,FormCollection form)
        {
            InterviewRounds round = new InterviewRounds();
            round.Description = rounds.Description;
            string department = form["Department"];
            round.Department = context.Departments.FirstOrDefault(x => x.DepartmentName == department);
            //round.Department = context.Departments.FirstOrDefault(x => x.Id == rounds.Department.Id);
            round.Stage = rounds.Stage;
            round.Id = rounds.Id;
            context.InterviewRounds.Add(round);
            context.SaveChanges();
            return RedirectToAction("ManageRounds");
        }

        [HttpGet]
        public ActionResult ManageDepts()
        {
            return View(new Departments());
        }

        [HttpPost]
        public ActionResult SaveDepts(Departments depts)
        {
            Departments dept = new Departments();
            dept.Id = depts.Id;
            dept.DepartmentName = depts.DepartmentName;
            context.Departments.Add(dept);
            context.SaveChanges();
            return RedirectToAction("ManageDepts");
        }

    }
}