using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QuanLiPhongTro.Data;
using System.Data;
using System;

namespace QuanLiPhongTro.Controllers
{
    public class UserHopDongController : Controller
    {
        private readonly DBHelper _dbHelper;
        public UserHopDongController(DBHelper dbHelper) { _dbHelper = dbHelper; }
        public IActionResult Detail(string id)
        {
            ViewBag.StudentID = "2415053122232";
            string query = $@"
        SELECT h.*, p.TenPhong, k.HoTen 
        FROM HopDong h 
        JOIN PhongTro p ON h.MaPhong = p.MaPhong 
        JOIN KhachThue k ON h.MaKhachThue = k.MaKhachThue
        WHERE h.MaHopDong = '{id}'";
            DataTable dt = _dbHelper.ExecuteQuery(query);
            if (dt.Rows.Count == 0) return RedirectToAction("Index");
            return View(dt.Rows[0]);
        }

        public IActionResult Index()
        {
            string email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "TaiKhoan");

            ViewBag.StudentID = "2415053122232";

            try
            {
                // Câu SQL đã được khôi phục lại JOIN TaiKhoan chuẩn xác
                string query = $@"
                    SELECT h.MaHopDong, p.TenPhong, h.NgayBatDau, h.NgayKetThuc, h.TienCoc, h.TrangThai
                    FROM HopDong h
                    JOIN KhachThue k ON h.MaKhachThue = k.MaKhachThue
                    JOIN TaiKhoan t ON k.MaTaiKhoan = t.MaTaiKhoan
                    JOIN PhongTro p ON h.MaPhong = p.MaPhong
                    WHERE t.Email = '{email}'
                    ORDER BY h.NgayBatDau DESC";

                DataTable dt = _dbHelper.ExecuteQuery(query);
                return View(dt);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi SQL: " + ex.Message;
                return View(new DataTable());
            }
        }
    }
}