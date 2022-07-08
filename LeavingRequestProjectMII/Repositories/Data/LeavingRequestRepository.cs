using API.Context;
using API.Models;
using API.ModelsView;
using API.ModelsInsert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Utils;

namespace API.Repositories.Data
{
    public class LeavingRequestRepository
    {
        private readonly MyContext context;
        private readonly NationalDayServices nationalDay;

        public LeavingRequestRepository(MyContext context)
        {
            this.context = context;
            this.nationalDay = new NationalDayServices();
        }

        //untuk isRead Notifikasi 
        public int IsReadNotif(string request_id)
        {
            LeavingRequest lr = context.leavingRequests.Find(request_id);
            lr.isRead = true;
            context.leavingRequests.Update(lr);
            int chk =context.SaveChanges();

            if (chk > 0) return Variables.SUCCESS;
            else return Variables.FAIL;
        }

        //untuk notfikasi update employee
        public Object GetNotifikasiEmployee(string employee_id)
        {
            return context.leavingRequests.Where(lr=>lr.employees.employee_id==employee_id && lr.isDelete==false && lr.isRead == false).Select(lr => new
            {
                request_id = lr.request_id,
                approvalMessage = lr.approvalMessage,
                leavingMessage = lr.leavingMessage,

                status = lr.approvalStatus.ToString()
            }).ToList();
        }

        //ngeliat request punya sendiri
        public Object GetLeavingEmployee(string employee_id)
        {
            return context.leavingRequests.Where(lr => lr.employee_id == employee_id)
                  .Select(lr => new LeavingRequestViewModel
                  {
                      request_id = lr.request_id,
                      employee_id = lr.employee_id,
                      employeeName = lr.employees.name,
                      categoryName = lr.leaveCategory.nameCategory,
                      approvalStatus = lr.approvalStatus.ToString(),
                      requestTime = lr.requestTime,
                      startDate = lr.startDate,
                      endDate = lr.endDate,
                      approvalMessage = lr.approvalMessage
                  }).ToList();
        }

        public Object GetLeavingEmployeeDetail(string request_id)
        {
            return context.leavingRequests
                  .Select(lr => new 
                  {
                      request_id = lr.request_id,
                      employee_id = lr.employee_id,
                      approvalStatus = lr.approvalStatus,
                      approvalStatusName = lr.approvalStatus.ToString(),
                      requestTime = lr.requestTime,
                      startDate = lr.startDate,
                      endDate = lr.endDate,
                      approvalMessage = lr.approvalMessage,
                      category_id = lr.category_id,
                      category_name = lr.leaveCategory.nameCategory,
                      leavingMessage = lr.leavingMessage,
                      fileBukti = lr.fileBukti,
                      tipeFileBukti = lr.tipeFileBukti.ToString(),
                       namaFileBukti = lr.namaFileBukti,

                  }).FirstOrDefault(lr => lr.request_id == request_id);
        }
        public Object DownloadFileBukti(string request_id)
        {
            return context.leavingRequests
                  .Select(lr => new LeavingRequest
                  {
                      request_id = lr.request_id,
                      employee_id = lr.employee_id,
                      fileBukti = lr.fileBukti,
                      tipeFileBukti = lr.tipeFileBukti
                  }).FirstOrDefault(lr => lr.request_id == request_id);
        }

        //untuk dashboart manager
        public object CountStatisticManager(string manager_id)
        {
            List<LeavingRequest> lr = context.leavingRequests.Where(lr => lr.employees.manager_id == manager_id && lr.isDelete == false).ToList();
            return new
            {
                total = lr.Count(),
                totalApprove = lr.Where(lr=>lr.approvalStatus== Approval_status.Diterima).Count(),
                totalRevisi= lr.Where(lr=>lr.approvalStatus== Approval_status.Revisi).Count(),
                totalReject = lr.Where(lr=>lr.approvalStatus== Approval_status.Ditolak).Count(),
                totalMenunggu = lr.Where(lr=>lr.approvalStatus== Approval_status.Menunggu).Count(),
            };
                    
        }

