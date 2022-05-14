using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer
{
    public interface ICongDAL
    {
        Cong GetCongTheoNgay(DateTime Ngay);

        Cong GetCongTheoThang(int Thang);

        Cong GetCongTheoNhanVien(int MaNhanVien);
        Cong GetCongTungThangCuaNhanVien(int MaNhanVien, int Thang);
        int CountCongTungThangCuaNhanVien(int MaNhanVien, int Thang, int Nam);
    }
}
