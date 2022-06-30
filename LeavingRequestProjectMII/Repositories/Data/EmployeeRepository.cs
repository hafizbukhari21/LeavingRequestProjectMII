using API.Context;
using API.Models;
using API.ModelsInsert;
using API.ModelsView;
using API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employees, string>
    {
        private readonly MyContext context;

        public EmployeeRepository(MyContext context) :base(context)
        {
            this.context = context;
        }

        public Object Get()
        {
            return context.employees.Where(emp=>emp.isDeleted==false).Select(emp => new  EmployeeViewModel{ 
                employee_id = emp.employee_id,
                name = emp.name,
                Email = emp.email,
                phoneNumber = emp.phoneNumber,
                sisaCuti = emp.sisaCuti,
                roleName = emp.role.roleName,
                namaDivisi = emp.divisi.namaDivisi,
                cuti = emp.leavingRequests.Select(cuti => new Cuti {
                    approvalMessage = cuti.approvalMessage
                }).ToList()


            }).ToList();
        }

        public int Insert(EmployeeInsertModel employeeInsertModel)
        {
            
            Employees emp = new Employees
            {
                employee_id = "Employee"+GetAutoIncrementConvertString(),
                name = employeeInsertModel.name,
                email = employeeInsertModel.email,
                password = Tools.BCryptHasing(employeeInsertModel.password),
                phoneNumber = employeeInsertModel.phoneNumber,
                gender = (Gender)Enum.Parse(typeof(Gender), employeeInsertModel.gender),
                sisaCuti = 12,
                role_Id = employeeInsertModel.role_id,
                manager_id = employeeInsertModel.manager,
                divisi_id = employeeInsertModel.divisi_id,
                isDeleted = false,

            };
            context.Add(emp);
            return context.SaveChanges();
        }


        public int Login(EmployeeLoginModel employeeLogin, out Employees empReturn)
        {
            Employees empChk = context.employees.FirstOrDefault(emp => emp.email == employeeLogin.email);

            empReturn = null;
            if (empChk == null) return Variables.WRONG_EMAIL;
            else if (empChk.email != employeeLogin.email) return Variables.WRONG_EMAIL;
            else if(!BCrypt.Net.BCrypt.Verify(employeeLogin.password, empChk.password)){
                return Variables.WRONG_PASSWORD;
            }
            else
            {
                empReturn = empChk;
                return Variables.SUCCESS;
            }
            
        }

        //public int Update (EmployeeUpdateModel employeeUpdate) {
        //    Employees emp = context.employees.Find(employeeUpdate.employee_id);
        //    emp.name = employeeUpdate.name;
        //    emp.gender = (Gender)Enum.Parse(typeof(Gender), employeeUpdate.gender);

        //}

       
        public bool EmailIsUsed(string Email)
        {
            Employees emp = context.employees.FirstOrDefault(emp => emp.email == Email);
            return emp != null;
        }

        public bool PhoneIsUsed(string phoneNumber)
        {
            Employees emp = context.employees.FirstOrDefault(emp => emp.phoneNumber == phoneNumber);
            return emp != null;
        }

        public string GetAutoIncrementConvertString()
        {
            Employees emp = context.employees.ToList().LastOrDefault();
            if (emp == null) return "0000";

            string currentString = emp.employee_id.Substring(emp.employee_id.Length - 4);
            int currentNIK = Int32.Parse(currentString);

            currentNIK++;
            if (currentNIK >= 1 && currentNIK <= 9) return "000" + currentNIK.ToString();
            else if (currentNIK >= 10 && currentNIK <= 99) return "00" + currentNIK.ToString();
            else if (currentNIK >= 100 && currentNIK <= 999) return "0" + currentNIK.ToString();
            return currentNIK.ToString();



        }

        
    }
}