        //untuk dashboart emp
        public object CountStatisticEmployee(string employee_id)
        {
            List<LeavingRequest> lr = context.leavingRequests.Where(lr => lr.employees.employee_id == employee_id && lr.isDelete == false).ToList();
            return new
            {
                total = lr.Count(),
                totalApprove = lr.Where(lr => lr.approvalStatus == Approval_status.Diterima).Count(),
                totalRevisi = lr.Where(lr => lr.approvalStatus == Approval_status.Revisi).Count(),
                totalReject = lr.Where(lr => lr.approvalStatus == Approval_status.Ditolak).Count(),
                totalMenunggu = lr.Where(lr => lr.approvalStatus == Approval_status.Menunggu).Count(),
            };

        }

        //ngelihat daftar cuti anak buah
        public Object GetLeavingManager(string manager_id)
        {
            return context.leavingRequests.Where(lr => lr.employees.manager_id == manager_id)
                  .Select(lr => new LeavingRequestViewModel
                  {
                      request_id = lr.request_id,
                      employee_id = lr.employee_id,
                      employeeName = lr.employees.name,
                      categoryName = lr.leaveCategory.nameCategory,
                      approvalStatus = lr.approvalStatus.ToString(),
                      requestTime = lr.requestTime,
                      startDate = lr.startDate,
                      endDate = lr.endDate,
                      approvalMessage = lr.approvalMessage,
                      
                  }).ToList();
        }

        public async Task<int> InsertLeaving(LeavingRequestInserModel leavingRequestInser)
        {
            //if (DateTime.Now.Month == 1 && DateTime.Now.Day == 1) return Variables.MANAGER_DILARANG_APPROVE_CUTI;
            var data = DateTime.Now;
            if (!  await TotalSisaHariCutiApprove(leavingRequestInser)) return Variables.CUTI_SUDAH_HABIS;
            else if ((leavingRequestInser.startDate - DateTime.Now).TotalDays < 6) return Variables.SYARAT_MIN_TANGGAL_REQUEST;

            LeavingRequest leavingRequest = new LeavingRequest()
            {
                request_id = "Leave" + GetAutoIncrementConvertString(),
                employee_id = leavingRequestInser.employee_id,
                category_id = leavingRequestInser.category_id,
                approvalStatus = Approval_status.Menunggu,
                requestTime = DateTime.Now,
                startDate = leavingRequestInser.startDate,
                endDate = leavingRequestInser.endDate,
                leavingMessage = leavingRequestInser.leavingMessage,
                fileBukti = leavingRequestInser.fileBukti,
                namaFileBukti = leavingRequestInser.namaFileBukti,
                tipeFileBukti = (TipeFileBukti)Enum.Parse(typeof(TipeFileBukti), leavingRequestInser.tipeFileBukti),
                approvalMessage = "Menunggu",
                isRead = true,
                isDelete = false
            };
            context.leavingRequests.Add(leavingRequest);
            return context.SaveChanges();
        }

        public async Task<int> UpdateLeaving(LeavingRequestInserModel leaving)
        {

            if (!await TotalSisaHariCutiApprove(new LeavingRequestInserModel
            {
                employee_id = leaving.employee_id,
                startDate = leaving.startDate,
                endDate = leaving.endDate
            })) return Variables.JUMLAH_CUTI_TIDAK_MENCUKUPI;

            LeavingRequest lrUpdate = context.leavingRequests.Find(leaving.request_id);
            lrUpdate.leavingMessage = leaving.leavingMessage;
            lrUpdate.category_id = leaving.category_id;
            lrUpdate.startDate = leaving.startDate;
            lrUpdate.endDate = leaving.endDate;
            lrUpdate.isRead = true;

            if(leaving.fileBukti!=null )
            {
                lrUpdate.fileBukti = leaving.fileBukti;
                lrUpdate.namaFileBukti = leaving.namaFileBukti;
                lrUpdate.tipeFileBukti = (TipeFileBukti)Enum.Parse(typeof(TipeFileBukti), leaving.tipeFileBukti);
            }

            context.leavingRequests.Update(lrUpdate);
            int ctx = context.SaveChanges();

            if (ctx > 0) return Variables.SUCCESS;
            else return Variables.FAIL;
        }

        public int CountNotReadRequest(string employee_id)
        {
            return context.leavingRequests.Where(lr => lr.employee_id == employee_id && lr.isRead == false).Count();
        }

