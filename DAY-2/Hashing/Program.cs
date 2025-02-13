using System;

namespace Encryption
{
    class Program
    {
        private static User myUser;
        static void Main(string[] args)
        {
            Console.WriteLine("1.Registration || 2.Login || 3.AddPatient");
            int choice = Convert.ToInt32(Console.ReadLine());
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
                default:
                    break;
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
