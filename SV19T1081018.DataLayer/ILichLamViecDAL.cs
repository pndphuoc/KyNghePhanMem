using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer
{
    public interface ILichLamViecDAL
    {
        IList<LichLamViec> List(int page = 1, int pageSize = 0, string searchValue = "");
        int Check(int MaThu, int MaCaLamViec, int MaNhanVien);
        bool Add(LichLamViec data);
        bool Delete(LichLamViec data);
    }
}
