using System;

namespace QuanLiPhongTro.Models
{
    public class HoaDon
    {
        public string MaHoaDon { get; set; }
        public string MaHopDong { get; set; }
        public DateTime NgayLap { get; set; }
        public DateTime HanThanhToan { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
    }
}