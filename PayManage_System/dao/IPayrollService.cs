using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayManage_System.dao
{
    internal interface IPayrollService
    {
        public void GeneratePayroll();
        public void InsertPayroll(int employeeID, DateTime payPeriodStartDate, DateTime payPeriodEndDate, decimal basicSalary, decimal overtimePay, decimal deductions);
        public bool PayrollExistsForEmployee(int employeeID);
        public bool EmployeeExists(int employeeID);
        public void GetPayrollById(int payrollId);
        public void GetPayrollsForEmployee();
        public void GetPayrollsForPeriod();
        public string GetEmployeeName(int employeeId);
        public void GetEmployeesWithNetSalaryGreaterThan(decimal netSalaryAmount);

    }
}
