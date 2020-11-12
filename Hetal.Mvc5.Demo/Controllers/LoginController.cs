using Hetal.Mvc5.Demo.Models;
using Hetal.Mvc5.Demo.Models.DataModels;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Hetal.Mvc5.Demo.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController()
        {
            _context = new AppDbContext();
        }

        public ActionResult Index()
        {
            LoginViewModel vm = new LoginViewModel();
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Email.ToLower() == model.Email.ToLower());
            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid username or password!";
                return View(model);
            }

            if (!user.Password.Equals(model.Password))
            {
                TempData["ErrorMessage"] = "Invalid username or password!";
                return View(model);
            }

            Session["Email"] = user.Email;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = await _context.Users.FirstOrDefaultAsync(m => m.Email.ToLower() == model.Email ||
                m.UserName.ToLower() == model.UserName);

            if(existingUser != null)
            {
                TempData["ErrorMessage"] = "User already exists!";
                return View(model);
            }

            UserMaster registeringUser = new UserMaster
            {
                Email = model.Email,
                UserName = model.UserName,
                FullName = model.FullName,
                City = model.City,
                Phone = model.Phone,
                Password = model.Password
            };

            _context.Users.Add(registeringUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Login");
        }

        public async Task<ActionResult> UpdateProfile()
        {
            var loggedInUserEmail = Convert.ToString(Session["Email"]);
            if(string.IsNullOrEmpty(loggedInUserEmail))
            {
                return Logout();
            }

            var loggedInUser = await _context.Users.FirstOrDefaultAsync(m => m.Email.ToLower() == loggedInUserEmail.ToLower());
            if(loggedInUser == null)
            {
                return Logout();
            }

            UpdateProfileViewModel model = new UpdateProfileViewModel
            {
                Id = loggedInUser.Id,
                UserName = loggedInUser.UserName,
                FullName = loggedInUser.FullName,
                City = loggedInUser.City,
                Phone = loggedInUser.Phone
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loggedInUserEmail = Convert.ToString(Session["Email"]);
            if (string.IsNullOrEmpty(loggedInUserEmail))
            {
                return Logout();
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(
                m => m.UserName.ToLower() == model.UserName.ToLower() && model.Id != m.Id);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "User already exists!";
                return View(model);
            }

            var updatingUser = await _context.Users.FirstOrDefaultAsync(m => m.Email.ToLower() == loggedInUserEmail.ToLower());
            updatingUser.UserName = model.UserName;
            updatingUser.FullName = model.FullName;
            updatingUser.City = model.City;
            updatingUser.Phone = model.Phone;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "User updated!";
            return RedirectToAction("UpdateProfile", "Login");

        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}