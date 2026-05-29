using System;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using QuanLiPhongTro.Data;

namespace QuanLiPhongTro.Controllers
{
    public class HopDongController : Controller
    {
        private readonly DBHelper _dbHelper;

        public HopDongController(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IActionResult Index()
        {
            try
            {
                ViewBag.StudentID = "2415053122232";

                string query = @"
                    SELECT h.MaHopDong, k.HoTen, p.TenPhong, h.NgayBatDau, h.NgayKetThuc, h.TrangThai
                    FROM HopDong h
                    JOIN KhachThue k ON h.MaKhachThue = k.MaKhachThue
                    JOIN PhongTro p ON h.MaPhong = p.MaPhong
                    ORDER BY h.NgayBatDau DESC";

                DataTable dt = _dbHelper.ExecuteQuery(query);
                return View(dt);
            }
            catch (Exception ex)
            {
                return Content("LỖI SQL: " + ex.Message);
            }
        }

        // ✅ ACTION DETAILS - đã thêm mới
        public IActionResult Details(string id)
        {
            try
            {
                ViewBag.StudentID = "2415053122232";

                string query = $@"
                    SELECT h.MaHopDong, k.HoTen, k.SoDienThoai, k.CCCD,
                           p.TenPhong, p.GiaThue,
                           h.NgayBatDau, h.NgayKetThuc, h.TienCoc, h.TrangThai
                    FROM HopDong h
                    JOIN KhachThue k ON h.MaKhachThue = k.MaKhachThue
                    JOIN PhongTro p ON h.MaPhong = p.MaPhong
                    WHERE h.MaHopDong = '{id}'";

                DataTable dt = _dbHelper.ExecuteQuery(query);

                if (dt.Rows.Count == 0)
                {
                    TempData["Error"] = $"Không tìm thấy hợp đồng với mã: {id}";
                    return RedirectToAction("Index");
                }

                return View(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                return Content("LỖI SQL: " + ex.Message);
            }
        }

        public IActionResult Create()
        {
            ViewBag.StudentID = "2415053122232";

            ViewBag.KhachThueList = _dbHelper.ExecuteQuery("SELECT MaKhachThue, HoTen, CCCD FROM KhachThue");
            ViewBag.PhongList = _dbHelper.ExecuteQuery("SELECT MaPhong, TenPhong FROM PhongTro");

            return View();
        }

        [HttpPost]
        public IActionResult Create(string MaHopDong, string MaKhachThue, string MaPhong, string NgayBatDau, string NgayKetThuc, decimal TienCoc, string TrangThai)
        {
            ViewBag.StudentID = "2415053122232";
            try
            {
                string query = $@"
                    INSERT INTO HopDong (MaHopDong, MaKhachThue, MaPhong, NgayBatDau, NgayKetThuc, TienCoc, TrangThai) 
                    VALUES ('{MaHopDong}', '{MaKhachThue}', '{MaPhong}', '{NgayBatDau}', '{NgayKetThuc}', {TienCoc}, N'{TrangThai}')";

                _dbHelper.ExecuteNonQuery(query);
                TempData["Success"] = "Đã lập hợp đồng mới thành công!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content("LỖI THÊM HỢP ĐỒNG: " + ex.Message);
            }
        }
    }
}
