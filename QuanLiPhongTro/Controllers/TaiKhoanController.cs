using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QuanLiPhongTro.Data;
using System.Data;
using System;

namespace QuanLiPhongTro.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly DBHelper _dbHelper;

        public TaiKhoanController(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // 1. Dùng để hiện trang đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.StudentID = "2415053122232";
            return View("~/Views/Auth/Login.cshtml");
        }

        // 2. Dùng để xử lý khi bấm nút Đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            ViewBag.StudentID = "2415053122232";

            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ViewBag.Error = "Vui lòng nhập đầy đủ thông tin!";
                    return View("~/Views/Auth/Login.cshtml");
                }

                string query = $"SELECT * FROM TaiKhoan WHERE Email = '{email}' AND MatKhau = '{password}' AND TrangThai = N'Hoạt động'";
                DataTable dt = _dbHelper.ExecuteQuery(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string vaiTro = dt.Rows[0]["VaiTro"]?.ToString() ?? "User";
                    string maTK = dt.Rows[0]["MaTaiKhoan"]?.ToString() ?? "";

                    HttpContext.Session.SetString("MaTaiKhoan", maTK);
                    HttpContext.Session.SetString("VaiTro", vaiTro);
                    HttpContext.Session.SetString("Email", email);

                    if (vaiTro == "Admin")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "UserDashboard");
                    }
                }

                ViewBag.Error = "Email hoặc mật khẩu không đúng!";
                return View("~/Views/Auth/Login.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi kết nối database: " + ex.Message;
                return View("~/Views/Auth/Login.cshtml");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}