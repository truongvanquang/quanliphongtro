using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLiPhongTro.Data;
using QuanLiPhongTro.Models;
using System.Data;

namespace QuanLiPhongTro.Controllers
{
    public class AuthController : Controller
    {
        private readonly DBHelper _dbHelper;

        public AuthController(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string matkhau)
        {
            string query = $"SELECT MaTaiKhoan, VaiTro FROM TaiKhoan WHERE Email = '{email}' AND MatKhau = '{matkhau}' AND TrangThai = N'Hoạt động'";
            DataTable dt = _dbHelper.ExecuteQuery(query);

            if (dt.Rows.Count > 0)
            {
                string vaiTro = dt.Rows[0]["VaiTro"].ToString();
                string maTaiKhoan = dt.Rows[0]["MaTaiKhoan"].ToString();

                HttpContext.Session.SetString("MaTaiKhoan", maTaiKhoan);
                HttpContext.Session.SetString("VaiTro", vaiTro);

                if (vaiTro == "Admin")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "UserDashboard");
                }
            }

            ViewBag.Error = "Email hoặc mật khẩu không chính xác hoặc tài khoản đã bị khóa!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}