namespace QuanLiPhongTro.Models
{
    public class ChiTietHoaDon
    {
        public int MaChiTiet { get; set; }
        public string MaHoaDon { get; set; }
        public string TenDichVu { get; set; }
        public decimal DonGia { get; set; }
        public double SoLuong { get; set; }
        public decimal ThanhTien { get; set; } // SQL Server sẽ tự tính cột này, nhưng C# vẫn cần property để đọc lên
    }
}