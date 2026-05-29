using Microsoft.AspNetCore.Mvc;
using QuanLiPhongTro.Data;
using System.Data;
using System;

namespace QuanLiPhongTro.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBHelper _dbHelper;

        public HomeController(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IActionResult Index()
        {
            ViewBag.StudentID = "2415053122232";

            DataTable dtPhong = _dbHelper.ExecuteQuery("SELECT COUNT(*) AS Total FROM PhongTro");
            DataTable dtTrong = _dbHelper.ExecuteQuery("SELECT COUNT(*) AS Total FROM PhongTro WHERE TrangThai = N'Đang trống'");
            DataTable dtKhach = _dbHelper.ExecuteQuery("SELECT COUNT(*) AS Total FROM KhachThue");
            DataTable dtDoanhThu = _dbHelper.ExecuteQuery("SELECT SUM(TongTien) AS Total FROM HoaDon WHERE TrangThai = N'Đã thanh toán'");

            int totalRooms = 0;
            int emptyRooms = 0;
            int tenants = 0;
            decimal revenue = 0;

            if (dtPhong.Rows.Count > 0 && dtPhong.Rows[0]["Total"] != DBNull.Value) totalRooms = Convert.ToInt32(dtPhong.Rows[0]["Total"]);
            if (dtTrong.Rows.Count > 0 && dtTrong.Rows[0]["Total"] != DBNull.Value) emptyRooms = Convert.ToInt32(dtTrong.Rows[0]["Total"]);
            if (dtKhach.Rows.Count > 0 && dtKhach.Rows[0]["Total"] != DBNull.Value) tenants = Convert.ToInt32(dtKhach.Rows[0]["Total"]);
            if (dtDoanhThu.Rows.Count > 0 && dtDoanhThu.Rows[0]["Total"] != DBNull.Value) revenue = Convert.ToDecimal(dtDoanhThu.Rows[0]["Total"]);

            ViewBag.TotalRooms = totalRooms;
            ViewBag.EmptyRooms = emptyRooms;
            ViewBag.Tenants = tenants;
            ViewBag.Revenue = revenue;

            ViewBag.TongSoPhong = totalRooms;
            ViewBag.PhongTrong = emptyRooms;
            ViewBag.DaThue = totalRooms - emptyRooms;
            ViewBag.DoanhThu = revenue;
            ViewBag.TyLeLapDay = totalRooms > 0 ? ((totalRooms - emptyRooms) * 100) / totalRooms : 0;

            decimal[] monthlyRevenue = new decimal[6] { 15000000, 24000000, 18000000, 29000000, 35000000, 45000000 };

            try
            {
                DataTable dtChart = _dbHelper.ExecuteQuery("SELECT MONTH(NgayLap) AS Thang, SUM(TongTien) AS Tong FROM HoaDon WHERE TrangThai = N'Đã thanh toán' AND NgayLap >= DATEADD(month, -6, GETDATE()) GROUP BY MONTH(NgayLap)");
                foreach (DataRow row in dtChart.Rows)
                {
                    int m = Convert.ToInt32(row["Thang"]);
                    decimal val = Convert.ToDecimal(row["Tong"]);
                    if (m >= 1 && m <= 6)
                    {
                        monthlyRevenue[m - 1] = val;
                    }
                }
            }
            catch { }

            ViewBag.MonthlyRevenue = monthlyRevenue;

            return View();
        }
    }
}