using System.Globalization;
using Azure;
using PayManage_System.dao;
using PayManage_System.entity;
using PayManage_System.exception;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static PayManage_System.exception.InvalidDataException;
using PayManage_System.util;

class Program
{
    public static void Main(string[] args)
    {
        int choice = 0;
        EmployeeService employeeService = new EmployeeService();
        Employee employee = new Employee();

        while (choice != 6)
        {
            Console.WriteLine("\n\t\t\t    COURIER MANAGEMENT SYSTEM\n" +
                  "---------------------------------------------------------------------------------\n" +
                  "1.  Employee Management\n" +
                  "2.  Financial Record Management\n" +
                  "3.  Tax Management\n" +
                  "4.  Payroll Management\n" +
                  "5.  Financial Records\n" +
                  "\x1b[31m6.  Exit the Menu\x1b[0m\n" +
                  "---------------------------------------------------------------------------------");
            try
            {
                Console.WriteLine("\nEnter your choice: ");
                choice = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid numeric choice.");
                continue; 
            }

            switch (choice)
            {
                case 1:
                    {
                        try
                        {
                            Console.WriteLine("\n\t\t\t    EMPLOYEE MANAGEMENT\n" +
                                          "---------------------------------------------------------------------------------\n" +
                                          "1.  Find Employee by ID \n" +
                                          "2.  List All Employees \n" +
                                          "3.  Add New Employee \n" +
                                          "4.  Update Employee Information \n" +
                                          "5.  Calculate Employee Age \n" +
                                          "6.  Remove Employee \n" +
                                          "---------------------------------------------------------------------------------");

                            Console.WriteLine("\nEnter your choice: ");
                            int employeeChoice = Convert.ToInt32(Console.ReadLine());
                            switch (employeeChoice)
                            {
                                case 1:
                                    {
                                        Console.WriteLine("Enter the Employee ID:");
                                        int employeeIDToGet = Convert.ToInt32(Console.ReadLine());
                                        employeeService.GetEmployeeById(employeeIDToGet);
                                        break;
                                    }
                                case 2:
                                    {
                                        employeeService.GetAllEmployees();
                                        break;
                                    }
                                case 3:
                                    AddEmployee();
                                    break;
                                case 4:
                                    UpdateEmployee();
                                    break;
                                case 5:
                                    {
                                        CalculateEmployeeAge();
                                        break;
                                    }
                                case 6:
                                    {
                                        Console.WriteLine("Enter the Employee ID to remove:");
                                        int employeeIDToRemove = Convert.ToInt32(Console.ReadLine());
                                        employeeService.RemoveEmployee(employeeIDToRemove);
                                        break;
                                    }
                                default:
                                    Console.WriteLine("Invalid choice.");
                                    break;
                            }
                            
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid input. Please enter a valid numeric choice.");
                            continue;
                        }

                        break;

                    }
                case 2:
                    {
                        try
                        {
                            Console.WriteLine("\n\t\t\t    FINANCIAL RECORD MANAGEMENT\n" +
                                          "---------------------------------------------------------------------------------\n" +
                                          "1.  Add Financial Record \n" +
                                          "2.  Find Financial Record by ID \n" +
                                          "3.  List Financial Records for Employee \n" +
                                          "4.  List Financial Records for Year \n" +
                                          "---------------------------------------------------------------------------------");
                            Console.WriteLine("\nEnter your choice: ");
                            int financialRecordChoice = Convert.ToInt32(Console.ReadLine());
                            switch (financialRecordChoice)
                            {
                                case 1:
                                    {
                                        AddFinancialRecord();
                                        break;
                                    }
                                case 2:
                                    {
                                        FinancialRecordService financialRecordService = new FinancialRecordService();
                                        financialRecordService.GetFinancialRecordById();
                                        break;
                                    }
                                case 3:
                                    {
                                        FinancialRecordService financialRecordService = new FinancialRecordService();
                                        financialRecordService.GetFinancialRecordsForEmployee();
                                        break;
                                    }
                                case 4:
                                    {
                                        try
                                        {
                                            FinancialRecordService financialRecordService = new FinancialRecordService();
                                            financialRecordService.GetFinancialRecordsForDate();

                                            Console.WriteLine("Financial records retrieved successfully!");
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"An error occurred: {ex.Message}");
                                        }
                                        break;
                                    }
                                default:
                                    Console.WriteLine("Invalid choice.");
                                    break;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Invalid input. Please enter a valid numeric choice.");
                            continue;
                        }
                        
                        break;
                    }
                case 3:
                    {
                        try
                        {
                            Console.WriteLine("\n\t\t\t    TAX MANAGEMENT\n" +
                                          "---------------------------------------------------------------------------------\n" +
                                          "1. Generate Taxable Amount for Employee (Year) \n" +
                                          "2. Find Taxes by ID \n" +
                                          "3. Find Taxes by Employee \n" +
                                          "4. Find Taxes by Year \n" +
                                          "---------------------------------------------------------------------------------");
                            Console.WriteLine("\nEnter your choice: ");
                            int taxChoice = Convert.ToInt32(Console.ReadLine());
                            switch (taxChoice)
                            {
                                case 1:
                                    {
                                        try
                                        {
                                            TaxService taxServi = new TaxService();
                                            taxServi.CalculateTax();
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"An error occurred: {ex.Message}");
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        try
                                        {
                                            TaxService tax = new TaxService();
                                            tax.GetTaxById();
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"An error occurred: {ex.Message}");
                                        }
                                        break;
                                    }
                                case 3:
                                    TaxService taxService = new TaxService();
                                    taxService.GetTaxesForEmployee();
                                    break;
                                case 4:
                                    TaxService taxServic = new TaxService();
                                    taxServic.GetTaxesForYear();
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice.");
                                    break;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Invalid input. Please enter a valid numeric choice.");
                            continue;
                        }
                        
                        break;
                    }
                case 4:
                    {
                        try
                        {
                            Console.WriteLine("\n\t\t\t    PAYROLL MANAGEMENT\n" +
                                          "---------------------------------------------------------------------------------\n" +
                                          "1. Generate Payroll for Employee \n" +
                                          "2. Find Payroll by ID \n" +
                                          "3. List Payrolls for Employee \n" +
                                          "4. List Payrolls for Period \n" +
                                          "---------------------------------------------------------------------------------");
                            Console.WriteLine("\nEnter your choice: ");
                            int payrollChoice = Convert.ToInt32(Console.ReadLine());
                            switch (payrollChoice)
                            {
                                case 1:
                                    {
                                        PayrollService payrollService = new PayrollService();
                                        payrollService.GeneratePayroll();
                                        break;
                                    }
                                case 2:
                                    PayrollByID();
                                    break;
                                case 3:
                                    {
                                        try
                                        {
                                            PayrollService payrollS = new PayrollService();
                                            payrollS.GetPayrollsForEmployee();
                                        }
                                        catch (FormatException)
                                        {
                                            Console.WriteLine("Invalid input format. Please enter a valid Employee ID.");
                                        }
                                        catch (EmployeeNotFoundException ex)
                                        {
                                            Console.WriteLine($"Employee not found: {ex.Message}");
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"An error occurred: {ex.Message}");
                                        }
                                        break;
                                    }
                                case 4:
                                    {
                                        try
                                        {
                                            PayrollService payro = new PayrollService();
                                            payro.GetPayrollsForPeriod();
                                        }
                                        catch (FormatException)
                                        {
                                            Console.WriteLine("Invalid date format. Please enter dates in the format YYYY-MM-DD.");
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"An error occurred: {ex.Message}");
                                        }
                                        break;
                                    }
                                default:
                                    Console.WriteLine("Invalid choice.");
                                    break;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Invalid input. Please enter a valid numeric choice.");
                            continue;
                        }
                        
                        break;
                    }
                case 5:
                    {
                        try
                        {
                            Console.WriteLine("\n\t\t\t    FINANCIAL RECORDS\n" +
                                          "---------------------------------------------------------------------------------\n" +
                                          "1. Find Financial Records by Type \n" +
                                          "2. Generate Employee Salaries Report \n" +
                                          "---------------------------------------------------------------------------------");
                            Console.WriteLine("\nEnter your choice: ");
                            int financialRecordChoice = Convert.ToInt32(Console.ReadLine());
                            switch (financialRecordChoice)
                            {
                                case 1:
                                    {
                                        Console.WriteLine("Enter the FinancialRecord Type : ");
                                        string returnType = Console.ReadLine();
                                        FinancialRecordService financialService = new FinancialRecordService();
                                        financialService.ListFinancialRecordsByRecordType(returnType);
                                        break;
                                    }
                                case 2:
                                    {
                                        Console.WriteLine("Enter the NetSalary Amount :");
                                        int NAmount = Convert.ToInt32(Console.ReadLine());
                                        PayrollService pay = new PayrollService();
                                        pay.GetEmployeesWithNetSalaryGreaterThan(NAmount);
                                        break;
                                    }
                                default:
                                    Console.WriteLine("Invalid choice.");
                                    break;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Invalid input. Please enter a valid numeric choice.");
                            continue;
                        }
                        
                        break;
                    }
                case 6:
                    {
                        try
                        {
                            Console.WriteLine("Exiting the menu...");
                            Environment.Exit(0);
                        }
                        catch
                        {
                            Console.WriteLine("Invalid input. Please enter a valid numeric choice.");
                            continue;
                        }
                        
                        break;
                    }
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }




























































        
        static void AddEmployee()
        {
            Console.WriteLine("Enter employee details:");
            Console.Write("First Name: ");
            string firstName = Console.ReadLine();
            Console.Write("Last Name: ");
            string lastName = Console.ReadLine();
            Console.Write("Date of Birth (YYYY-MM-DD): ");
            DateTime dateOfBirth = GetValidDateTimeInput();
            Console.Write("Gender: ");
            string gender = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Phone Number: ");
            string phoneNumber = Console.ReadLine();
            Console.Write("Address: ");
            string address = Console.ReadLine();
            Console.Write("Position: ");
            string position = Console.ReadLine();
            Console.Write("Joining Date (YYYY-MM-DD): ");
            DateTime joiningDate = GetValidDateTimeInput();
            Console.Write("Termination Date (YYYY-MM-DD): ");
            DateTime? terminationDate = GetNullableDateTimeInput();

            EmployeeService employeeService1 = new EmployeeService();
            employeeService1.AddEmployee(firstName, lastName, dateOfBirth, gender, email, phoneNumber, address, position, joiningDate, terminationDate);
        }

        static DateTime GetValidDateTimeInput()
        {
            DateTime inputDate;
            while (!DateTime.TryParse(Console.ReadLine(), out inputDate) || inputDate > DateTime.Now)
            {
                Console.WriteLine("Invalid date or date in the future. Please enter a valid date (YYYY-MM-DD):");
            }
            return inputDate;
        }

        static DateTime? GetNullableDateTimeInput()
        {
            DateTime inputDate;
            while (!DateTime.TryParse(Console.ReadLine(), out inputDate) || inputDate > DateTime.Now)
            {
                Console.WriteLine("Invalid date or date in the future. Please enter a valid date (YYYY-MM-DD), or leave blank:");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    return null;
                }
            }
            return inputDate;
        }
        static void UpdateEmployee()
        {
            Console.WriteLine("Enter the Employee ID to update:");
            int employeeID = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the attribute to update:");
            string attribute = Console.ReadLine();

            Console.WriteLine($"Enter the updated value for {attribute}:");
            string updatedValue = Console.ReadLine();
            EmployeeService employeeService1 = new EmployeeService();
            employeeService1.UpdateEmployee(employeeID, attribute, updatedValue);
        }
        
        static void CalculateEmployeeAge()
        {
            Console.Write("Enter EmployeeID: ");
            int employeeId = int.Parse(Console.ReadLine());

            string constr = "Server=LAGLOP\\SQLEXPRESS;Database=CASE_STUDY;Trusted_Connection=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            try
            {
                Employee employee1 = new Employee();
                string sql = "SELECT DateOfBirth FROM Employee WHERE EmployeeID = @EmployeeID";
                SqlCommand command = new SqlCommand(sql, con);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    DateTime dateOfBirth = (DateTime)result;
                    employee1.CalculateAge(dateOfBirth);
                }
                else
                {
                    Console.WriteLine($"Employee with ID {employeeId} not found.");
                }
            }
            catch (EmployeeNotFoundException ex)
            {
                Console.WriteLine($"Employee not found: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        static void AddFinancialRecord()
        {
            Console.WriteLine("Enter Financial Record details:");
            Console.Write("Employee ID: ");
            int employeeID = Convert.ToInt32(Console.ReadLine());
            Console.Write("Record Date (YYYY-MM-DD): ");
            DateTime recordDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Description: ");
            string description = Console.ReadLine();
            Console.Write("Amount: ");
            decimal amount = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Record Type: ");
            string recordType = Console.ReadLine();
            FinancialRecordService financialRecordService = new FinancialRecordService();
            financialRecordService.AddFinancialRecord(employeeID, description, amount, recordType, recordDate);
        }
        static void PayrollByID()
        {
            try
            {
                Console.Write("Enter Payroll ID: ");
                int payrollId = Convert.ToInt32(Console.ReadLine());

                PayrollService payrollServic = new PayrollService();
                payrollServic.GetPayrollById(payrollId);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid Payroll ID.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }






        string constr = "Server=LAGLOP\\SQLEXPRESS;Database=CASE_STUDY;Trusted_Connection=True;TrustServerCertificate=True;";
    }

}