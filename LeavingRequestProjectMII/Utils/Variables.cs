using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Utils
{

    //Jangan lupa selalu diupdate
    public class VariableServices
    {
        //for Email Services
        public const string EMAIL = "brannon.flatley65@ethereal.email";
        public const string PASSWORD = "Xf1C1p6MMU2pPsBMV4";
        public const string CLIENT_DOMAIN = "";
        public const string API_DOMAIN = "";
    }
    public class Variables
    {
        public const int EMAIL_DUPLICATE = 1;
        public const int NO_TELP_DUPLICATE = 2;

        public const int WRONG_PASSWORD = 3;
        public const int WRONG_EMAIL = 4;

        public const int JUMLAH_CUTI_TIDAK_MENCUKUPI=5;
        public const int CUTI_SUDAH_HABIS = 6;
        public const int SYARAT_MIN_TANGGAL_REQUEST = 7;

        public const int SUCCESS = 200;
        public const int FAIL = 400;

    }

    

    public class HtmlTemplate
    {
        public static string RequestLeaving(string namaManager,string namaEmployee, string dateFrom, string dateEnd)
        {
            return @"
               <div>
                    <h1>From:"+namaManager+@"</h1>

                    <p>Pegawai atas nama "+namaEmployee+ @" ini diizin kan cuti dari tanggal " + dateFrom + @" sampai " + dateEnd + @"</p>
                    <p>tolong surat ini digunakan seperlunya</p>
                     <br/><br/>
                    <p>ttd. " + namaManager + @"</p>
               </div>
                ";
        }
    }
}
