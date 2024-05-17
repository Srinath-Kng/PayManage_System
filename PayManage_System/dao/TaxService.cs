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
    public class TaxService : ITaxService
    {
        
        
        public void CalculateTax()
        {
            decimal CalculateTax(decimal taxableIncome)
            {
                decimal[] brackets = { 10000, 20000, 30000, 40000 };
                decimal[] rates = { 0.1m, 0.2m, 0.3m, 0.4m };
                decimal taxAmount = 0;

                for (int i = 0; i < brackets.Length; i++)
                {
                    if (taxableIncome <= brackets[i])
                    {
                        taxAmount += taxableIncome * rates[i];
                        return taxAmount;
                    }
                    else
                    {
                        taxAmount += brackets[i] * rates[i];
                        taxableIncome -= brackets[i];
                    }
                }

                taxAmount += taxableIncome * rates[rates.Length - 1];

                return taxAmount;
            }

            try
            {
                Console.WriteLine("Enter Employee ID:");
                int employeeId = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter Tax Year:");
                int taxYear = Convert.ToInt32(Console.ReadLine());
                if (taxYear < 0 || taxYear > DateTime.Now.Year)
                {
                    throw new ArgumentException("Tax year is invalid. Please enter a valid tax year.");
                }
                if (employeeId <= 0)
                {
                    throw new InvalidInputException("Invalid employee ID. Please enter a positive integer.");
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = $"SELECT TaxableIncome FROM Tax WHERE EmployeeID = {employeeId} AND TaxYear = {taxYear}";
                    SqlCommand command = new SqlCommand(query, conn);
                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        throw new Exception("Could not find any tax details for the given data!");
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            decimal taxableIncome = Convert.ToDecimal(reader["TaxableIncome"]);

                            decimal taxAmount = CalculateTax(taxableIncome);

                            Console.WriteLine();
                            Console.WriteLine($"TaxAmount for employee with ID {employeeId} for year {taxYear} is Rs.{taxAmount}");

                            string employeeName = GetEmployeeName(employeeId);
                            if (!string.IsNullOrEmpty(employeeName))
                            {
                                Console.WriteLine($"Employee Name: {employeeName}");
                            }
                        }
                        Console.WriteLine();
                        Console.WriteLine("Data Retrieved Successfully");
                    }
                }
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Invalid input error: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }





        public string GetEmployeeName(int employeeId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = $"SELECT FirstName, LastName FROM Employee WHERE EmployeeID = {employeeId}";
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
        public void GetTaxById()
        {
            try
            {
                Console.WriteLine("Enter the Tax ID:");
                int taxID = Convert.ToInt32(Console.ReadLine());

                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Tax.*, Employee.FirstName, Employee.LastName FROM Tax INNER JOIN Employee ON Tax.EmployeeID = Employee.EmployeeID WHERE Tax.TaxID = @TaxID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TaxID", taxID);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Tax ID: {reader["TaxID"]}");
                        Console.WriteLine($"Employee Name: {reader["FirstName"]} {reader["LastName"]}");
                        Console.WriteLine($"Tax Year: {reader["TaxYear"]}");
                        Console.WriteLine($"Taxable Income: {reader["TaxableIncome"]}");
                        Console.WriteLine($"Tax Amount: {reader["TaxAmount"]}");
                    }
                    else
                    {
                        Console.WriteLine("No tax record found with the provided Tax ID.");
                    }
                }
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

        public void GetTaxesForEmployee()
        {
            try
            {
                Console.WriteLine("Enter the Employee ID:");
                int employeeID = Convert.ToInt32(Console.ReadLine());
                if (employeeID <= 0)
                {
                    throw new InvalidInputException("Invalid employee ID. Please enter a positive integer.");
                }
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Tax WHERE EmployeeID = @EmployeeID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@EmployeeID", employeeID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine($"No tax records found for employee with ID: {employeeID}");
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"Tax ID: {reader["TaxID"]}");
                                Console.WriteLine($"Employee ID: {reader["EmployeeID"]}");
                                Console.WriteLine($"Tax Year: {reader["TaxYear"]}");
                                Console.WriteLine($"Taxable Income: {reader["TaxableIncome"]}");
                                Console.WriteLine($"Tax Amount: {reader["TaxAmount"]}");

                                string employeeName = GetEmployeeName(employeeID);
                                if (!string.IsNullOrEmpty(employeeName))
                                {
                                    Console.WriteLine($"Employee Name: {employeeName}");
                                }

                                Console.WriteLine();
                            }
                            Console.WriteLine("Data Retrived Successfully");
                        }
                    }
                }
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Invalid input error: {ex.Message}");
            }
            catch (EmployeeNotFoundException ex)
            {
                Console.WriteLine($"Employee not found: {ex.Message}");
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

        public void GetTaxesForYear()
        {
            try
            {
                Console.WriteLine("Enter the Tax Year:");
                int taxYear = Convert.ToInt32(Console.ReadLine());

               
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Tax WHERE TaxYear = @TaxYear";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TaxYear", taxYear);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine($"No tax records found for the year {taxYear}");
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"Tax ID: {reader["TaxID"]}");
                                Console.WriteLine($"Employee ID: {reader["EmployeeID"]}");
                                Console.WriteLine($"Tax Year: {reader["TaxYear"]}");
                                Console.WriteLine($"Taxable Income: {reader["TaxableIncome"]}");
                                Console.WriteLine($"Tax Amount: {reader["TaxAmount"]}");
                                int employeeID = Convert.ToInt32(reader["EmployeeID"]);
                                string employeeName = GetEmployeeName(employeeID);
                                if (!string.IsNullOrEmpty(employeeName))
                                {
                                    Console.WriteLine($"Employee Name: {employeeName}");
                                }

                                Console.WriteLine();
                            }
                            Console.WriteLine("Data Retrived Successfully");
                        }
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid tax year.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public decimal CalculateValueofTax(decimal taxableIncome)
        {
            decimal[] brackets = { 10000, 20000, 30000, 40000 };
            decimal[] rates = { 0.1m, 0.2m, 0.3m, 0.4m };
            decimal taxAmount = 0;

            for (int i = 0; i < brackets.Length; i++)
            {
                if (taxableIncome <= brackets[i])
                {
                    taxAmount += taxableIncome * rates[i];
                    return taxAmount;
                }
                else
                {
                    taxAmount += brackets[i] * rates[i];
                    taxableIncome -= brackets[i];
                }
            }

            taxAmount += taxableIncome * rates[rates.Length - 1];

            return taxAmount;

        }
































        string connectionString = "Server=LAGLOP\\SQLEXPRESS;Database=CASE_STUDY;Trusted_Connection=True;TrustServerCertificate=True;";
       

    }
}
