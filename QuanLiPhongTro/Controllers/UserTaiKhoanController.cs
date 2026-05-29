using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QuanLiPhongTro.Data;
using System.Data;
using System;

namespace QuanLiPhongTro.Controllers
{
    public class UserTaiKhoanController : Controller
    {
        private readonly DBHelper _dbHelper;

        public UserTaiKhoanController(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IActionResult Index()
        {
            ViewBag.StudentID = "2415053122232";
            string email = HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(email)) return RedirectToAction("DangNhap", "UserTaiKhoan");

            try
            {
                string query = $@"
                    SELECT k.HoTen, k.SoDienThoai, k.CCCD, k.NgayThamGia, k.TrangThai,
                           t.Email, t.MaTaiKhoan
                    FROM KhachThue k
                    JOIN TaiKhoan t ON k.MaTaiKhoan = t.MaTaiKhoan
                    WHERE t.Email = '{email}'";

                DataTable dt = _dbHelper.ExecuteQuery(query);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ViewBag.ThongTin = dt.Rows[0];
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi tải thông tin: " + ex.Message;
            }

            return View();
        }

        [HttpPost]
        public IActionResult CapNhatThongTin(string HoTen, string SoDienThoai, string CCCD)
        {
            ViewBag.StudentID = "2415053122232";
            string email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("DangNhap", "UserTaiKhoan");

            try
            {
                string query = $@"
                    UPDATE k 
                    SET k.HoTen = N'{HoTen}', k.SoDienThoai = '{SoDienThoai}', k.CCCD = '{CCCD}'
                    FROM KhachThue k
                    JOIN TaiKhoan t ON k.MaTaiKhoan = t.MaTaiKhoan
                    WHERE t.Email = '{email}'";

                _dbHelper.ExecuteNonQuery(query);
                TempData["Success"] = "Đã cập nhật thông tin thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi cập nhật: " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DoiMatKhau(string matkhauCu, string matkhauMoi, string xacNhan)
        {
            ViewBag.StudentID = "2415053122232";
            string email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("DangNhap", "UserTaiKhoan");

            if (matkhauMoi != xacNhan)
            {
                TempData["Error"] = "Mật khẩu xác nhận không khớp!";
                return RedirectToAction("Index");
            }

            try
            {
                string checkQuery = $"SELECT COUNT(*) FROM TaiKhoan WHERE Email = '{email}' AND MatKhau = '{matkhauCu}'";
                DataTable dt = _dbHelper.ExecuteQuery(checkQuery);

                int count = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    count = Convert.ToInt32(dt.Rows[0][0]);
                }

                if (count == 0)
                {
                    TempData["Error"] = "Mật khẩu cũ không chính xác!";
                    return RedirectToAction("Index");
                }

                string updateQuery = $"UPDATE TaiKhoan SET MatKhau = '{matkhauMoi}' WHERE Email = '{email}'";
                _dbHelper.ExecuteNonQuery(updateQuery);
                TempData["Success"] = "Đổi mật khẩu thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi đổi mật khẩu: " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        public IActionResult DangKy()
        {
            ViewBag.StudentID = "2415053122232";
            return View();
        }

        [HttpPost]
        public IActionResult DangKy(string hoTen, string email, string soDienThoai, string matKhau, string xacNhanMatKhau)
        {
            ViewBag.StudentID = "2415053122232";

            if (matKhau != xacNhanMatKhau)
            {
                TempData["Error"] = "Mật khẩu xác nhận không khớp!";
                return View();
            }

            try
            {
                string checkQuery = $"SELECT COUNT(*) FROM TaiKhoan WHERE Email = '{email}'";
                DataTable dt = _dbHelper.ExecuteQuery(checkQuery);
                int count = Convert.ToInt32(dt.Rows[0][0]);

                if (count > 0)
                {
                    TempData["Error"] = "Email này đã được đăng ký!";
                    return View();
                }

                string maTaiKhoan = "TK" + DateTime.Now.ToString("yyMMddHHmmss");
                string maKhachThue = "KT" + DateTime.Now.ToString("yyMMddHHmmss");

                string insertTK = $"INSERT INTO TaiKhoan (MaTaiKhoan, Email, MatKhau, VaiTro) VALUES ('{maTaiKhoan}', '{email}', '{matKhau}', 'User')";
                _dbHelper.ExecuteNonQuery(insertTK);

                string insertKT = $"INSERT INTO KhachThue (MaKhachThue, HoTen, SoDienThoai, MaTaiKhoan) VALUES ('{maKhachThue}', N'{hoTen}', '{soDienThoai}', '{maTaiKhoan}')";
                _dbHelper.ExecuteNonQuery(insertKT);

                TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("DangNhap");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi hệ thống: " + ex.Message;
                return View();
            }
        }

        public IActionResult DangNhap()
        {
            ViewBag.StudentID = "2415053122232";
            return View();
        }

        [HttpPost]
        public IActionResult DangNhap(string email, string matKhau)
        {
            ViewBag.StudentID = "2415053122232";
            try
            {
                string query = $"SELECT VaiTro FROM TaiKhoan WHERE Email = '{email}' AND MatKhau = '{matKhau}'";
                DataTable dt = _dbHelper.ExecuteQuery(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    HttpContext.Session.SetString("Email", email);

                    string vaiTro = dt.Rows[0]["VaiTro"].ToString();
                    if (vaiTro == "Admin")
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    return RedirectToAction("Index", "UserPhong");
                }

                TempData["Error"] = "Email hoặc mật khẩu không chính xác!";
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi hệ thống: " + ex.Message;
                return View();
            }
        }

        public IActionResult DangXuat()
        {
            HttpContext.Session.Remove("Email");
            return RedirectToAction("DangNhap");
        }

        public IActionResult ManHinhChinh()
        {
            ViewBag.StudentID = "2415053122232";
            return View();
        }
    }
}