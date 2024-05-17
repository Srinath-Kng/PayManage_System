using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayManage_System.entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PayManage_System.util;
using static PayManage_System.exception.InvalidDataException;


namespace PayManage_System.dao
{
    public class EmployeeService : IEmployeeService
    {
        public DateTime GetEmployeeDateOfBirth(int employeeID)
        {
            string selectQuery = "SELECT DateOfBirth FROM Employee WHERE EmployeeID = @EmployeeID";
            using (SqlConnection con = DatabaseService.GetConnection())
            {
                SqlCommand command = new SqlCommand(selectQuery, con);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                con.Open();
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return (DateTime)result;
                }
                else
                {
                    throw new EmployeeNotFoundException($"Employee with ID {employeeID} not found.");
                }
            } 
        }

        public void GetEmployeeById(int employeeID)
        {
            using (SqlConnection con = DatabaseService.GetConnection())
            {
                try
                {
                    string selectQuery = "SELECT * FROM Employee WHERE EmployeeID = @EmployeeID";
                    SqlCommand command = new SqlCommand(selectQuery, con);
                    command.Parameters.AddWithValue("@EmployeeID", employeeID);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Employee ID:       {reader["EmployeeID"]}");
                            Console.WriteLine($"First Name:        {reader["FirstName"]}");
                            Console.WriteLine($"Last Name:         {reader["LastName"]}");
                            Console.WriteLine($"Date of Birth:     {reader["DateOfBirth"]}");
                            Console.WriteLine($"Gender:            {reader["Gender"]}");
                            Console.WriteLine($"Email:             {reader["Email"]}");
                            Console.WriteLine($"Phone Number:      {reader["PhoneNumber"]}");
                            Console.WriteLine($"Address:           {reader["Address"]}");
                            Console.WriteLine($"Position:          {reader["Position"]}");
                            Console.WriteLine($"Joining Date:      {reader["JoiningDate"]}");
                            Console.WriteLine($"Termination Date:  {reader["TerminationDate"]}");
                            Console.WriteLine();


                        }
                        Console.WriteLine("Employee Data Retrived Successfully");
                    }
                    else
                    {
                        Console.WriteLine("No employee found with the provided Employee ID.");
                    }
                }
                catch (EmployeeNotFoundException ex)
                {
                    Console.WriteLine($"Employee not found: {ex.Message}");
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }
        public void GetAllEmployees()
        {
            using (SqlConnection con = DatabaseService.GetConnection())
            {
                try
                {
                    string sql = "SELECT * FROM Employee";
                    SqlCommand command = new SqlCommand(sql, con);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"Employee ID:       {reader["EmployeeID"]}");
                        Console.WriteLine($"First Name:        {reader["FirstName"]}");
                        Console.WriteLine($"Last Name:         {reader["LastName"]}");
                        Console.WriteLine($"Date of Birth:     {reader["DateOfBirth"]}");
                        Console.WriteLine($"Gender:            {reader["Gender"]}");
                        Console.WriteLine($"Email:             {reader["Email"]}");
                        Console.WriteLine($"Phone Number:      {reader["PhoneNumber"]}");
                        Console.WriteLine($"Address:           {reader["Address"]}");
                        Console.WriteLine($"Position:          {reader["Position"]}");
                        Console.WriteLine($"Joining Date:      {reader["JoiningDate"]}");
                        Console.WriteLine($"Termination Date:  {reader["TerminationDate"]}");
                        Console.WriteLine();
                    }
                    Console.WriteLine("All the Data's have been Fetched From the Database");
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
        }
        
        
        public void AddEmployee(string firstName, string lastName, DateTime dateOfBirth, string gender, string email, string phoneNumber, string address, string position, DateTime joiningDate, DateTime? terminationDate)
        {
            
            SqlConnection con = new SqlConnection(constr);
            con.Open();


            string query = @"INSERT INTO Employee (FirstName, LastName, DateOfBirth, Gender, Email, PhoneNumber, Address, Position, JoiningDate, TerminationDate) 
                                     VALUES (@FirstName, @LastName, @DateOfBirth, @Gender, @Email, @PhoneNumber, @Address, @Position, @JoiningDate, @TerminationDate)";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@FirstName", firstName);
            command.Parameters.AddWithValue("@LastName", lastName);
            command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
            command.Parameters.AddWithValue("@Gender", gender);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
            command.Parameters.AddWithValue("@Address", address);
            command.Parameters.AddWithValue("@Position", position);
            command.Parameters.AddWithValue("@JoiningDate", joiningDate);
            command.Parameters.AddWithValue("@TerminationDate", terminationDate);
            command.ExecuteNonQuery();
            Console.WriteLine("Employee added successfully!");

