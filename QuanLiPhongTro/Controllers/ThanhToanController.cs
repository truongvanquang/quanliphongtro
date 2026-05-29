using System;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using QuanLiPhongTro.Data;

namespace QuanLiPhongTro.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly DBHelper _dbHelper;

        public ThanhToanController(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IActionResult Index(string monthFilter)
        {
            try
            {
                ViewBag.StudentID = "2415053122232";

                string query = @"
                    SELECT hd.MaHoaDon, k.HoTen, p.TenPhong, hd.TongTien, hd.HanThanhToan, hd.TrangThai, hd.NgayLap
                    FROM HoaDon hd
                    JOIN HopDong h ON hd.MaHopDong = h.MaHopDong
                    JOIN KhachThue k ON h.MaKhachThue = k.MaKhachThue
                    JOIN PhongTro p ON h.MaPhong = p.MaPhong
                    ORDER BY hd.HanThanhToan DESC";

                DataTable dt = _dbHelper.ExecuteQuery(query);

                decimal daThu = 0;
                decimal chuaThu = 0;
                decimal quaHan = 0;

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        decimal tien = Convert.ToDecimal(row["TongTien"]);
                        string trangThai = row["TrangThai"].ToString() ?? "";

                        DateTime hanThanhToan = DateTime.MaxValue;
                        if (row["HanThanhToan"] != DBNull.Value)
                        {
                            hanThanhToan = Convert.ToDateTime(row["HanThanhToan"]);
                        }

                        if (trangThai == "Đã thanh toán" || trangThai == "ĐÃ THANH TOÁN")
                        {
                            daThu += tien;
                        }
                        else if (trangThai.Contains("Quá hạn") || (hanThanhToan < DateTime.Now && !trangThai.Contains("Đã thanh toán")))
                        {
                            quaHan += tien;
                        }
                        else
                        {
                            chuaThu += tien;
                        }
                    }
                }

                ViewBag.DaThu = daThu;
                ViewBag.ChuaThu = chuaThu;
                ViewBag.QuaHan = quaHan;
                ViewBag.CurrentMonth = monthFilter;

                return View(dt);
            }
            catch (Exception ex)
            {
                return Content("LỖI SQL: " + ex.Message);
            }
        }

        public IActionResult Create()
        {
            ViewBag.StudentID = "2415053122232";

            string queryHopDong = @"
                SELECT h.MaHopDong, k.HoTen, p.TenPhong 
                FROM HopDong h 
                JOIN KhachThue k ON h.MaKhachThue = k.MaKhachThue 
                JOIN PhongTro p ON h.MaPhong = p.MaPhong";

            ViewBag.HopDongList = _dbHelper.ExecuteQuery(queryHopDong);

            return View();
        }

        [HttpPost]
        public IActionResult Create(string MaHoaDon, string MaHopDong, decimal TongTien, string HanThanhToan, string TrangThai)
        {
            ViewBag.StudentID = "2415053122232";
            string ngayLap = DateTime.Now.ToString("yyyy-MM-dd");
            string query = $"INSERT INTO HoaDon (MaHoaDon, MaHopDong, TongTien, HanThanhToan, TrangThai, NgayLap) VALUES ('{MaHoaDon}', '{MaHopDong}', {TongTien}, '{HanThanhToan}', N'{TrangThai}', '{ngayLap}')";

            try
            {
                _dbHelper.ExecuteNonQuery(query);
                TempData["Success"] = "Đã thêm hóa đơn mới thành công!";
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

            string queryHopDong = @"
                SELECT h.MaHopDong, k.HoTen, p.TenPhong 
                FROM HopDong h 
                JOIN KhachThue k ON h.MaKhachThue = k.MaKhachThue 
                JOIN PhongTro p ON h.MaPhong = p.MaPhong";
            ViewBag.HopDongList = _dbHelper.ExecuteQuery(queryHopDong);

            string query = $"SELECT * FROM HoaDon WHERE MaHoaDon = '{id}'";
            DataTable dt = _dbHelper.ExecuteQuery(query);

            if (dt.Rows.Count == 0) return RedirectToAction("Index");

            return View(dt.Rows[0]);
        }

        [HttpPost]
        public IActionResult Edit(string MaHoaDon, string MaHopDong, decimal TongTien, string HanThanhToan, string TrangThai)
        {
            ViewBag.StudentID = "2415053122232";
            string query = $"UPDATE HoaDon SET MaHopDong = '{MaHopDong}', TongTien = {TongTien}, HanThanhToan = '{HanThanhToan}', TrangThai = N'{TrangThai}' WHERE MaHoaDon = '{MaHoaDon}'";

            try
            {
                _dbHelper.ExecuteNonQuery(query);
                TempData["Success"] = "Đã cập nhật hóa đơn thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi SQL: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DanhDauDaThu(string maHoaDon)
        {
            ViewBag.StudentID = "2415053122232";
            if (!string.IsNullOrEmpty(maHoaDon))
            {
                string query = $"UPDATE HoaDon SET TrangThai = N'Đã thanh toán' WHERE MaHoaDon = '{maHoaDon}'";
                try
                {
                    _dbHelper.ExecuteNonQuery(query);
                    TempData["Success"] = $"Đã cập nhật thanh toán cho hóa đơn {maHoaDon}!";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Lỗi SQL: " + ex.Message;
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult ChiTiet(string id)
        {
            try
            {
                ViewBag.StudentID = "2415053122232";

                string query = $@"
                    SELECT hd.*, k.HoTen, k.SoDienThoai, p.TenPhong 
                    FROM HoaDon hd
                    JOIN HopDong h ON hd.MaHopDong = h.MaHopDong
                    JOIN KhachThue k ON h.MaKhachThue = k.MaKhachThue
                    JOIN PhongTro p ON h.MaPhong = p.MaPhong
                    WHERE hd.MaHoaDon = '{id}'";

                DataTable dt = _dbHelper.ExecuteQuery(query);

                if (dt.Rows.Count == 0) return RedirectToAction("Index");

                return View(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                return Content("LỖI SQL TRANG CHI TIẾT: " + ex.Message);
            }
        }
    }
}