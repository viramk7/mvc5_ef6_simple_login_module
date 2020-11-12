using Hetal.Mvc5.Demo.Models.DataModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Hetal.Mvc5.Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController()
        {
            _context = new AppDbContext();
        }

        public async Task<ActionResult> Index()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            //var users = await _context.Users.ToListAsync();
            return View();
        }
    }
}