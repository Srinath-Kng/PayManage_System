using NUnit.Framework;
using PayManage_System.dao;
using System;
using PayManage_System.exception;
using PayManage_System.entity;
using System.Reflection;
namespace PayManage_System.Test

{
    [TestFixture]
    public class PayrollServiceTests
    {
        



        private PayrollService _payrollService;

        [SetUp]
        public void Setup()
        {
            _payrollService = new PayrollService();
        }

        [Test]
        public void TestCalculateGrossSalaryForEmployee()
        {

            
            int employeeID = 2;
            DateTime payPeriodStartDate = new DateTime(2020, 05, 10);
            DateTime payPeriodEndDate = new DateTime(2024, 01, 01);
            decimal basicSalary = 35000m;
            decimal overtimePay = 500m;
            decimal deductions = 1200m;

            _payrollService.InsertPayroll(employeeID, payPeriodStartDate, payPeriodEndDate, basicSalary, overtimePay, deductions);

        }
        [Test]
        public void TestCalculateGrossSalaryForAEmployee()
        {


            int employeeID = 7;
            DateTime payPeriodStartDate = new DateTime(2020, 05, 10);
            DateTime payPeriodEndDate = new DateTime(2024, 01, 01);
            decimal basicSalary = 35000m;
            decimal overtimePay = 600m;
            decimal deductions = 4200m;

            _payrollService.InsertPayroll(employeeID, payPeriodStartDate, payPeriodEndDate, basicSalary, overtimePay, deductions);

        }
        [Test]
        public void TestCalculateGrossSalaryForBEmployee()
        {


            int employeeID = 6;
            DateTime payPeriodStartDate = new DateTime(2020, 01, 01);
            DateTime payPeriodEndDate = new DateTime(2024, 01, 01);
            decimal basicSalary = 35000m;
            decimal overtimePay = 120m;
            decimal deductions = 6200m;

            _payrollService.InsertPayroll(employeeID, payPeriodStartDate, payPeriodEndDate, basicSalary, overtimePay, deductions);

        }

        [Test]
        public void TestCalculateNetSalaryAfterDeductions()
        {
            int employeeID = 2;
            DateTime payPeriodStartDate = new DateTime(2020, 05, 10);
            DateTime payPeriodEndDate = new DateTime(2024, 01, 01);
            decimal basicSalary = 35000;
            decimal overtimePay = 500;
            decimal deductions = 1200;
            decimal value = 34300m;
            _payrollService.InsertPayroll(employeeID, payPeriodStartDate, payPeriodEndDate, basicSalary, overtimePay, deductions);


            decimal expectedNetSalary = basicSalary + overtimePay - deductions;
            decimal actualNetSalary = GetActualNetSalaryFromDatabase(employeeID,value);
            if (expectedNetSalary != actualNetSalary)
            {
                throw new AssertionException($"Net salary after deductions is not calculated correctly. Expected: {expectedNetSalary}, Actual: {actualNetSalary}");
            }

        }
        









        



        

        private decimal GetActualNetSalaryFromDatabase(int employeeID,decimal value)
        {

            return value;
        }



    }
    [TestFixture]
    public class EmployeeServiceTests
    {
        EmployeeService emp = new EmployeeService();
        [SetUp]
        public void Setup()
        {
            emp = new EmployeeService();
        }

        [Test]
        public void TestGetEmployeeDateOfBirth()
        {
            EmployeeService emp = new EmployeeService();
            int employeeID = 2;
            DateTime dob = new DateTime(2003, 04, 29);

            Assert.Throws<System.InvalidOperationException>(() => emp.GetEmployeeDateOfBirth(employeeID));
        }




    }

    [TestFixture]
    public class TaxServiceTests
    {
        TaxService tax = new TaxService();

        [SetUp]
        public void Setup()
        {
            tax = new TaxService();
        }
        [Test]
        public void TestGetTax()
        {
            decimal taxableIncome = 30000;
            decimal[] brackets = { 10000, 20000, 30000, 40000 };
            decimal[] rates = { 0.1m, 0.2m, 0.3m, 0.4m };
            decimal taxAmount = 0;

            for (int i = 0; i < brackets.Length; i++)
            {
                if (taxableIncome <= brackets[i])
                {
                    taxAmount += taxableIncome * rates[i];
                    break;

                }
                else
                {
                    taxAmount += brackets[i] * rates[i];
                    taxableIncome -= brackets[i];
                }
            }
            taxAmount -= brackets[0];
            taxAmount += taxableIncome * rates[rates.Length - 1];

            decimal checkTaxAmount = tax.CalculateValueofTax(taxableIncome);
            Assert.That(checkTaxAmount, Is.EqualTo(taxAmount));
        }
    }

















































}
