using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DomainModel
{
    public class Cong
    {
        public int MaCong { get; set; }
        public int MaNhanVien { get; set; }
        public int MaCaLamViec { get; set; }
        public DateTime ThoiGianVaoCa { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public bool status { get; set; }
    }
}
