using Microsoft.AspNetCore.Mvc;

namespace QuanLiPhongTro.Controllers
{
    public class BaoCaoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}