using System;
using ISP.Interfaces;

namespace ISP.Employee
{
    public class Developer : IDeveloper
    {
        public void WriteCode()
        {
            Console.WriteLine("Developer is writing code!");
        }
    }
}