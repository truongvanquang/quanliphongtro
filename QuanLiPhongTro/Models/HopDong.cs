using System;

namespace QuanLiPhongTro.Models
{
    public class HopDong
    {
        public string MaHopDong { get; set; }
        public string MaKhachThue { get; set; }
        public string MaPhong { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public decimal TienCoc { get; set; }
        public string TrangThai { get; set; }
    }
}