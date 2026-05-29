using Microsoft.AspNetCore.Mvc;
using QuanLiPhongTro.Data;
using System.Data;
using System;

namespace QuanLiPhongTro.Controllers
{
    public class PhongTroController : Controller
    {
        private readonly DBHelper _dbHelper;

        public PhongTroController(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IActionResult Index()
        {
            ViewBag.StudentID = "2415053122232";
            string query = "SELECT p.MaPhong, p.TenPhong, l.TenLoai, p.DienTich, p.GiaThue, p.TrangThai FROM PhongTro p LEFT JOIN LoaiPhong l ON p.MaLoai = l.MaLoai";
            DataTable dt = _dbHelper.ExecuteQuery(query);
            return View(dt);
        }

        public IActionResult Create()
        {
            ViewBag.StudentID = "2415053122232";
            return View();
        }

        [HttpPost]
        public IActionResult Create(string MaPhong, string TenPhong, string MaLoai, string DienTich, string GiaThue, string TrangThai)
        {
            string dienTichVal = string.IsNullOrEmpty(DienTich) ? "0" : DienTich;
            string giaThueVal = string.IsNullOrEmpty(GiaThue) ? "0" : GiaThue;
            string maLoaiVal = string.IsNullOrEmpty(MaLoai) ? "NULL" : $"'{MaLoai}'";

            string query = $"INSERT INTO PhongTro (MaPhong, TenPhong, MaLoai, DienTich, GiaThue, TrangThai) VALUES ('{MaPhong}', N'{TenPhong}', {maLoaiVal}, {dienTichVal}, {giaThueVal}, N'{TrangThai}')";

            try
            {
                _dbHelper.ExecuteNonQuery(query);
                TempData["Success"] = "Đã lưu phòng mới thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi SQL: " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        // --- CẬP NHẬT: HÀM SỬA PHÒNG ---
        public IActionResult Edit(string id)
        {
            ViewBag.StudentID = "2415053122232";
            DataTable dt = _dbHelper.ExecuteQuery($"SELECT * FROM PhongTro WHERE MaPhong = '{id}'");
            if (dt.Rows.Count == 0) return RedirectToAction("Index");
            return View(dt.Rows[0]);
        }

        [HttpPost]
        public IActionResult Edit(string MaPhong, string TenPhong, string MaLoai, string DienTich, string GiaThue, string TrangThai)
        {
            string maLoaiVal = string.IsNullOrEmpty(MaLoai) ? "NULL" : $"'{MaLoai}'";
            string dienTichVal = string.IsNullOrEmpty(DienTich) ? "0" : DienTich;
            string giaThueVal = string.IsNullOrEmpty(GiaThue) ? "0" : GiaThue;

            string query = $@"UPDATE PhongTro 
                              SET TenPhong = N'{TenPhong}', 
                                  MaLoai = {maLoaiVal}, 
                                  DienTich = {dienTichVal}, 
                                  GiaThue = {giaThueVal}, 
                                  TrangThai = N'{TrangThai}' 
                              WHERE MaPhong = '{MaPhong}'";

            try
            {
                _dbHelper.ExecuteNonQuery(query);
                TempData["Success"] = "Đã cập nhật thông tin thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Cập nhật thất bại! Lỗi: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
        // ------------------------------

        public IActionResult Delete(string id)
        {
            string query = $"DELETE FROM PhongTro WHERE MaPhong = '{id}'";
            try
            {
                _dbHelper.ExecuteNonQuery(query);
                TempData["Success"] = "Đã xóa phòng thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}