        public async Task<Tuple<int, string>> ApproveLeaving(string request_id,string approvalMessage) 
        {
           
            LeavingRequest leavingRequest = context.leavingRequests.Find(request_id);
            Employees managerDetail = context.employees.Find(leavingRequest.employees.manager_id);
            string namaEmp = leavingRequest.employees.name;
             

            if (!await TotalSisaHariCutiApprove(new LeavingRequestInserModel {
                employee_id = leavingRequest.employee_id,
                startDate = leavingRequest.startDate,
                endDate = leavingRequest.endDate
            })) return new Tuple<int, string>(Variables.JUMLAH_CUTI_TIDAK_MENCUKUPI, namaEmp);


            leavingRequest.approvalMessage = approvalMessage;
            leavingRequest.approvalStatus = Approval_status.Diterima;
            leavingRequest.isRead = false;
            leavingRequest.employees.sisaCuti = 
                leavingRequest.employees.sisaCuti-(
                    (leavingRequest.endDate - leavingRequest.startDate.AddDays(-1)).Days  - await nationalDay.CountPotonganLibur(leavingRequest.startDate,leavingRequest.endDate)
                );
            context.leavingRequests.Update(leavingRequest);
            EmailServices.SendEmail(leavingRequest.employees.email, "Perihal Cuti", HtmlTemplate.RequestLeaving(managerDetail.name, leavingRequest.employees.name, leavingRequest.startDate.ToString("D"), leavingRequest.endDate.ToString("D")));
            int chk = context.SaveChanges();

            if (chk > 0) return new Tuple<int, string>(Variables.SUCCESS, namaEmp); 
            else return new Tuple<int, string>(Variables.FAIL, namaEmp); 
            
        }

        public int RejectLeaving(string request_id, string approvalMessage, out string namaEmp)
        {
            LeavingRequest leavingRequest = context.leavingRequests.Find(request_id);
            namaEmp = leavingRequest.employees.name;
            leavingRequest.approvalMessage = approvalMessage;
            leavingRequest.approvalStatus = Approval_status.Ditolak;
            leavingRequest.isRead = false;
            context.leavingRequests.Update(leavingRequest);
            return context.SaveChanges();
        }

        public int RevisiLeaving(string request_id, string approvalMessage, out string namaEmp)
        {
            LeavingRequest leavingRequest = context.leavingRequests.Find(request_id);
            namaEmp = leavingRequest.employees.name;
            leavingRequest.approvalMessage = approvalMessage;
            leavingRequest.approvalStatus = Approval_status.Revisi;
            leavingRequest.isRead = false;
            context.leavingRequests.Update(leavingRequest);
            return context.SaveChanges();
        }


        public async Task<bool> TotalSisaHariCutiApprove(LeavingRequestInserModel leavingRequestInser)
        {
            Employees emp = context.employees.Find(leavingRequestInser.employee_id);
            int totalCuti = (leavingRequestInser.endDate - leavingRequestInser.startDate.AddDays(-1)).Days - await nationalDay.CountPotonganLibur(leavingRequestInser.startDate, leavingRequestInser.endDate);
            if (totalCuti > emp.sisaCuti) return false;
            else return true;
        }

        public string GetAutoIncrementConvertString()
        {
            LeavingRequest leaving = context.leavingRequests.ToList().LastOrDefault();
            if (leaving == null) return "0000";

            string currentString = leaving.request_id.Substring(leaving.request_id.Length - 4);
            int currentNIK = Int32.Parse(currentString);
            currentNIK++;
            if (currentNIK >= 1 && currentNIK <= 9) return "000" + currentNIK.ToString();
            else if (currentNIK >= 10 && currentNIK <= 99) return "00" + currentNIK.ToString();
            else if (currentNIK >= 100 && currentNIK <= 999) return "0" + currentNIK.ToString();
            return currentNIK.ToString();


        }

       
        //untuk chart calendar manager
        public Object GetCutiEmployeeForManager(string manager_id)
        {
            return context.leavingRequests.Where(lr => lr.employees.manager_id == manager_id && lr.isDelete == false)
                .Select(lr=>new { 
                    request_id = lr.request_id,
                    startDate = lr.startDate,
                    endDate = lr.endDate,
                    nameEmployee = lr.employees.name,
                    leavingMessage = lr.leavingMessage,
                    status = lr.approvalStatus.ToString()
                }).ToList();
        }




    }
}
