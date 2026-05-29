using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QuanLiPhongTro.Data;
using System.Data;
using System;

namespace QuanLiPhongTro.Controllers
{
    public class UserThanhToanController : Controller
    {
        private readonly DBHelper _dbHelper;

        public UserThanhToanController(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IActionResult Index()
        {
            try
            {
                string email = HttpContext.Session.GetString("Email");
                if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "TaiKhoan");

                ViewBag.StudentID = "2415053122232";

                string queryKhach = $@"
                    SELECT k.MaKhachThue 
                    FROM KhachThue k
                    JOIN TaiKhoan t ON k.MaTaiKhoan = t.MaTaiKhoan
                    WHERE t.Email = '{email}'";

                DataTable dtKhach = _dbHelper.ExecuteQuery(queryKhach);

                if (dtKhach == null || dtKhach.Rows.Count == 0)
                {
                    ViewBag.TongDaThanhToan = 0m;
                    ViewBag.ChuaThanhToan = 0m;
                    return View(new DataTable());
                }

                string maKhach = dtKhach.Rows[0]["MaKhachThue"].ToString();

                string queryLichSu = $@"
                    SELECT hd.MaHoaDon, hd.TongTien, hd.HanThanhToan, hd.TrangThai, hd.NgayLap, p.TenPhong
                    FROM HoaDon hd
                    JOIN HopDong h ON hd.MaHopDong = h.MaHopDong
                    JOIN PhongTro p ON h.MaPhong = p.MaPhong
                    WHERE h.MaKhachThue = '{maKhach}'
                    ORDER BY hd.HanThanhToan DESC";

                DataTable dtLichSu = _dbHelper.ExecuteQuery(queryLichSu);

                decimal tongDaThanhToan = 0;
                decimal chuaThanhToan = 0;

                if (dtLichSu != null)
                {
                    foreach (DataRow row in dtLichSu.Rows)
                    {
                        decimal tien = Convert.ToDecimal(row["TongTien"]);
                        string trangThai = row["TrangThai"].ToString() ?? "";

                        if (trangThai == "Đã thanh toán" || trangThai.ToUpper() == "ĐÃ THANH TOÁN")
                        {
                            tongDaThanhToan += tien;
                        }
                        else
                        {
                            chuaThanhToan += tien;
                        }
                    }
                }

                ViewBag.TongDaThanhToan = tongDaThanhToan;
                ViewBag.ChuaThanhToan = chuaThanhToan;

                return View(dtLichSu);
            }
            catch (Exception ex)
            {
                return Content("LỖI SQL TẠI USER THANH TOÁN: " + ex.Message);
            }
        }

        // Hàm này dùng để gọi giao diện Xác nhận thanh toán (Chọn Momo/Ngân hàng)
        public IActionResult XacNhanThanhToan(string id)
        {
            ViewBag.StudentID = "2415053122232";
            try
            {
                string query = $@"
                    SELECT hd.MaHoaDon, hd.TongTien, hd.HanThanhToan, p.TenPhong 
                    FROM HoaDon hd
                    JOIN HopDong h ON hd.MaHopDong = h.MaHopDong
                    JOIN PhongTro p ON h.MaPhong = p.MaPhong
                    WHERE hd.MaHoaDon = '{id}'";

                DataTable dt = _dbHelper.ExecuteQuery(query);
                if (dt.Rows.Count == 0) return RedirectToAction("Index");

                return View(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                return Content("LỖI SQL: " + ex.Message);
            }
        }

        // Hàm này xử lý khi khách bấm nút "Xác nhận & Thanh toán"
        [HttpPost]
        public IActionResult ThanhToanNgay(string maHoaDon, string phuongThuc)
        {
            ViewBag.StudentID = "2415053122232";
            try
            {
                string note = string.IsNullOrEmpty(phuongThuc) ? "Tiền mặt" : phuongThuc;

                // Cập nhật trạng thái hóa đơn thành Đã thanh toán
                string query = $"UPDATE HoaDon SET TrangThai = N'Đã thanh toán' WHERE MaHoaDon = '{maHoaDon}'";
                _dbHelper.ExecuteNonQuery(query);

                TempData["Success"] = $"Đã thanh toán thành công hóa đơn {maHoaDon} qua {note}!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content("LỖI THANH TOÁN: " + ex.Message);
            }
        }
    }
}