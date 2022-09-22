﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayrollSQL
{
    public class EmployeeRepo
    {
        string connectionString = @"Data Source=(localdb)\ProjectModels;Initial Catalog=Payroll_Service;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        SqlConnection connection = null;
        public void GetAllEmployees()
        {
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    EmployeeModel model = new EmployeeModel();
                    string query = "select * from employee_payroll";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            model.Id = Convert.ToInt32(reader["Id"] == DBNull.Value ? default : reader["Id"]);
                            model.Name = Convert.ToString(reader["Name"] == DBNull.Value ? default : reader["Name"]);
                            model.Basic_Pay = Convert.ToDouble(reader["Basic_Pay"] == DBNull.Value ? default : reader["Basic_Pay"]);
                            model.Startdate = (DateTime)(reader["Startdate"] == DBNull.Value ? default : reader["Startdate"]);
                            model.Gender = Convert.ToChar(reader["Gender"] == DBNull.Value ? default : reader["Gender"]);
                            model.Phone = Convert.ToInt64(reader["Phone"] == DBNull.Value ? default : reader["Phone"]);
                            model.Address = Convert.ToString(reader["Address"] == DBNull.Value ? default : reader["Address"]);
                            model.Department = Convert.ToString(reader["Department"] == DBNull.Value ? default : reader["Department"]);
                            model.Deductions = Convert.ToDouble(reader["Deductions"] == DBNull.Value ? default : reader["Deductions"]);
                            model.Taxable_Pay = Convert.ToDouble(reader["Taxable_Pay"] == DBNull.Value ? default : reader["Taxable_Pay"]);
                            model.Income_Tax = Convert.ToDouble(reader["Income_Tax"] == DBNull.Value ? default : reader["Income_Tax"]);
                            model.Net_Pay = Convert.ToDouble(reader["Net_Pay"] == DBNull.Value ? default : reader["Net_Pay"]);
                            Console.WriteLine($"{model.Id}, {model.Name}, {model.Basic_Pay}, {model.Startdate}, {model.Gender}, {model.Phone}, {model.Address}, {model.Department}, {model.Deductions}, {model.Taxable_Pay}, {model.Income_Tax}, {model.Net_Pay} \n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //finally
            //{
            //    connection.Close(); Require if we don't use using above 
            //}
        }
        public void AddEmployee(EmployeeModel obj)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand("spAddEmployees", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Name", obj.Name);
                command.Parameters.AddWithValue("@Gender", obj.Gender);
                command.Parameters.AddWithValue("@Startdate", obj.Startdate);
                command.Parameters.AddWithValue("@Phone", obj.Phone);
                command.Parameters.AddWithValue("@Department", obj.Department);
                command.Parameters.AddWithValue("@Address", obj.Address);
                command.Parameters.AddWithValue("@Basic_Pay", obj.Basic_Pay);
                command.Parameters.AddWithValue("@Deductions", obj.Deductions);
                command.Parameters.AddWithValue("@Taxable_Pay", obj.Taxable_Pay);
                command.Parameters.AddWithValue("@Income_Tax", obj.Income_Tax);
                command.Parameters.AddWithValue("@Net_Pay", obj.Net_Pay);
                connection.Open();
                var result = command.ExecuteNonQuery();
                if (result != 0)
                {
                    Console.WriteLine("Employee details added successfully");
                }
                else
                {
                    Console.WriteLine("Failed to add employee details");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public void UpdateEmployee(EmployeeModel obj)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand("spUpdateEmployee", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@Id", obj.Id);
                command.Parameters.AddWithValue("@Name", obj.Name);
                command.Parameters.AddWithValue("@Basic_Pay", obj.Basic_Pay);
                connection.Open();
                var result = command.ExecuteNonQuery();
                if (result != 0)
                {
                    Console.WriteLine("Employee details updated successfully");
                }
                else
                {
                    Console.WriteLine("Failed to update employee details");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public void DeleteEmployee(EmployeeModel obj)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand("spDeleteEmployee", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@Id", obj.Id);
                command.Parameters.AddWithValue("@Name", obj.Name);
                connection.Open();
                var result = command.ExecuteNonQuery();
                if (result != 0)
                {
                    Console.WriteLine("Employee details deleted successfully");
                }
                else
                {
                    Console.WriteLine("Failed to deleted employee details");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public void GetEmployeesWithDataAdapter(string query)
        {
            try
            {
                DataSet dataSet = new DataSet();
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.Fill(dataSet);
                    foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                    {
                        Console.WriteLine(dataRow["Id"] + ", " + dataRow["Name"] + ", " + dataRow["StartDate"] + ", " + dataRow["Gender"] + ", " + dataRow["Phone"] + ", " + dataRow["Address"] + ", " + dataRow["Department"] + ", " + dataRow["Basic_Pay"] + ", " + dataRow["Deductions"] + ", " + dataRow["Taxable_Pay"] + ", " + dataRow["Income_Tax"] + ", " + dataRow["Net_Pay"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public void GetSumOfSalary(string query)
        {
            try
            {
                DataSet dataSet = new DataSet();
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.Fill(dataSet);
                    foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                    {
                        Console.WriteLine(dataRow["sumsalary"] + "," + dataRow["Gender"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public void GetAvgOfSalary(string query)
        {
            try
            {
                DataSet dataSet = new DataSet();
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.Fill(dataSet);
                    foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                    {
                        Console.WriteLine(dataRow["avgsalary"] + "," + dataRow["Gender"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public void GetMaxOfSalary(string query)
        {
            try
            {
                DataSet dataSet = new DataSet();
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.Fill(dataSet);
                    foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                    {
                        Console.WriteLine(dataRow["maxsalary"] + "," + dataRow["Gender"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public void GetMinOfSalary(string query)
        {
            try
            {
                DataSet dataSet = new DataSet();
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.Fill(dataSet);
                    foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                    {
                        Console.WriteLine(dataRow["minsalary"] + "," + dataRow["Gender"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public void GetCount(string query)
        {
            try
            {
                DataSet dataSet = new DataSet();
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.Fill(dataSet);
                    foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                    {
                        Console.WriteLine(dataRow["EmployeeCount"] + "," + dataRow["Gender"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public void InsertIntoTwoTables(EmployeeModel obj)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = new SqlCommand("spInsertIntoTwoTables", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", obj.Name);
                command.Parameters.AddWithValue("@Gender", obj.Gender);
                command.Parameters.AddWithValue("@Address", obj.Address);
                //Id is output parameter so thar it will not accept any input from user
                command.Parameters.Add("Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                var result = command.ExecuteScalar();
                //Convert To String
                //In order TO fetch value used .value
                string id = command.Parameters["Id"].Value.ToString();
                //Convert To Int
                int newId = Convert.ToInt32(id);

                string query = $"Insert into Payroll_Details(EmpId, Salary) values({newId},7894)";
                SqlCommand cmd = new SqlCommand(query, connection);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    Console.WriteLine("Inserted into salary table successfully");
                }
                else
                {
                    Console.WriteLine("Failed to insert into salary table");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //Refactor UC8 using transactional query
        public void InsertIntoTwoTablesWithTransactions()
        {
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction sqlTran = connection.BeginTransaction();             //Start a local transaction
                SqlCommand command = connection.CreateCommand();                    //Enlist a command in the current transaction
                command.Transaction = sqlTran;

                try
                {
                    //Execute two seperate commands
                    command.CommandText = $"insert into employee_payroll(Name, Basic_pay, Address) values('Raj', '36000', 'Hyd')";
                    command.ExecuteScalar();
                    command.CommandText = "insert into Payroll_Details(EmpID, Salary) values(13, 36000)";
                    int res = command.ExecuteNonQuery();
                    if (res != 0)
                    {
                        //commit transaction
                        sqlTran.Commit();
                        Console.WriteLine("Both queries successfull");
                    }
                }
                catch (Exception ex)
                {
                    //Handle the exception if transaction fails to commit
                    Console.WriteLine(ex.Message);
                    try
                    {
                        //Attempt to rollback transaction
                        sqlTran.Rollback();
                    }
                    catch (Exception exRollBack)
                    {
                        //Throws an invalidOperationexception if the connection is closed or the transaction has already been rolled back on the server
                        Console.WriteLine(exRollBack.Message);
                    }
                }
            }
        }
    }
}