            con.Close();


        }

       




        public void UpdateEmployee(int employeeID, string attribute, string updatedValue)
        {

            SqlConnection con = new SqlConnection(constr);
            con.Open();
            try
            {
                string updateQuery = "";

                switch (attribute.ToLower())
                {
                    case "firstname":
                        updateQuery = "UPDATE Employee SET FirstName = @UpdatedValue WHERE EmployeeID = @EmployeeID";
                        break;
                    case "lastname":
                        updateQuery = "UPDATE Employee SET LastName = @UpdatedValue WHERE EmployeeID = @EmployeeID";
                        break;
                    case "dateofbirth":
                        updateQuery = "UPDATE Employee SET DateOfBirth = @UpdatedValue WHERE EmployeeID = @EmployeeID";
                        break;
                    case "gender":
                        updateQuery = "UPDATE Employee SET Gender = @UpdatedValue WHERE EmployeeID = @EmployeeID";
                        break;
                    case "email":
                        updateQuery = "UPDATE Employee SET Email = @UpdatedValue WHERE EmployeeID = @EmployeeID";
                        break;
                    case "phonenumber":
                        updateQuery = "UPDATE Employee SET PhoneNumber = @UpdatedValue WHERE EmployeeID = @EmployeeID";
                        break;
                    case "address":
                        updateQuery = "UPDATE Employee SET Address = @UpdatedValue WHERE EmployeeID = @EmployeeID";
                        break;
                    case "position":
                        updateQuery = "UPDATE Employee SET Position = @UpdatedValue WHERE EmployeeID = @EmployeeID";
                        break;
                    case "joiningdate":
                        updateQuery = "UPDATE Employee SET JoiningDate = @UpdatedValue WHERE EmployeeID = @EmployeeID";
                        break;
                    case "terminationdate":
                        updateQuery = "UPDATE Employee SET TerminationDate = @UpdatedValue WHERE EmployeeID = @EmployeeID";
                        break;
                    default:
                        Console.WriteLine("Invalid attribute name!");
                        return;
                }

                SqlCommand command = new SqlCommand(updateQuery, con);
                command.Parameters.AddWithValue("@UpdatedValue", updatedValue);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Employee attribute updated successfully!");
                }
                else
                {
                    Console.WriteLine("No employee found with the provided Employee ID.");
                }

            }
            catch (EmployeeNotFoundException ex)
            {
                Console.WriteLine($"Employee not found: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);

            }
            con.Close();

        }
        


        public void RemoveEmployee(int employeeID)
        {
            
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            try
            {
                string deleteQuery = "DELETE FROM Employee WHERE EmployeeID = @EmployeeID";
                SqlCommand command = new SqlCommand(deleteQuery, con);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Employee removed successfully!");
                }
                else
                {
                    Console.WriteLine("No employee found with the provided Employee ID.");
                }

            }
            catch (EmployeeNotFoundException ex)
            {
                Console.WriteLine($"Employee not found: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            con.Close();
        }






















































        string constr = "Server=LAGLOP\\SQLEXPRESS;Database=CASE_STUDY;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}
