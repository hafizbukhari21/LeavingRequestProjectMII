using API.Context;
using API.Models;
using API.ModelsView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                      approvalMessage = lr.approvalMessage
                  }).ToList();
        }
    }
}
