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
    internal class FinancialRecordService : IFinancialRecordService 
    {
        
    
        public void AddFinancialRecord(int employeeID, string description, decimal amount, string recordType, DateTime recordDate)
        {
          
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            try
            {
                if (amount <= 0)
                {
                    throw new FinancialRecordException("Amount must be a positive value.");
                }
                string query = @"INSERT INTO FinancialRecord (EmployeeID, RecordDate, Description, Amount, RecordType) 
     VALUES (@EmployeeID, @RecordDate, @Description, @Amount, @RecordType)";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@RecordDate", recordDate);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@RecordType", recordType);
                command.ExecuteNonQuery();
                Console.WriteLine("Financial Record Added Successfully!");


            }
            catch(DatabaseConnectionException ex)
            {
                Console.WriteLine($"Database Connection error: {ex.Message}");
            }
            catch (FinancialRecordException ex)
            {
                Console.WriteLine($"Financial record error: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter valid input.");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            con.Close();
        }
        
        public void GetFinancialRecordById()
        {
            try
            {
                Console.WriteLine("Enter Financial Record ID:");
                int recordIDToGet = Convert.ToInt32(Console.ReadLine());

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    string query = @"SELECT F.*, E.FirstName, E.LastName 
                             FROM FinancialRecord F 
                             INNER JOIN Employee E ON F.EmployeeID = E.EmployeeID
                             WHERE F.RecordID = @RecordID;";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@RecordID", recordIDToGet);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Record ID: {reader["RecordID"]}");
                        Console.WriteLine($"Employee Name: {reader["FirstName"]} {reader["LastName"]}");
                        Console.WriteLine($"Record Date: {reader["RecordDate"]}");
                        Console.WriteLine($"Description: {reader["Description"]}");
                        Console.WriteLine($"Amount: {reader["Amount"]}");
                        Console.WriteLine($"Record Type: {reader["RecordType"]}");
                        Console.WriteLine();
                        Console.WriteLine("Data Fetched Successfully");
                    }
                    else
                    {
                        Console.WriteLine("No financial record found with the provided Record ID.");
                    }

                    reader.Close();
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid financial record ID.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void GetFinancialRecordsForEmployee()
        {
            try
            {
                Console.WriteLine("Enter Employee ID to get financial records:");
                int employeeID = Convert.ToInt32(Console.ReadLine());

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    string query = @"SELECT F.*, E.FirstName, E.LastName 
                             FROM FinancialRecord F 
                             INNER JOIN Employee E ON F.EmployeeID = E.EmployeeID
                             WHERE F.EmployeeID = @EmployeeID";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@EmployeeID", employeeID);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Record ID: {reader["RecordID"]}");
                        Console.WriteLine($"Employee Name: {reader["FirstName"]} {reader["LastName"]}");
                        Console.WriteLine($"Record Date: {reader["RecordDate"]}");
                        Console.WriteLine($"Description: {reader["Description"]}");
                        Console.WriteLine($"Amount: {reader["Amount"]}");
                        Console.WriteLine($"Record Type: {reader["RecordType"]}");
                        Console.WriteLine();
                        Console.WriteLine("Data Fetched Successfully");
                        Console.WriteLine();
                    }
                }

                Console.WriteLine("Financial records retrieved successfully!");
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
        
        public void GetFinancialRecordsForDate()
        {
            try
            {
                Console.WriteLine("Enter the year to get financial records (YYYY):");
                int year = Convert.ToInt32(Console.ReadLine());

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    string query = @"SELECT F.*, E.FirstName, E.LastName 
                             FROM FinancialRecord F 
                             INNER JOIN Employee E ON F.EmployeeID = E.EmployeeID
                             WHERE YEAR(F.RecordDate) = @Year";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@Year", year);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine();
                            Console.WriteLine($"RecordID: {reader["RecordID"]}");
                            Console.WriteLine($"Employee Name: {reader["FirstName"]} {reader["LastName"]}");
                            Console.WriteLine($"RecordDate: {reader["RecordDate"]}");
                            Console.WriteLine($"Description: {reader["Description"]}");
                            Console.WriteLine($"Amount: {reader["Amount"]}");
                            Console.WriteLine($"RecordType: {reader["RecordType"]}");
                            Console.WriteLine();
                            
                        }
                        Console.WriteLine("Data Fetched Successfully");
                    }
                    else
                    {
                        Console.WriteLine("No financial records found for the specified year.");
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid year (YYYY).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public void ListFinancialRecordsByRecordType(string recordType)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    string query = @"SELECT F.*, E.FirstName, E.LastName 
                             FROM FinancialRecord F 
                             INNER JOIN Employee E ON F.EmployeeID = E.EmployeeID
                             WHERE F.RecordType = @RecordType";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@RecordType", recordType);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Record ID: {reader["RecordID"]}");
                            Console.WriteLine($"Employee Name: {reader["FirstName"]} {reader["LastName"]}");
                            Console.WriteLine($"Record Date: {reader["RecordDate"]}");
                            Console.WriteLine($"Description: {reader["Description"]}");
                            Console.WriteLine($"Amount: {reader["Amount"]}");
                            Console.WriteLine($"Record Type: {reader["RecordType"]}");
                            Console.WriteLine();
                            
                        }
                        Console.WriteLine("Data Fetched Successfully");
                        Console.WriteLine("Financial records retrieved successfully!");
                    }
                    else
                    {
                        Console.WriteLine($"No financial records found for the record type: {recordType}");
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


























        string constr = "Server=LAGLOP\\SQLEXPRESS;Database=CASE_STUDY;Trusted_Connection=True;TrustServerCertificate=True;";

    }
}