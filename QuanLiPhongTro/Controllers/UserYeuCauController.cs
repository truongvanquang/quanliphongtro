using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QuanLiPhongTro.Data;
using System.Data;
using System;

namespace QuanLiPhongTro.Controllers
{
    public class UserYeuCauController : Controller
    {
        private readonly DBHelper _dbHelper;

        public UserYeuCauController(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IActionResult Index()
        {
            ViewBag.StudentID = "2415053122232";
            try
            {
                string email = HttpContext.Session.GetString("Email");
                if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "TaiKhoan");

                string queryKhach = $"SELECT k.MaKhachThue FROM KhachThue k JOIN TaiKhoan t ON k.MaTaiKhoan = t.MaTaiKhoan WHERE t.Email = '{email}'";
                DataTable dtKhach = _dbHelper.ExecuteQuery(queryKhach);

                if (dtKhach == null || dtKhach.Rows.Count == 0)
                {
                    ViewBag.TatCa = 0; ViewBag.DangCho = 0; ViewBag.DangXuLy = 0; ViewBag.DaXong = 0;
                    ViewBag.MaKhachThue = "";
                    ViewBag.PhongHienTai = new DataTable();
                    return View(new DataTable());
                }

                string maKhach = dtKhach.Rows[0]["MaKhachThue"].ToString();
                ViewBag.MaKhachThue = maKhach;

                string queryPhong = $@"
                    SELECT p.MaPhong, p.TenPhong 
                    FROM HopDong h 
                    JOIN PhongTro p ON h.MaPhong = p.MaPhong 
                    WHERE h.MaKhachThue = '{maKhach}' 
                    AND h.TrangThai NOT IN (N'Đã hủy', N'Đã kết thúc')";
                ViewBag.PhongHienTai = _dbHelper.ExecuteQuery(queryPhong);

                // Đã sửa lại đúng tên cột: TieuDe, MoTa, UuTien
                string queryYeuCau = $@"
                    SELECT y.MaYeuCau, y.NgayGui, y.TieuDe, y.MoTa, y.UuTien, y.TrangThai, p.TenPhong 
                    FROM YeuCauSuaChua y
                    JOIN PhongTro p ON y.MaPhong = p.MaPhong
                    WHERE y.MaKhachThue = '{maKhach}'
                    ORDER BY y.NgayGui DESC";

                DataTable dtYeuCau = _dbHelper.ExecuteQuery(queryYeuCau);

                int tatCa = 0, dangCho = 0, dangXuLy = 0, daXong = 0;

                if (dtYeuCau != null)
                {
                    tatCa = dtYeuCau.Rows.Count;
                    foreach (DataRow row in dtYeuCau.Rows)
                    {
                        string status = row["TrangThai"].ToString();
                        if (status == "Đang chờ") dangCho++;
                        else if (status == "Đang xử lý") dangXuLy++;
                        else if (status == "Đã xong") daXong++;
                    }
                }

                ViewBag.TatCa = tatCa;
                ViewBag.DangCho = dangCho;
                ViewBag.DangXuLy = dangXuLy;
                ViewBag.DaXong = daXong;

                return View(dtYeuCau);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "LỖI SQL: " + ex.Message;
                return View(new DataTable());
            }
        }

        [HttpPost]
        public IActionResult TaoYeuCau(string MaKhachThue, string MaPhong, string TieuDe, string MoTa, string UuTien)
        {
            ViewBag.StudentID = "2415053122232";
            try
            {
                if (string.IsNullOrEmpty(MaPhong))
                {
                    TempData["Error"] = "Bạn chưa có phòng hợp lệ để gửi yêu cầu.";
                    return RedirectToAction("Index");
                }

                string maYeuCau = "YC" + DateTime.Now.ToString("MMddHHmmss");
                string ngayGui = DateTime.Now.ToString("yyyy-MM-dd");

                // Đã sửa lại Insert vào đúng các cột TieuDe, MoTa, UuTien
                string queryInsert = $@"
                    INSERT INTO YeuCauSuaChua (MaYeuCau, MaKhachThue, MaPhong, TieuDe, MoTa, UuTien, NgayGui, TrangThai)
                    VALUES ('{maYeuCau}', '{MaKhachThue}', '{MaPhong}', N'{TieuDe}', N'{MoTa}', N'{UuTien}', '{ngayGui}', N'Đang chờ')";

                _dbHelper.ExecuteNonQuery(queryInsert);

                TempData["Success"] = "Tạo yêu cầu mới thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi lưu dữ liệu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}