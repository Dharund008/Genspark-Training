using System;
using System.Collections.Generic;

namespace DIP.solid
{
    public class ReportGenerator : IReportGenerator
    {
        public void GenerateReport(IEmployee employee)
        {
            Console.WriteLine("Generating PDF report for:");
            Console.WriteLine(employee.GetEmployeeDetails());
        }
    }
}