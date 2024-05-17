using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayManage_System.dao
{
    internal interface ITaxService
    {
        public void CalculateTax();
        public void GetTaxById();
        public void GetTaxesForEmployee();
        public void GetTaxesForYear();

        public string GetEmployeeName(int employeeId);

        public decimal CalculateValueofTax(decimal taxableIncome);
    }
}
