using System;
using ISP.Interfaces;

namespace ISP.Employee
{
    public class Manager : IManager
    {
        public void Manages()
        {
            Console.WriteLine("Manager currently managing the work!");
        }
    }
}