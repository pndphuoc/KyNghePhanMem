using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DomainModel
{
    public class NhanVien
    {
        public int MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Anh { get; set; }
        public string GhiChu { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime NgayVaoLam { get; set; }
        public bool isNam { get; set; }
    }
}
