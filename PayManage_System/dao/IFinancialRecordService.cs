using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayManage_System.dao
{
    internal interface IFinancialRecordService
    {

        public void AddFinancialRecord(int employeeID, string description, decimal amount, string recordType, DateTime recordDate);
        void GetFinancialRecordById();
        void GetFinancialRecordsForEmployee();
        void GetFinancialRecordsForDate();
        public void ListFinancialRecordsByRecordType(string recordType);

    }
}
