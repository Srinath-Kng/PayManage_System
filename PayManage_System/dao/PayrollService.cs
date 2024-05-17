using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using PayManage_System.entity;
using PayManage_System.util;
using static PayManage_System.exception.InvalidDataException;

namespace PayManage_System.dao
{
    public class PayrollService : IPayrollService
    {
        
        public void GeneratePayroll()
        {
            try
            {
                Console.WriteLine("Enter Employee ID to generate payroll:");
                int employeeID = Convert.ToInt32(Console.ReadLine());

                if (!EmployeeExists(employeeID))
                {
                    Console.WriteLine($"Employee not available for the given ID: {employeeID}");
                    return;
                }

                if (PayrollExistsForEmployee(employeeID))
                {
                    Console.WriteLine($"Employee already has payroll generated.");
                    return;
                }

                Console.WriteLine("Enter Pay Period Start Date (yyyy-mm-dd):");
                DateTime payPeriodStartDate = Convert.ToDateTime(Console.ReadLine());

                Console.WriteLine("Enter Pay Period End Date (yyyy-mm-dd):");
                DateTime payPeriodEndDate = Convert.ToDateTime(Console.ReadLine());

                Console.WriteLine("Enter Basic Salary:");
                decimal basicSalary = Convert.ToDecimal(Console.ReadLine());

                Console.WriteLine("Enter Overtime Pay:");
                decimal overtimePay = Convert.ToDecimal(Console.ReadLine());

                Console.WriteLine("Enter Deductions:");
                decimal deductions = Convert.ToDecimal(Console.ReadLine());

      
                InsertPayroll(employeeID, payPeriodStartDate, payPeriodEndDate, basicSalary, overtimePay, deductions);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter valid input.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void InsertPayroll(int employeeID, DateTime payPeriodStartDate, DateTime payPeriodEndDate, decimal basicSalary, decimal overtimePay, decimal deductions)
        {
           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                if (basicSalary < 0 || overtimePay < 0 || deductions < 0)
                {
                    throw new PayrollGenerationException("Invalid payroll data. Please ensure all values are positive.");
                }
                string query = @"INSERT INTO Payroll (EmployeeID, PayPeriodStartDate, PayPeriodEndDate, BasicSalary, OvertimePay, Deductions, NetSalary)
                        VALUES (@EmployeeID, @PayPeriodStartDate, @PayPeriodEndDate, @BasicSalary, @OvertimePay, @Deductions, @NetSalary)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@PayPeriodStartDate", payPeriodStartDate);
                command.Parameters.AddWithValue("@PayPeriodEndDate", payPeriodEndDate);
                command.Parameters.AddWithValue("@BasicSalary", basicSalary);
                command.Parameters.AddWithValue("@OvertimePay", overtimePay);
                command.Parameters.AddWithValue("@Deductions", deductions);

                decimal netSalary = ((basicSalary + overtimePay)*12-deductions);
                command.Parameters.AddWithValue("@NetSalary", netSalary);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Payroll generated successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to generate payroll.");
                }
            }

        }

        public bool PayrollExistsForEmployee(int employeeID)
        {
           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT TOP 1 1 FROM Payroll WHERE EmployeeID = @EmployeeID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                return command.ExecuteScalar() != null;
            }
        }

