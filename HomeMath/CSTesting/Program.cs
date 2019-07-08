using System;

namespace CSTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            int num1 = 1111;
            int num2 = 1212;
            char opp = '+';
            int answer = 238293;
            string str = String.Format("{0,25} = {1,-10}",
               num1 + " " + opp + " " + num2, answer);
            Console.WriteLine(str + "A");
            Console.ReadLine();
        }
    }
}
