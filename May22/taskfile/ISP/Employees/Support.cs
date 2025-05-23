using System;
using ISP.Interfaces;

namespace ISP.Employee
{
    public class Support : ISupport
    {
        public void HandleSupport()
        {
            Console.WriteLine("Support staff......");
        }
    }
}