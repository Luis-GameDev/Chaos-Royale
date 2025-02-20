using System;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    private string connectionString = "Server=localhost;Database=db_chaosroyale;User ID=root;Password=;Pooling=false";
    
    public bool Register(string username, string password, string passwordCheck)
    {
        if (password != passwordCheck)
        {
            Console.WriteLine("Passwords do not match.");
            return false;  
        }

        string hashedPassword = HashPassword(password);

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string checkUserQuery = "SELECT COUNT(*) FROM user WHERE username = @username";
                using (MySqlCommand checkCmd = new MySqlCommand(checkUserQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@username", username);
                    long userExists = (long)checkCmd.ExecuteScalar();

                    if (userExists > 0)
                    {
                        Console.WriteLine("User already exists.");
                        return false; 
                    }
                }

                string insertQuery = "INSERT INTO user (username, password_hash) VALUES (@username, @password)";
                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("User registered successfully.");
                return true; 
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during registration: " + ex.Message);
            return false;  
        }
    }

    public bool Login(string username, string password)
    {
        string hashedPassword = HashPassword(password);

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string loginQuery = "SELECT password_hash FROM user WHERE username = @username";
                using (MySqlCommand cmd = new MySqlCommand(loginQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    var result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        Console.WriteLine("User not found.");
                        return false; 
                    }

                    string storedPassword = result.ToString();

                    if (storedPassword == hashedPassword)
                    {
                        Console.WriteLine("Login successful.");
                        return true;  
                    }
                    else
                    {
                        Console.WriteLine("Incorrect password.");
                        return false;  
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during login: " + ex.Message);
            return false;  
        }
    }

    public int GetUserID(string username, string password) {
        string hashedPassword = HashPassword(password);

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT ID FROM user WHERE username = @username AND password_hash = @password";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        Console.WriteLine("User not found or incorrect password.");
                        return -1; 
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during GetUserID: " + ex.Message);
            return -1;  
        }
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
