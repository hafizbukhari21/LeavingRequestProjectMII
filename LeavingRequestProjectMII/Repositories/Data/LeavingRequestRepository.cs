﻿using API.Context;
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
                      tipeFileBukti = lr.tipeFileBukti,
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

        public int InsertLeaving(LeavingRequestInserModel leavingRequestInser)
        {
            var data = DateTime.Now;
            if (!TotalSisaHariCutiApprove(leavingRequestInser)) return Variables.CUTI_SUDAH_HABIS;
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
                approvalMessage = "Menunggu"
            };
            context.leavingRequests.Add(leavingRequest);
            return context.SaveChanges();
        }

        public int UpdateLeaving(LeavingRequestInserModel leaving)
        {
            LeavingRequest lrUpdate = context.leavingRequests.Find(leaving.request_id);
            lrUpdate.leavingMessage = leaving.leavingMessage;
            lrUpdate.category_id = leaving.category_id;
            lrUpdate.startDate = leaving.startDate;
            lrUpdate.endDate = leaving.endDate;

            if(leaving.fileBukti!=null )
            {
                lrUpdate.fileBukti = leaving.fileBukti;
                lrUpdate.namaFileBukti = leaving.namaFileBukti;
                lrUpdate.tipeFileBukti = (TipeFileBukti)Enum.Parse(typeof(TipeFileBukti), leaving.tipeFileBukti);
            }

            context.leavingRequests.Update(lrUpdate);
            return context.SaveChanges();
        }

        public int CountNotReadRequest(string employee_id)
        {
            return context.leavingRequests.Where(lr => lr.employee_id == employee_id && lr.isRead == false).Count();
        }

        public int ApproveLeaving(string request_id,string approvalMessage, out string namaEmp)
        {
           
            LeavingRequest leavingRequest = context.leavingRequests.Find(request_id);
            Employees managerDetail = context.employees.Find(leavingRequest.employees.manager_id);
            namaEmp = leavingRequest.employees.name;


            if (!TotalSisaHariCutiApprove(new LeavingRequestInserModel {
                employee_id = leavingRequest.employee_id,
                startDate = leavingRequest.startDate,
                endDate = leavingRequest.endDate
            })) return Variables.JUMLAH_CUTI_TIDAK_MENCUKUPI;


            leavingRequest.approvalMessage = approvalMessage;
            leavingRequest.approvalStatus = Approval_status.Diterima;
            leavingRequest.isRead = false;
            leavingRequest.employees.sisaCuti = leavingRequest.employees.sisaCuti-(leavingRequest.endDate - leavingRequest.startDate).Days;
            context.leavingRequests.Update(leavingRequest);
            EmailServices.SendEmail(leavingRequest.employees.email, "Perihal Cuti", HtmlTemplate.RequestLeaving(managerDetail.name, leavingRequest.employees.name, leavingRequest.startDate.ToString("D"), leavingRequest.endDate.ToString("D")));
            int chk = context.SaveChanges();

            if (chk > 0) return Variables.SUCCESS;
            else return Variables.FAIL;
            
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
