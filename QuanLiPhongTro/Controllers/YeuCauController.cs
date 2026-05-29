using Microsoft.AspNetCore.Mvc;
using QuanLiPhongTro.Data;
using System.Data;
using System;

namespace QuanLiPhongTro.Controllers
{
    public class YeuCauController : Controller
    {
        private readonly DBHelper _dbHelper;

        public YeuCauController(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IActionResult Index(string searchString)
        {
            ViewBag.StudentID = "2415053122232";
            ViewBag.CurrentSearch = searchString;

            string query = @"
                SELECT yc.*, k.HoTen, p.TenPhong 
                FROM YeuCauSuaChua yc
                LEFT JOIN KhachThue k ON yc.MaKhachThue = k.MaKhachThue
                LEFT JOIN PhongTro p ON yc.MaPhong = p.MaPhong
                WHERE 1=1";

            if (!string.IsNullOrEmpty(searchString))
            {
                query += $" AND (k.HoTen LIKE N'%{searchString}%' OR p.TenPhong LIKE '%{searchString}%' OR yc.MoTa LIKE N'%{searchString}%' OR yc.TieuDe LIKE N'%{searchString}%')";
            }

            query += " ORDER BY yc.NgayGui DESC";

            DataTable dt = _dbHelper.ExecuteQuery(query);
            return View(dt);
        }

        public IActionResult Create()
        {
            ViewBag.StudentID = "2415053122232";
            return View();
        }

        [HttpPost]
        public IActionResult Create(string MaYeuCau, string MaKhachThue, string MaPhong, string TieuDe, string MoTa, string UuTien, string TrangThai)
        {
            string ngayGui = DateTime.Now.ToString("yyyy-MM-dd");
            string query = $"INSERT INTO YeuCauSuaChua (MaYeuCau, MaKhachThue, MaPhong, TieuDe, MoTa, UuTien, TrangThai, NgayGui) VALUES ('{MaYeuCau}', '{MaKhachThue}', '{MaPhong}', N'{TieuDe}', N'{MoTa}', N'{UuTien}', N'{TrangThai}', '{ngayGui}')";

            try
            {
                _dbHelper.ExecuteNonQuery(query);
                TempData["Success"] = "Đã tạo yêu cầu mới!";
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
            DataTable dt = _dbHelper.ExecuteQuery($"SELECT * FROM YeuCauSuaChua WHERE MaYeuCau = '{id}'");
            if (dt.Rows.Count == 0) return RedirectToAction("Index");
            return View(dt.Rows[0]);
        }

        [HttpPost]
        public IActionResult Edit(string MaYeuCau, string TieuDe, string MoTa, string UuTien, string TrangThai)
        {
            string query = $"UPDATE YeuCauSuaChua SET TieuDe = N'{TieuDe}', MoTa = N'{MoTa}', UuTien = N'{UuTien}', TrangThai = N'{TrangThai}' WHERE MaYeuCau = '{MaYeuCau}'";

            try
            {
                _dbHelper.ExecuteNonQuery(query);
                TempData["Success"] = "Đã cập nhật yêu cầu!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi SQL: " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(string id)
        {
            try
            {
                _dbHelper.ExecuteNonQuery($"DELETE FROM YeuCauSuaChua WHERE MaYeuCau = '{id}'");
                TempData["Success"] = "Đã xóa yêu cầu!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}