        public bool EmployeeExists(int employeeID)
        {
        
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT TOP 1 1 FROM Employee WHERE EmployeeID = @EmployeeID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                return command.ExecuteScalar() != null;
            }
        }


        

        public void GetPayrollById(int payrollId)
        {
            SqlConnection conn = null;
            try
            {
                
               
                
                conn = new SqlConnection(connectionString);
                conn.Open();

                
                string query = $"SELECT DISTINCT * FROM Payroll WHERE PayrollID = {payrollId}";

                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    throw new PayrollGenerationException($"No payroll record found with ID: {payrollId}");
                }
                else
                {
                    while (reader.Read())
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Payroll ID: {reader.GetInt32(0)}");
                        Console.WriteLine($"Employee ID: {reader.GetInt32(1)}");
                        Console.WriteLine($"Pay Period Start Date: {reader.GetDateTime(2)}");
                        Console.WriteLine($"Pay Period End Date: {reader.GetDateTime(3)}");
                        Console.WriteLine($"Basic Salary: Rs {reader.GetInt32(4)}");
                        Console.WriteLine($"Overtime Pay: Rs {reader.GetInt32(5)}");
                        Console.WriteLine($"Deductions: Rs {reader.GetInt32(6)}");
                        Console.WriteLine($"Net Salary: Rs {reader.GetInt32(7)}");
                        Console.WriteLine();
                    }
                    Console.WriteLine("Data Retrived Successfully");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        
        public string GetEmployeeName(int employeeId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = $"SELECT DISTINCT FirstName, LastName FROM Employee WHERE EmployeeID = {employeeId}";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string firstName = reader["FirstName"].ToString();
                    string lastName = reader["LastName"].ToString();
                    return $"{firstName} {lastName}";
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public void GetPayrollsForEmployee()
        {
            try
            {
                Console.Write("Enter Employee ID: ");
                int employeeId = Convert.ToInt32(Console.ReadLine());

                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT DISTINCT * FROM Payroll WHERE EmployeeID = @EmployeeID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@EmployeeID", employeeId);
                    string employeeName = GetEmployeeName(employeeId);
                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        Console.WriteLine($"No payroll records found for employee with ID: {employeeId}");
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            int payrollId = reader.GetInt32(reader.GetOrdinal("PayrollID"));
                            DateTime startDate = reader.GetDateTime(reader.GetOrdinal("PayPeriodStartDate"));
                            DateTime endDate = reader.GetDateTime(reader.GetOrdinal("PayPeriodEndDate"));
                            int basicSalary = reader.GetInt32(reader.GetOrdinal("BasicSalary"));
                            int overtimePay = reader.GetInt32(reader.GetOrdinal("OvertimePay"));
                            int deductions = reader.GetInt32(reader.GetOrdinal("Deductions"));
                            int netSalary = reader.GetInt32(reader.GetOrdinal("NetSalary"));
                            Console.WriteLine();
                            Console.WriteLine($"Payroll ID: {payrollId}");
                            Console.WriteLine($"Employee ID: {employeeId}");
                            Console.WriteLine($"Employee Name: {employeeName}");
                            Console.WriteLine($"Pay Period Start Date: {startDate}");
                            Console.WriteLine($"Pay Period End Date: {endDate}");
                            Console.WriteLine($"Basic Salary: Rs {basicSalary}");
                            Console.WriteLine($"Overtime Pay: Rs {overtimePay}");
                            Console.WriteLine($"Deductions: Rs {deductions}");
                            Console.WriteLine($"Net Salary: Rs {netSalary}");
                            Console.WriteLine();
                        }
                        Console.WriteLine("Data Retrived Successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public void GetPayrollsForPeriod()
        {
            try
            {
                Console.WriteLine("Enter the start date (YYYY-MM-DD): ");
                DateTime startDate = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("Enter the end date (YYYY-MM-DD): ");
                DateTime endDate = DateTime.Parse(Console.ReadLine());

                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Payroll WHERE PayPeriodStartDate >= @StartDate AND PayPeriodEndDate <= @EndDate";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        Console.WriteLine($"No payroll records found for the period from {startDate.ToShortDateString()} to {endDate.ToShortDateString()}");
                        return;
                    }

                    while (reader.Read())
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Payroll ID: {reader.GetInt32(0)}");
                        Console.WriteLine($"Employee ID: {reader.GetInt32(1)}");
                        Console.WriteLine($"Pay Period Start Date: {reader.GetDateTime(2)}");
                        Console.WriteLine($"Pay Period End Date: {reader.GetDateTime(3)}");
                        Console.WriteLine($"Basic Salary: Rs {reader.GetInt32(4)}");
                        Console.WriteLine($"Overtime Pay: Rs {reader.GetInt32(5)}");
                        Console.WriteLine($"Deductions: Rs {reader.GetInt32(6)}");
                        Console.WriteLine($"Net Salary: Rs {reader.GetInt32(7)}");
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public void GetEmployeesWithNetSalaryGreaterThan(decimal netSalaryAmount)
        {
            try
            {
               
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT DISTINCT E.EmployeeID, E.FirstName, E.LastName, P.NetSalary " +
                                   "FROM Payroll P " +
                                   "INNER JOIN Employee E ON P.EmployeeID = E.EmployeeID " +
                                   "WHERE P.NetSalary > @NetSalaryAmount";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NetSalaryAmount", netSalaryAmount);

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        Console.WriteLine($"No employees found with a net salary greater than {netSalaryAmount}.");
                    }
                    else
                    {
                        Console.WriteLine("Employees with net salary greater than the specified amount:");
                        Console.WriteLine();
                        while (reader.Read())
                        {
                            int employeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID"));
                            string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                            string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                            decimal netSalary = reader.GetInt32(reader.GetOrdinal("NetSalary"));


                            Console.WriteLine($"Employee ID: {employeeID}, Name: {firstName} {lastName}, Net Salary: Rs {netSalary}");
                            Console.WriteLine();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }








































       
        string connectionString = "Server=LAGLOP\\SQLEXPRESS;Database=CASE_STUDY;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}
