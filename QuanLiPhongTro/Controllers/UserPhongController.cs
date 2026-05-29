using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QuanLiPhongTro.Data;
using System.Data;
using System;

namespace QuanLiPhongTro.Controllers
{
    public class UserPhongController : Controller
    {
        private readonly DBHelper _dbHelper;

        public UserPhongController(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IActionResult Index(string khuVuc, string khongGia, string loaiPhong)
        {
            ViewBag.StudentID = "2415053122232";
            ViewBag.KhuVuc = khuVuc;
            ViewBag.KhongGia = khongGia;
            ViewBag.LoaiPhong = loaiPhong;

            try
            {
                string query = @"
                    SELECT p.MaPhong, p.TenPhong, p.DienTich, p.GiaThue, p.TrangThai,
                           l.TenLoai
                    FROM PhongTro p
                    LEFT JOIN LoaiPhong l ON p.MaLoai = l.MaLoai
                    WHERE p.TrangThai = N'Đang trống'";

                if (!string.IsNullOrEmpty(khongGia))
                {
                    if (khongGia == "duoi3") query += " AND p.GiaThue < 3000000";
                    else if (khongGia == "3den5") query += " AND p.GiaThue BETWEEN 3000000 AND 5000000";
                    else if (khongGia == "tren5") query += " AND p.GiaThue > 5000000";
                }

                if (!string.IsNullOrEmpty(loaiPhong))
                    query += $" AND l.TenLoai LIKE N'%{loaiPhong}%'";

                query += " ORDER BY p.GiaThue ASC";

                DataTable dt = _dbHelper.ExecuteQuery(query);
                return View(dt);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi: " + ex.Message;
                return View(new DataTable());
            }
        }

        public IActionResult Detail(string id)
        {
            ViewBag.StudentID = "2415053122232";
            try
            {
                string query = $@"
                    SELECT p.MaPhong, p.TenPhong, p.DienTich, p.GiaThue, p.TrangThai,
                           l.TenLoai
                    FROM PhongTro p
                    LEFT JOIN LoaiPhong l ON p.MaLoai = l.MaLoai
                    WHERE p.MaPhong = '{id}'";

                DataTable dt = _dbHelper.ExecuteQuery(query);
                if (dt.Rows.Count == 0) return RedirectToAction("Index");
                return View(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                return Content("Lỗi: " + ex.Message);
            }
        }

        public IActionResult XacNhan(string maPhong)
        {
            ViewBag.StudentID = "2415053122232";
            string query = $"SELECT MaPhong, TenPhong, GiaThue FROM PhongTro WHERE MaPhong = '{maPhong}'";
            DataTable dt = _dbHelper.ExecuteQuery(query);
            if (dt.Rows.Count == 0) return RedirectToAction("Index");
            return View(dt.Rows[0]);
        }

        [HttpPost]
        [HttpPost]
        public IActionResult DatPhong(string maPhong, decimal giaThue)
        {
            ViewBag.StudentID = "2415053122232";
            try
            {
                string email = HttpContext.Session.GetString("Email");
                if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "TaiKhoan");

                string queryTK = $"SELECT MaTaiKhoan FROM TaiKhoan WHERE Email = '{email}'";
                DataTable dtTK = _dbHelper.ExecuteQuery(queryTK);
                if (dtTK.Rows.Count == 0) return RedirectToAction("Login", "TaiKhoan");
                string maTaiKhoan = dtTK.Rows[0]["MaTaiKhoan"].ToString();

                string queryKhach = $"SELECT MaKhachThue FROM KhachThue WHERE MaTaiKhoan = '{maTaiKhoan}'";
                DataTable dtKhach = _dbHelper.ExecuteQuery(queryKhach);

                string maKhach = "";
                if (dtKhach.Rows.Count == 0)
                {
                    maKhach = "KT" + DateTime.Now.ToString("MMddHHmmss");
                    string insertKhach = $"INSERT INTO KhachThue (MaKhachThue, MaTaiKhoan, HoTen) VALUES ('{maKhach}', '{maTaiKhoan}', N'Khách mới')";
                    _dbHelper.ExecuteNonQuery(insertKhach);
                }
                else
                {
                    maKhach = dtKhach.Rows[0]["MaKhachThue"].ToString();
                }

                // 1. TẠO HỢP ĐỒNG
                string maHopDong = "HD" + DateTime.Now.ToString("yyMMddHHmmss");
                string ngayBatDau = DateTime.Now.ToString("yyyy-MM-dd");
                string ngayKetThuc = DateTime.Now.AddMonths(6).ToString("yyyy-MM-dd");

                string queryInsertHD = $@"
            INSERT INTO HopDong (MaHopDong, MaKhachThue, MaPhong, NgayBatDau, NgayKetThuc, TienCoc, TrangThai)
            VALUES ('{maHopDong}', '{maKhach}', '{maPhong}', '{ngayBatDau}', '{ngayKetThuc}', {giaThue}, N'Chờ ký')";
                _dbHelper.ExecuteNonQuery(queryInsertHD);

                // 2. KHÓA PHÒNG LẠI
                _dbHelper.ExecuteNonQuery($"UPDATE PhongTro SET TrangThai = N'Đang chờ' WHERE MaPhong = '{maPhong}'");

                // 3. TỰ ĐỘNG SINH HÓA ĐƠN TIỀN CỌC
                string maHoaDon = "INV" + DateTime.Now.ToString("MMddHHmmss");
                string hanThanhToan = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd"); // Hạn nộp cọc là 3 ngày

                string queryInsertHoaDon = $@"
            INSERT INTO HoaDon (MaHoaDon, MaHopDong, NgayLap, HanThanhToan, TongTien, TrangThai)
            VALUES ('{maHoaDon}', '{maHopDong}', '{ngayBatDau}', '{hanThanhToan}', {giaThue}, N'Chưa thanh toán')";
                _dbHelper.ExecuteNonQuery(queryInsertHoaDon);

                TempData["Success"] = "Đặt phòng thành công! Đã tự động tạo hóa đơn tiền cọc. Vui lòng thanh toán để hoàn tất.";

                // Chuyển thẳng sang trang Thanh toán luôn
                return RedirectToAction("Index", "UserThanhToan");
            }
            catch (Exception ex)
            {
                return Content("LỖI ĐẶT PHÒNG: " + ex.Message);
            }
        }
    }
}