using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using System.IO;

namespace BestBuyCRUD
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggerMessage("Begin Running");
            var departments = GetAllDepratment();
            Console.WriteLine("Would you like to see all departments?");
            Console.WriteLine("Yes or No");

            if (Console.ReadLine().ToUpper() == "YES" || Console.ReadLine().ToUpper() == "YE")
            {
                Console.WriteLine("Please wait a moment...");

                foreach (var department in departments)
                {
                    Console.WriteLine(department);
                }
            }
            try
            {
                Console.WriteLine(departments[8]);
            }
            catch(Exception e)
            {
                LoggerError(e);
            }
            
     
        }

        static void LoggerMessage(string message)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Environment.NewLine}---------------{Environment.NewLine}");
            sb.Append($"{message} {DateTime.Now}");
            sb.Append($"{Environment.NewLine}---------------{Environment.NewLine}");
            var filePath = "";

            File.AppendAllText(filePath + "log.txt", sb.ToString());

        }

        static void LoggerError(Exception error)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Environment.NewLine}---------------{Environment.NewLine}");
            sb.Append($"{error.Message}{DateTime.Now}");
            sb.Append($"{Environment.NewLine}---------------{Environment.NewLine}");
            var filePath = "";

            File.AppendAllText(filePath + "log.txt", sb.ToString());
        }

        static List<string> GetAllDepratment()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = System.IO.File.ReadAllText("connectionstring.txt");

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT Name FROM Departments;";

            using (conn)
            {
                conn.Open();
                List<String> allDepartments = new List<string>();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read() == true)
                {
                    var currentDepartment = reader.GetString("Name");
                    allDepartments.Add(currentDepartment);
                }

                return allDepartments;
            }
        }

        static void InsertNewDepartment(string newDepartmentName)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = System.IO.File.ReadAllText("connectionString.txt");

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "INSERT INTO departments (Name) VALUES (@deptname);";
            cmd.Parameters.AddWithValue("deptname", newDepartmentName);

            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

        static void DeleteDepartment(string departmentName)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = System.IO.File.ReadAllText("connectionString.txt");

            MySqlCommand cmd = conn.CreateCommand();
            // parameterized query to prevent SQL Injection
            cmd.CommandText = "DELETE FROM departments WHERE Name = @departmentName;";
            cmd.Parameters.AddWithValue("departmentName", departmentName);

            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

        static void UpdateDeparmentName(string currentDepartmentName, string newDepartmentName)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = System.IO.File.ReadAllText("connectionString.txt");

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "UPDATE departments SET Name = @newDepartmentName WHERE name = @currentDepartmentName;";
            cmd.Parameters.AddWithValue("currentDepartmentName", currentDepartmentName);
            cmd.Parameters.AddWithValue("newDepartmentName", newDepartmentName);

            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

}
