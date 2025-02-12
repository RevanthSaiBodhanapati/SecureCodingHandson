using System;

namespace SecureCoding
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter two numbers : ");
                int a = Convert.ToInt32(Console.ReadLine());
                int b = Convert.ToInt32(Console.ReadLine());
                if (!(b > 0 && a > int.MaxValue - b ||
                b < 0 && a < int.MinValue - b))
                {
                    Console.WriteLine($"Result :{a+b}");
                }
               
            }
            catch(Exception theEx)
            {
                Console.WriteLine(theEx.Message);
                Console.WriteLine(theEx);
            }
        }
      
    }
}
