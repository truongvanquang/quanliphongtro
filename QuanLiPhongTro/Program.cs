using QuanLiPhongTro.Data;
using System;

namespace QuanLiPhongTro
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<DBHelper>();
            builder.Services.AddSession();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=UserTaiKhoan}/{action=ManHinhChinh}/{id?}");

            Console.WriteLine("SV: 2415053122232");

            app.Run();
        }
    }
}