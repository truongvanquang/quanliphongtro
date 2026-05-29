using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QuanLiPhongTro.Data;
using System.Data;
using System;

namespace QuanLiPhongTro.Controllers
{
    public class UserDashboardController : Controller
    {
        private readonly DBHelper _dbHelper;

        public UserDashboardController(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IActionResult Index()
        {
            string email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "TaiKhoan");

            ViewBag.StudentID = "2415053122232";
            ViewBag.UserEmail = email;

            try
            {
                // Cố gắng lấy thông tin khách thuê
                // LƯU Ý: Nếu query này lỗi, hãy kiểm tra lại tên cột 'Email' trong bảng 'KhachThue'
                string queryThongTin = $@"
                    SELECT TOP 1 k.MaKhachThue, k.HoTen, p.TenPhong, p.GiaThue, h.TienCoc, h.NgayLap, h.NgayKetThuc 
                    FROM KhachThue k
                    JOIN HopDong h ON k.MaKhachThue = h.MaKhachThue
                    JOIN PhongTro p ON h.MaPhong = p.MaPhong
                    WHERE k.Email = '{email}' AND h.TrangThai = N'Đang hiệu lực'
                    ORDER BY h.NgayLap DESC";

                DataTable dtThongTin = _dbHelper.ExecuteQuery(queryThongTin);

                if (dtThongTin != null && dtThongTin.Rows.Count > 0)
                {
                    ViewBag.ThongTin = dtThongTin.Rows[0];
                    string maKhach = dtThongTin.Rows[0]["MaKhachThue"].ToString();

                    string queryHoaDon = $@"
                        SELECT TOP 1 TongTien, HanThanhToan 
                        FROM HoaDon hd 
                        JOIN HopDong h ON hd.MaHopDong = h.MaHopDong 
                        WHERE h.MaKhachThue = '{maKhach}' AND hd.TrangThai != N'Đã thanh toán'
                        ORDER BY hd.HanThanhToan ASC";

                    DataTable dtHoaDon = _dbHelper.ExecuteQuery(queryHoaDon);
                    if (dtHoaDon != null && dtHoaDon.Rows.Count > 0)
                    {
                        ViewBag.ThanhToanTiepTheo = dtHoaDon.Rows[0]["TongTien"];
                        ViewBag.HanChot = dtHoaDon.Rows[0]["HanThanhToan"];
                    }
                }
            }
            catch (Exception ex)
            {
                // Nếu lỗi SQL, hiện thông báo để ông biết đường sửa, không làm sập web
                ViewBag.Error = "Lỗi SQL: " + ex.Message;
            }

            return View();
        }
    }
}