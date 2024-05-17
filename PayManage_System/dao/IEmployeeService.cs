using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayManage_System.dao
{
    internal interface IEmployeeService
    {
        public void GetEmployeeById(int employeeID);
        public void GetAllEmployees();
        public void AddEmployee(string firstName, string lastName, DateTime dateOfBirth, string gender, string email, string phoneNumber, string address, string position, DateTime joiningDate, DateTime? terminationDate);
        public void UpdateEmployee(int employeeID, string attribute, string updatedValue);
        public void RemoveEmployee(int employeeID);
        public DateTime GetEmployeeDateOfBirth(int employeeID);

    }
}
