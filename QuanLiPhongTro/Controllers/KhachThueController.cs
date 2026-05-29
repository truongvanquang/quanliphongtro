using Microsoft.AspNetCore.Mvc;
using QuanLiPhongTro.Data;
using System.Data;
using System;

namespace QuanLiPhongTro.Controllers
{
    public class KhachThueController : Controller
    {
        private readonly DBHelper _dbHelper;

        public KhachThueController(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IActionResult Index(string searchString, string statusFilter)
        {
            ViewBag.StudentID = "2415053122232";
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentFilter = statusFilter;

            string query = "SELECT * FROM KhachThue WHERE 1=1";

            if (!string.IsNullOrEmpty(searchString))
            {
                query += $" AND (HoTen LIKE N'%{searchString}%' OR SoDienThoai LIKE '%{searchString}%' OR CCCD LIKE '%{searchString}%')";
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                if (statusFilter == "active")
                {
                    query += " AND TrangThai LIKE N'%ĐANG%'";
                }
                else if (statusFilter == "expired")
                {
                    query += " AND (TrangThai LIKE N'%HẾT%' OR TrangThai LIKE N'%TRỐNG%' OR TrangThai LIKE N'%CHUYỂN%')";
                }
            }

            query += " ORDER BY NgayThamGia DESC";

            DataTable dt = _dbHelper.ExecuteQuery(query);
            return View(dt);
        }

        public IActionResult Create()
        {
            ViewBag.StudentID = "2415053122232";
            return View();
        }

        [HttpPost]
        public IActionResult Create(string MaKhachThue, string HoTen, string SoDienThoai, string CCCD, string NgayThamGia, string TrangThai)
        {
            string ngay = string.IsNullOrEmpty(NgayThamGia) ? DateTime.Now.ToString("yyyy-MM-dd") : NgayThamGia;
            string query = $"INSERT INTO KhachThue (MaKhachThue, HoTen, SoDienThoai, CCCD, NgayThamGia, TrangThai) VALUES ('{MaKhachThue}', N'{HoTen}', '{SoDienThoai}', '{CCCD}', '{ngay}', N'{TrangThai}')";
            try
            {
                _dbHelper.ExecuteNonQuery(query);
                TempData["Success"] = "Đã thêm khách thuê thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi SQL: " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(string id)
        {
            ViewBag.StudentID = "2415053122232";
            DataTable dt = _dbHelper.ExecuteQuery($"SELECT * FROM KhachThue WHERE MaKhachThue = '{id}'");
            if (dt.Rows.Count == 0) return RedirectToAction("Index");
            return View(dt.Rows[0]);
        }

        [HttpPost]
        public IActionResult Edit(string MaKhachThue, string HoTen, string SoDienThoai, string CCCD, string TrangThai)
        {
            string query = $"UPDATE KhachThue SET HoTen = N'{HoTen}', SoDienThoai = '{SoDienThoai}', CCCD = '{CCCD}', TrangThai = N'{TrangThai}' WHERE MaKhachThue = '{MaKhachThue}'";
            try
            {
                _dbHelper.ExecuteNonQuery(query);
                TempData["Success"] = "Đã cập nhật thông tin khách thuê!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi SQL: " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(string id)
        {
            ViewBag.StudentID = "2415053122232";
            DataTable dt = _dbHelper.ExecuteQuery($"SELECT * FROM KhachThue WHERE MaKhachThue = '{id}'");
            if (dt.Rows.Count == 0) return RedirectToAction("Index");
            return View(dt.Rows[0]);
        }
    }
}