IF DB_ID('QuanLyPhongTroVN') IS NULL
BEGIN
    CREATE DATABASE QuanLyPhongTroVN
END
GO

USE QuanLyPhongTroVN
GO

IF OBJECT_ID('TaiKhoan', 'U') IS NULL
CREATE TABLE TaiKhoan (
    MaTaiKhoan VARCHAR(50) PRIMARY KEY,
    Email VARCHAR(100) UNIQUE,
    MatKhau VARCHAR(255),
    VaiTro NVARCHAR(50),
    TrangThai NVARCHAR(50)
)
GO

IF OBJECT_ID('KhachThue', 'U') IS NULL
CREATE TABLE KhachThue (
    MaKhachThue VARCHAR(50) PRIMARY KEY,
    MaTaiKhoan VARCHAR(50) FOREIGN KEY REFERENCES TaiKhoan(MaTaiKhoan),
    HoTen NVARCHAR(100),
    msv VARCHAR(20),
    CCCD VARCHAR(20),
    SoDienThoai VARCHAR(20),
    NgayThamGia DATE,
    TrangThai NVARCHAR(50)
)
GO

IF OBJECT_ID('LoaiPhong', 'U') IS NULL
CREATE TABLE LoaiPhong (
    MaLoai VARCHAR(50) PRIMARY KEY,
    TenLoai NVARCHAR(100)
)
GO

IF OBJECT_ID('TienIch', 'U') IS NULL
CREATE TABLE TienIch (
    MaTienIch VARCHAR(50) PRIMARY KEY,
    TenTienIch NVARCHAR(100),
    Icon VARCHAR(50)
)
GO

IF OBJECT_ID('PhongTro', 'U') IS NULL
CREATE TABLE PhongTro (
    MaPhong VARCHAR(50) PRIMARY KEY,
    MaLoai VARCHAR(50) FOREIGN KEY REFERENCES LoaiPhong(MaLoai),
    TenPhong NVARCHAR(100),
    DienTich FLOAT,
    GiaThue DECIMAL(18,2),
    TrangThai NVARCHAR(50)
)
GO

IF OBJECT_ID('PhongTienIch', 'U') IS NULL
CREATE TABLE PhongTienIch (
    MaPhong VARCHAR(50) FOREIGN KEY REFERENCES PhongTro(MaPhong),
    MaTienIch VARCHAR(50) FOREIGN KEY REFERENCES TienIch(MaTienIch),
    PRIMARY KEY (MaPhong, MaTienIch)
)
GO

IF OBJECT_ID('ChiSoDienNuoc', 'U') IS NULL
CREATE TABLE ChiSoDienNuoc (
    MaChiSo VARCHAR(50) PRIMARY KEY,
    MaPhong VARCHAR(50) FOREIGN KEY REFERENCES PhongTro(MaPhong),
    Thang INT,
    Nam INT,
    SoDienCu INT,
    SoDienMoi INT,
    SoNuocCu INT,
    SoNuocMoi INT
)
GO

IF OBJECT_ID('HopDong', 'U') IS NULL
CREATE TABLE HopDong (
    MaHopDong VARCHAR(50) PRIMARY KEY,
    MaKhachThue VARCHAR(50) FOREIGN KEY REFERENCES KhachThue(MaKhachThue),
    MaPhong VARCHAR(50) FOREIGN KEY REFERENCES PhongTro(MaPhong),
    NgayBatDau DATE,
    NgayKetThuc DATE,
    TienCoc DECIMAL(18,2),
    TrangThai NVARCHAR(50)
)
GO

IF OBJECT_ID('HoaDon', 'U') IS NULL
CREATE TABLE HoaDon (
    MaHoaDon VARCHAR(50) PRIMARY KEY,
    MaHopDong VARCHAR(50) FOREIGN KEY REFERENCES HopDong(MaHopDong),
    NgayLap DATE,
    HanThanhToan DATE,
    TongTien DECIMAL(18,2),
    TrangThai NVARCHAR(50)
)
GO

IF OBJECT_ID('ChiTietHoaDon', 'U') IS NULL
CREATE TABLE ChiTietHoaDon (
    MaChiTiet INT IDENTITY(1,1) PRIMARY KEY,
    MaHoaDon VARCHAR(50) FOREIGN KEY REFERENCES HoaDon(MaHoaDon),
    TenDichVu NVARCHAR(100),
    DonGia DECIMAL(18,2),
    SoLuong FLOAT,
    ThanhTien AS (DonGia * SoLuong)
)
GO

IF OBJECT_ID('LichSuThanhToan', 'U') IS NULL
CREATE TABLE LichSuThanhToan (
    MaGiaoDich VARCHAR(50) PRIMARY KEY,
    MaHoaDon VARCHAR(50) FOREIGN KEY REFERENCES HoaDon(MaHoaDon),
    NgayThanhToan DATETIME,
    SoTien DECIMAL(18,2),
    PhuongThuc NVARCHAR(50)
)
GO

