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
        public const string EMAIL = "imelda.strosin46@ethereal.email";
        public const string PASSWORD = "bSJUC6kwUFdVuvPyQY";
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

        public const int FORGET_TOKEN_EXPIRED = 8;
        public const int DATA_TIDAK_SESUAI = 9;

        public const int MANAGER_DILARANG_APPROVE_CUTI = 10;

     


        public const int SUCCESS = 200;
        public const int FAIL = 400;

    }

    

    public class HtmlTemplate
    {
        public static string RequestLeaving(string namaManager,string namaEmployee, string dateFrom, string dateEnd)
        {
            return @"<div style='font - family: Helvetica,Arial,sans - serif; min-width:1000px; overflow: auto; line - height:2'>
              <div style = 'margin:50px auto;width:70%;padding:20px 0'>
                 <div style = 'border-bottom:1px solid #eee'>
                    <a href = '' style = 'font-size:1.4em;color: #00466a;text-decoration:none;font-weight:600' > e-Cuti </a>
                 </div>
                         <p style = 'font-size:1.1em'> Hi, " + namaEmployee + @"</p>
                              <p> Thank you for choosing e-Cuti to make your leave request. </p>
                              <p> Your Leave Request has been approved. Your leave start at "+dateFrom+@" until "+dateEnd+@" </p>
                              <br/>
                              <p> Please use this email use properly. </p>
                               <p style = 'font-size:0.9em;' > Regards,<br/> e-Cuti </p>
                               <hr style = 'border:none;border-top:1px solid #eee'/>
                               <div style = 'float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300'>
                                 <p> "+namaManager+@" </p>
                                 <p> APL Tower, Jakarta </p>
                                 <p> Indonesia </p>
                               </div>
                             </div>
                           </div> ";
            /*return @"
               <div>
                    <h1>From:"+namaManager+@"</h1>

                    <p>Pegawai atas nama "+namaEmployee+ @" ini diizin kan cuti dari tanggal " + dateFrom + @" sampai " + dateEnd + @"</p>
                    <p>tolong surat ini digunakan seperlunya</p>
                     <br/><br/>
                    <p>ttd. " + namaManager + @"</p>
               </div>
                ";*/
        }

        public static string ForgotPasswordTemplate( string otp, string expiredTime, string email)
        {
            return @"<div style='font - family: Helvetica,Arial,sans - serif; min-width:1000px; overflow: auto; line - height:2'>
              <div style = 'margin:50px auto;width:70%;padding:20px 0'>
                 <div style = 'border-bottom:1px solid #eee'>
                    <a href = '' style = 'font-size:1.4em;color: #00466a;text-decoration:none;font-weight:600' > e-Cuti </a>
                 </div>
                         <p style = 'font-size:1.1em'> Hi, " +email+ @"</p>
                              <p> You have 5 minutes, please use the OTP code below on e-Cute Website: </p>
                               <h2 style = 'background: #00466a;margin: 0 auto;width: max-content;padding: 0 10px;color: #fff;border-radius: 4px;' > " +otp+ @"</h2>
                              <p> Thank you for choosing e-Cuti. Use the following OTP to complete your Forgot Password procedures. OTP code is valid until <b>" +expiredTime+ @"</b> </p>
                               <p style = 'font-size:0.9em;' > Regards,<br/> e-Cuti </p>
                               <hr style = 'border:none;border-top:1px solid #eee'/>
                               <div style = 'float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300'>
                                 <p> e-Cuti </p>
                                 <p> APL Tower, Jakarta </p>
                                 <p> Indonesia </p>
                               </div>
                             </div>
                           </div> ";

            /*return @" <h1>Forgot Password</h1>
               <p>Otp : <bold>" + otp + @"</bold></p> 
               <p>Email : " + email + @"</p>
               <p>Expired_time : " + expiredTime + @"</p>
               <br/><br/><br/>
               <p>Segera ganti password Anda Sebelum expired time</p>";*/
        }


    }
}
