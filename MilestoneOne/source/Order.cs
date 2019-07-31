using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilestoneOne.source
{
    public class Order
    {
        public double TongTien { get; set; }
        public int SoLuongSanPham { get; set; }
        public string tinhTrang { get; set; }
        public List<Product> danhSachSanPhamTrongDonHang { get; set; }
        public string idHoaDon { get; set; }
    }
}
