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

        public LeavingRequestRepository(MyContext context)
        {
            this.context = context;
        }

        public Object GetLeavingEmployee(Employees employee)
        {
            return context.leavingRequests.Where(lr => lr.employees.employee_id == employee.employee_id)
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

        public Object GetLeavingEmployee(string request_id)
        {
            return context.leavingRequests
                  .Select(lr => new LeavingRequest
                  {
                      request_id = lr.request_id,
                      employee_id = lr.employee_id,
                      approvalStatus = lr.approvalStatus,
                      requestTime = lr.requestTime,
                      startDate = lr.startDate,
                      endDate = lr.endDate,
                      approvalMessage = lr.approvalMessage,
                      fileBukti = lr.fileBukti,
                      tipeFileBukti = lr.tipeFileBukti
                  }).FirstOrDefault(lr => lr.request_id == request_id);
        }

        public Object GetLeavingManager(Employees employee)
        {
            return context.leavingRequests.Where(lr => lr.employees.manager_id == employee.employee_id)
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


        public int InsertLeaving(LeavingRequestInserModel leavingRequestInser)
        {
            if (!TotalSisaHariCutiApprove(leavingRequestInser)) return Variables.CUTI_SUDAH_HABIS;
            LeavingRequest leavingRequest = new LeavingRequest()
            {
                request_id = "Leave" + GetAutoIncrementConvertString(),
                employee_id = leavingRequestInser.employee_id,
                category_id = leavingRequestInser.category_id,
                approvalStatus = Approval_status.Menunggu,
                requestTime = leavingRequestInser.requestTime,
                startDate = leavingRequestInser.startDate,
                endDate = leavingRequestInser.endDate,
                leavingMessage = leavingRequestInser.leavingMessage,
                fileBukti = leavingRequestInser.fileBukti,
                tipeFileBukti = (TipeFileBukti)Enum.Parse(typeof(TipeFileBukti), leavingRequestInser.tipeFileBukti),
                approvalMessage = "Menunggu"
            };
            context.leavingRequests.Add(leavingRequest);
            return context.SaveChanges();
        }

        public int ApproveLeaving(string request_id,string approvalMessage, out string namaEmp)
        {
           
            LeavingRequest leavingRequest = context.leavingRequests.Find(request_id);
            Employees managerDetail = context.employees.Find(leavingRequest.employees.manager_id);
            namaEmp = leavingRequest.employees.name;

            leavingRequest.approvalMessage = approvalMessage;
            leavingRequest.approvalStatus = Approval_status.Diterima;
            leavingRequest.employees.sisaCuti = leavingRequest.employees.sisaCuti-(leavingRequest.endDate - leavingRequest.startDate).Days;
            context.leavingRequests.Update(leavingRequest);
            EmailServices.SendEmail(leavingRequest.employees.email, "Perihal Cuti", HtmlTemplate.RequestLeaving(managerDetail.name, leavingRequest.employees.name, leavingRequest.startDate.ToString("D"), leavingRequest.endDate.ToString("D")));
            return context.SaveChanges();
            
        }

        public int RejectLeaving(string request_id, string approvalMessage, out string namaEmp)
        {
            LeavingRequest leavingRequest = context.leavingRequests.Find(request_id);
            namaEmp = leavingRequest.employees.name;
            leavingRequest.approvalMessage = approvalMessage;
            leavingRequest.approvalStatus = Approval_status.Ditolak;
            context.leavingRequests.Update(leavingRequest);
            return context.SaveChanges();
        }

        public int RevisiLeaving(string request_id, string approvalMessage, out string namaEmp)
        {
            LeavingRequest leavingRequest = context.leavingRequests.Find(request_id);
            namaEmp = leavingRequest.employees.name;
            leavingRequest.approvalMessage = approvalMessage;
            leavingRequest.approvalStatus = Approval_status.Revisi;
            context.leavingRequests.Update(leavingRequest);
            return context.SaveChanges();
        }





        public bool TotalSisaHariCutiApprove(LeavingRequestInserModel leavingRequestInser)
        {
            Employees emp = context.employees.Find(leavingRequestInser.employee_id);
            int totalCuti = (leavingRequestInser.endDate - leavingRequestInser.startDate).Days;
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

       

        




    }
}
