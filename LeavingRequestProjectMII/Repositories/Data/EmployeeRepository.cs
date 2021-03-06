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

        
        public Object GetTotCutiSaya(string employee_id)
        {
            return context.employees.Select(emp => new
            {
                sisaCuti = emp.sisaCuti,
                employee_id = emp.employee_id
            }).First(emp => emp.employee_id == employee_id);
        }
        public Object Get()
        {
            return context.employees.Where(emp=>emp.isDeleted==false).Select(emp => new  EmployeeViewModel{ 
                employee_id = emp.employee_id,
                name = emp.name,
                gender = emp.gender.ToString(),
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

        public Object Get(string employee_id)
        {
            return context.employees.Where(emp=>emp.isDeleted==false).Select(emp => new  Employees{ 
                employee_id = emp.employee_id,
                name = emp.name,
                email = emp.email,
                phoneNumber = emp.phoneNumber,
                sisaCuti = emp.sisaCuti,
                role_Id = emp.role_Id,
                divisi_id = emp.divisi_id,
                manager_id = emp.manager_id,
                gender = emp.gender,
                
            }).ToList().FirstOrDefault(emp=>emp.employee_id == employee_id);
        }

        public Object GetEmployeeWithTotalCuti(string manager_id)
        {
            return context.employees.Where(emp => emp.manager_id == manager_id && emp.isDeleted == false)
                .Select(emp => new
                {
                    name = emp.name,
                    sisaCuti = emp.sisaCuti,
                    namaDivisi = emp.divisi.namaDivisi,
                    namaRole = emp.role.roleName,
                    employee_id = emp.employee_id,
                }).ToList();
        }

        public Object GetWithManagerName(string employee_id)
        {
            
            var emp =  context.employees.Where(emp => emp.isDeleted == false).ToList().FirstOrDefault(emp => emp.employee_id == employee_id);

            var manager = context.employees.Find(emp.manager_id);

            return new
            {
                employee_id = emp.employee_id,
                name = emp.name,
                email = emp.email,
                phoneNumber = emp.phoneNumber,
                sisaCuti = emp.sisaCuti,
                role_Id = emp.role_Id,
                divisi_id = emp.divisi_id,
                manager_id = emp.manager_id,
                gender = emp.gender,
                manager = new
                {
                    managerName = manager.name,
                    employee_id = manager.employee_id

                },
                divisi = new {
                    namaDivisi = emp.divisi.namaDivisi
                }
                
            };
        }

        public int softDelete(string employeeId)
        {
            Employees employee = context.employees.Find(employeeId);
            employee.isDeleted = true;

            context.employees.Update(employee);
            return context.SaveChanges();
        }

        public Object GetAllManager()
        {
            return context.employees.Where(emp => emp.role_Id == 2).Select(emp => new Employees { 
                employee_id = emp.employee_id,
                name = emp.name,
                role_Id = emp.role_Id
                
            });
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
                resettahunCuti= DateTime.Now.Year,
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
                
                ResetJatahCuti(empChk.employee_id);//reset jatah cuti on 1 jan
                empReturn = empChk;
                return Variables.SUCCESS;
            }
            
        }

        public int Update(EmployeeUpdateModel employeeUpdate)
        {
            Employees emp = context.employees.Find(employeeUpdate.employee_id);
            emp.name = employeeUpdate.name;
            emp.gender = (Gender)Enum.Parse(typeof(Gender), employeeUpdate.gender);
            emp.email = employeeUpdate.email;
            emp.phoneNumber = employeeUpdate.phoneNumber;
            emp.role_Id = employeeUpdate.role_Id;
            emp.manager_id = employeeUpdate.manager_id;
            emp.divisi_id = employeeUpdate.divisi_id;

            if (EmailIsUsed_Update(emp)) return Variables.EMAIL_DUPLICATE;
            else if (PhoneIsUsed_Update(emp)) return Variables.NO_TELP_DUPLICATE;
            
            context.employees.Update(emp);
            if (context.SaveChanges() > 0) return Variables.SUCCESS;
            else return Variables.FAIL;
        }

        public bool EmailIsUsed_Update(Employees employees)
        {
            var empCheck = context.employees.Where(emp => emp.employee_id != employees.employee_id).FirstOrDefault(emp=>emp.email == employees.email);
            return empCheck != null;
        }

        public bool PhoneIsUsed_Update(Employees employees)
        {
            var empCheck = context.employees.Where(emp => emp.employee_id != employees.employee_id).FirstOrDefault(emp => emp.phoneNumber == employees.phoneNumber);
            return empCheck != null;
        }

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

        public void ResetJatahCuti(string employee_id)
        {
            Employees emp = context.employees.Find(employee_id);
            if(DateTime.Now.Year != emp.resettahunCuti)
            {
                emp.resettahunCuti = DateTime.Now.Year;
                emp.sisaCuti = 12;
                context.employees.Update(emp);
                context.SaveChanges();
            }
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


        public int ForgotPassword(Employees employees)
        {
            Employees emp = context.employees.Where(emp => emp.email == employees.email).FirstOrDefault();
            if (emp == null) return Variables.DATA_TIDAK_SESUAI;
            emp.tokenOtp = Tools.RandomString(10);
            emp.expired = DateTime.Now.AddMinutes(5);
            emp.isActiveOtp = true;
            context.employees.Update(emp);
            int chk = context.SaveChanges();

            if (chk <= 0) return Variables.FAIL;
            else
            {
                EmailServices.SendEmail(employees.email, "Forgot Password", HtmlTemplate.ForgotPasswordTemplate(emp.tokenOtp, emp.expired.ToString("t"), emp.email));
                return Variables.SUCCESS;
            }

        }

        public int ValidateForgotPassword(Employees empVal)
        {
            Employees emp = context.employees.FirstOrDefault(emp => emp.tokenOtp == empVal.tokenOtp && emp.email == empVal.email);

            if (emp == null) return Variables.DATA_TIDAK_SESUAI;
            else if (DateTime.Now > emp.expired) {
                emp.isActiveOtp = false;
                context.employees.Update(emp);
                context.SaveChanges();
                return Variables.FORGET_TOKEN_EXPIRED;
            } 
           
            emp.password = Tools.BCryptHasing(empVal.password);

            context.employees.Update(emp);
            int chk = context.SaveChanges();

            if (chk > 0) return Variables.SUCCESS;
            else return Variables.FAIL;
        }

    }
}
