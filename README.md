# 🎉 Payroll Management System 🎉

This is a console-based Payroll Management System developed in C# that allows for the management of employees, payroll processing, tax calculations, and financial reporting.

![GitHub repo size](https://img.shields.io/github/repo-size/your-username/payroll-management-system)
![GitHub stars](https://img.shields.io/github/stars/your-username/payroll-management-system)
![GitHub forks](https://img.shields.io/github/forks/your-username/payroll-management-system)
![GitHub issues](https://img.shields.io/github/issues/your-username/payroll-management-system)
![GitHub license](https://img.shields.io/github/license/your-username/payroll-management-system)

## 📁 Project Structure

The project is organized into several folders:

- **Entity**: Contains the data model classes.
- **Interfaces**: Contains the service interfaces.
- **Services**: Contains the service implementations.
- **Exceptions**: Contains custom exception classes.
- **Program**: Contains the main program and the menu structure.

## 🌟 Features

### 👨‍💼 Employee Management

- CRUD operations for employee data, including personal details, position, and employment history.

### 💵 Payroll Processing

- Automated calculation of employee salaries and deductions.
- Generation of pay stubs for each pay period.

### 📊 Tax Calculation

- Automatic computation of taxes based on employee income and deductions.

### 📑 Financial Reporting

- Generation of financial reports, including income statements and tax summaries.

## 🗃️ SQL Tables

| Table Name         | Columns                                                                                 |
|--------------------|-----------------------------------------------------------------------------------------|
| **Employee**       | `EmployeeID` (Primary Key), `FirstName`, `LastName`, `DateOfBirth`, `Gender`, `Email`, `PhoneNumber`, `Address`, `Position`, `JoiningDate`, `TerminationDate` |
| **Payroll**        | `PayrollID` (Primary Key), `EmployeeID` (Foreign Key), `PayPeriodStartDate`, `PayPeriodEndDate`, `BasicSalary`, `OvertimePay`, `Deductions`, `NetSalary`        |
| **Tax**            | `TaxID` (Primary Key), `EmployeeID` (Foreign Key), `TaxYear`, `TaxableIncome`, `TaxAmount` |
| **FinancialRecord**| `RecordID` (Primary Key), `EmployeeID` (Foreign Key), `RecordDate`, `Description`, `Amount`, `RecordType` |

### 🛠️ Dependencies

- Add NUnit Framework in Visual Studio

## 🚀 Setup and Installation

1. Clone the repository:

   ```sh
   git clone https://github.com/your-username/payroll-management-system.git
