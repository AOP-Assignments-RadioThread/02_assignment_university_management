using System.Security.Cryptography;
using System.Text;
namespace UniversityManager.Models;

public static class PasswordHasher
{
    // Hash a password using SHA256
    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // Convert the input string to a byte array and compute the hash
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            // Convert byte array to a string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2")); // Convert to hexadecimal
            }
            return builder.ToString();
        }
    }

    // Verify a password against a stored hash
    public static bool VerifyPassword(string password, string storedHash)
    {
        // Hash the input password
        string hashedPassword = HashPassword(password);
        
        // Compare the computed hash with the stored hash
        return hashedPassword == storedHash;
    }
}

    

