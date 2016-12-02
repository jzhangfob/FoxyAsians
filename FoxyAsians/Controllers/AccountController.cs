using FoxyAsians.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;

namespace FoxyAsians.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }
        private AppDbContext db = new AppDbContext();

        private AppSignInManager _signInManager;
        private AppUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(AppUserManager userManager, AppSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public AppSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<AppSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)//NOTE: User has been re-directed here from a page they're not authorized to see
            {
                return View("Error", new string[] { "Access Denied" });
            }
            AuthenticationManager.SignOut();  //this removes any old cookies hanging around
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }  

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //TODO: Add fields to user here so they will be saved to the database
                //Create a new user with all the properties you need for the class
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    StreetAddress = model.StreetAddress,
                    City = model.City,
                    State = model.State,
                    Zip = model.Zip,
                    PhoneNumber = model.PhoneNumber

                };

                //Add the new user to the database
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded) //user was created successfully
                {
                    //TODO: Once you get roles working, you may want to add users to roles upon creation
                    await UserManager.AddToRoleAsync(user.Id, "Customer"); //adds user to role called "Customer"                
                    //sign the user in
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    //send them to the home page
                    return RedirectToAction("Index", "Home");
                }

                //if there was a problem, add the error messages to what we will display
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }


        // GET: /Account/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }


        // POST: /Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        // GET: /Account/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            ViewBag.FirstName = currentUser.FirstName;
            ViewBag.LastName = currentUser.LastName;
            ViewBag.PhoneNumber = currentUser.PhoneNumber;
            ViewBag.Email = currentUser.Email;
            ViewBag.StreetAddress = currentUser.StreetAddress;
            ViewBag.City = currentUser.City;
            ViewBag.State = currentUser.State;
            ViewBag.Zip = currentUser.Zip;

            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        // GET: /Account/Edit 
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var userId = id;
            if (string.IsNullOrEmpty(id))
            {
                userId = User.Identity.GetUserId();
            }
            AppUser user = db.Users.SingleOrDefault(u => u.Id == userId);

            AppUser model = new AppUser()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                StreetAddress = user.StreetAddress,
                City = user.City,
                State = user.State,
                Zip = user.Zip,
                CreditCard1 = user.CreditCard1,
                CreditCard2 = user.CreditCard2,
                isActive = user.isActive
            };

            //get the user role and pass it in the ViewBag
            var admins = db.Roles.Where(x => x.Name == "Admin" || x.Name == "Employee").ToList();
            foreach (var admin in admins)
            {
                if (admin.Name == "Admin")
                {
                    ViewData["isAdmin"] = true;
                    ViewData["isEmployee"] = true;
                    break;
                }
                else if (admin.Name == "Employee")
                {
                    ViewData["isEmployee"] = true;
                    break;
                }
            }

            return View(model);

        }

        // POST: /Account/Edit 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditInfo([Bind(Include = "Id, FirstName, LastName, Email, StreetAddress, City, State, Zip, CreditCard1, CreditCard2, isActive")] AppUser appUser, string id)
        {
            if (ModelState.IsValid)
            {
                AppUser user = UserManager.FindById(id);

                user.FirstName = appUser.FirstName;
                user.LastName = appUser.LastName;
                user.PhoneNumber = appUser.PhoneNumber;
                user.Email = appUser.Email;
                user.StreetAddress = appUser.StreetAddress;
                user.City = appUser.City;
                user.State = appUser.State;
                user.Zip = appUser.Zip;
                user.CreditCard1 = appUser.CreditCard1;
                user.CreditCard2 = appUser.CreditCard2;
                user.isActive = appUser.isActive;
                var result = await UserManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    AddErrors(result);
                    return RedirectToAction("Edit", new { Id = appUser.Id });
                }
                return RedirectToAction("index", new { Id = appUser.Id });

            }

            return View(appUser);
        }


        //// Select list for all customers
        //public SelectList GetAllCustomers()
        //{ 
        //    var query = from c in db.Users
        //                select c;

        //    List<AppUser> allCustomers = query.ToList();

        //    SelectList list = new SelectList(allCustomers);

        //    return list;
        //}


        // GET: Account/SelectCustomer
        public ActionResult SelectCustomer()
        {
            var role = db.Roles.FirstOrDefault(x => x.Name == "Customer");

            var user = db.Users.Where(x => x.Roles.Any(y => y.RoleId.Equals(role.Id))).ToList();

            return View(user);
        }

        // GET: Account/SelectEmployee
        public ActionResult SelectEmployee()
        {
            var role = db.Roles.FirstOrDefault(x => x.Name == "Employee");

            var user = db.Users.Where(x => x.Roles.Any(y => y.RoleId.Equals(role.Id))).ToList();

            return View(user);
        }

        //// POST: Account/SelectCustomer
        //public ActionResult SelectCustomer(AppUser user)
        //{
        //    return View("EditCustomer");
        //}


        // GET: Account/EditCustomer
        public ActionResult EditCustomer()
        {
            return View();
        }


        // POST: Account/EditCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer([Bind(Include = "Id, FirstName,LastName,PhoneNumber,Email,StreetAddress,City,State,Zip,CreditCard1,CreditCard2")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                AppUser SelectedUser = db.Users.Find(appUser.Id);

                SelectedUser.FirstName = appUser.FirstName;
                SelectedUser.LastName = appUser.LastName;
                SelectedUser.PhoneNumber = appUser.PhoneNumber;
                SelectedUser.Email = appUser.Email;
                SelectedUser.StreetAddress = appUser.StreetAddress;
                SelectedUser.City = appUser.City;
                SelectedUser.State = appUser.State;
                SelectedUser.Zip = appUser.Zip;
                SelectedUser.CreditCard1 = appUser.CreditCard1;
                SelectedUser.CreditCard2 = appUser.CreditCard2;

                db.Entry(SelectedUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EditCustomer", "Account");

            }

            return View(appUser);

        }


        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

       
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
