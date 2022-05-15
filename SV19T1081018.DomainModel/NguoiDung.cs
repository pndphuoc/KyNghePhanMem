using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DomainModel
{
    public class NguoiDung
    {
        public int MaNguoiDung { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string HoTen { get; set; }
        public bool isChuCuaHang { get; set; }
    }
}
