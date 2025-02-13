using System;
using System.Security.Cryptography;
using System.Text;

namespace Encryption
{
    class Program
    {
        private static User myUser;
        private static Patient myPatient;
        private static byte[] key;
        static void Main(string[] args)
        {
            Console.WriteLine("1.Registration || 2.Login || 3.AddPatient || 4.View");
            int choice = Convert.ToInt32(Console.ReadLine());
            key = GenerateKey();
            switch (choice)
            {
                case 1:
                    Registration();
                    break;
                case 2:
                    string aUserName = Console.ReadLine();
                    string aPassword = Console.ReadLine();
                    if (aUserName == myUser.UserName && LoginAuthentication(aPassword, myUser.Password))
                    {
                        Console.WriteLine("Login Successful");
                    }
                    break;
                case 3:
                    AddPatient();
                    break;

                case 4:
                    PrintData();
                    break;
                default:
                    break;
            }
           
        }
        private static void AddPatient()
        {
            myPatient = new Patient();
            Console.WriteLine("Enter Patient Name :");
            myPatient.PatientName = Console.ReadLine();
            Console.WriteLine("Enter Patient Emial address :");
            myPatient.EmailAddress = Console.ReadLine();
            Console.WriteLine("Enter Patient SSIN :");
            myPatient.SSIN = Console.ReadLine();
            Console.WriteLine("Enter Patient illness :");
            myPatient.illness = Console.ReadLine();
            EncryptPatientdata();
        }

        private static void  EncryptPatientdata()
        {
            //byte[] key = GenerateKey();
            myPatient.EncryptedEmail=Encryption(myPatient.EmailAddress, key);
            myPatient.EncryptedSSIN = Encryption(myPatient.SSIN, key);
            myPatient.Encryptedillness = Encryption(myPatient.illness, key);
        }
        private static void PrintData()
        {
            Console.WriteLine($"Patient Name :{myPatient.PatientName}");
            Console.WriteLine($"Email Adress:{Decrypt(myPatient.EncryptedEmail)}");
            Console.WriteLine($"SSIN :{Decrypt(myPatient.EncryptedSSIN)}");
            Console.WriteLine($"illness:{Decrypt(myPatient.Encryptedillness)}");
        }
        private static string Decrypt(byte[] encryptedData)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;

                // Extract the IV from the beginning of the encrypted data
                byte[] iv = new byte[16]; // AES block size is 16 bytes
                byte[] ciphertext = new byte[encryptedData.Length - iv.Length];
                Buffer.BlockCopy(encryptedData, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(encryptedData, iv.Length, ciphertext, 0, ciphertext.Length);

                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
        static byte[] GenerateKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[32]; // 32 bytes = 256 bits for AES-256
                rng.GetBytes(key);
                return key;
            }
        }
        private static byte[] Encryption(string text , byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV(); // Generate a random IV

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(text);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    // Prepend the IV to the encrypted data
                    byte[] result = new byte[aes.IV.Length + encryptedBytes.Length];
                    Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);

                    return result;
                }
            }
        }
        private static void Registration()
        {
            myUser = new User();
            Console.WriteLine("Enter UserName :");
            myUser.UserName = Console.ReadLine();
            
            myUser.Password =Console.ReadLine();
            myUser.HashedPassword = HashPassword(myUser.Password);

        }
        private static  string HashPassword(string thePassword)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(thePassword);

            return hashedPassword;
        }
       
        static bool LoginAuthentication(string enteredPassword, string storedHashedPassword)
        {
            // Verify if the entered password matches the stored hashed password
            bool isVerified = BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashedPassword);

            return isVerified;
        }
    }
}