IF OBJECT_ID('YeuCauSuaChua', 'U') IS NULL
CREATE TABLE YeuCauSuaChua (
    MaYeuCau VARCHAR(50) PRIMARY KEY,
    MaKhachThue VARCHAR(50) FOREIGN KEY REFERENCES KhachThue(MaKhachThue),
    MaPhong VARCHAR(50) FOREIGN KEY REFERENCES PhongTro(MaPhong),
    TieuDe NVARCHAR(200),
    MoTa NVARCHAR(MAX),
    UuTien NVARCHAR(50),
    TrangThai NVARCHAR(50),
    NgayGui DATETIME
)
GO

IF NOT EXISTS (
    SELECT 1 FROM TaiKhoan WHERE MaTaiKhoan = 'TK_ADMIN01'
)
INSERT INTO TaiKhoan (MaTaiKhoan, Email, MatKhau, VaiTro, TrangThai) 
VALUES ('TK_ADMIN01', 'admin@gmail.com', '123456', 'Admin', N'Hoạt động')
GO

IF NOT EXISTS (
    SELECT 1 FROM TaiKhoan WHERE MaTaiKhoan = 'TK_USER01'
)
INSERT INTO TaiKhoan (MaTaiKhoan, Email, MatKhau, VaiTro, TrangThai) 
VALUES ('TK_USER01', 'user@gmail.com', '123456', N'Khách thuê', N'Hoạt động')
GO

-- 1. Thêm dữ liệu bảng LoaiPhong
INSERT INTO LoaiPhong (MaLoai, TenLoai) 
VALUES 
    ('P01', N'VIP'),
    ('P02', N'THƯỜNG')
GO

-- 2. Thêm dữ liệu bảng KhachThue
INSERT INTO KhachThue (MaKhachThue, MaTaiKhoan, HoTen, msv, CCCD, SoDienThoai, NgayThamGia, TrangThai) 
VALUES
    ('KT002', NULL, N'Trần Thị B', NULL, '079111222333', '0912111222', '2026-05-01', N'Đang thuê'),
    ('KT003', NULL, N'Lê Văn C', NULL, '079333444555', '0912333444', '2026-05-10', N'Đang thuê'),
    ('KT004', NULL, N'Phạm Thị D', NULL, '079555666777', '0912555666', '2026-05-15', N'Đang thuê'),
    ('KT005', NULL, N'Hoàng Văn E', NULL, '079777888999', '0912777888', '2026-05-20', N'Đang thuê'),
    ('KT006', NULL, N'Đặng Thị F', NULL, '079999000111', '0912999000', '2026-05-25', N'Đang thuê')
GO

-- 3. Thêm dữ liệu bảng PhongTro
INSERT INTO PhongTro (MaPhong, MaLoai, TenPhong, DienTich, GiaThue, TrangThai) 
VALUES
    ('P01', NULL, N'Phòng 101 VIP', 30, 3500000.00, N'Đã thuê'),
    ('P02', NULL, N'Phòng 102', 25, 3200000.00, N'Đang chờ'),
    ('P03', NULL, N'Phòng 103', 25, 3200000.00, N'Đang chờ'),
    ('P04', NULL, N'Phòng 201', 30, 4000000.00, N'Đang chờ'),
    ('P05', NULL, N'Phòng 202', 30, 4000000.00, N'Đang chờ'),
    ('P06', NULL, N'Phòng 301', 35, 4500000.00, N'Đang chờ'),
    ('RM_01', 'P01', N'TẦNG 2', 50, 30000000.00, N'Đang chờ')
GO

-- 4. Thêm dữ liệu bảng HopDong
INSERT INTO HopDong (MaHopDong, MaKhachThue, MaPhong, NgayBatDau, NgayKetThuc, TienCoc, TrangThai) 
VALUES
    ('HD002', 'KT002', 'P02', '2026-05-01', '2027-05-01', 5000000.00, N'Hiệu lực'),
    ('HD003', 'KT003', 'P03', '2026-05-10', '2027-05-10', 5000000.00, N'Hiệu lực'),
    ('HD004', 'KT004', 'P04', '2026-05-15', '2027-05-15', 6000000.00, N'Hiệu lực'),
    ('HD005', 'KT005', 'P05', '2026-05-20', '2027-05-20', 6000000.00, N'Hiệu lực'),
    ('HD006', 'KT006', 'P06', '2026-05-25', '2027-05-25', 7000000.00, N'Hiệu lực')
GO

-- 5. Thêm dữ liệu bảng YeuCauSuaChua
INSERT INTO YeuCauSuaChua (MaYeuCau, MaKhachThue, MaPhong, TieuDe, MoTa, UuTien, TrangThai, NgayGui) 
VALUES
    ('YC001', 'KT002', 'P02', N'Hỏng điều hòa', N'Điều hòa không mát, kêu to bất thường', N'Cao', N'Đang chờ', '2026-05-20 00:00:00.000'),
    ('YC002', 'KT003', 'P03', N'Bóng đèn cháy', N'Bóng đèn trong nhà vệ sinh bị cháy', N'Thấp', N'Đã hoàn thành', '2026-05-25 00:00:00.000'),
    ('YC003', 'KT004', 'P04', N'Vòi nước rỉ', N'Vòi nước bồn rửa mặt bị rỉ nước liên tục', N'Trung bình', N'Đang xử lý', '2026-05-29 00:00:00.000'),
    ('YC004', 'KT004', 'P04', N'Hư tủ lạnh', N'hỏng', N'Thấp', N'Đang chờ', '2026-05-29 00:00:00.000')
GO