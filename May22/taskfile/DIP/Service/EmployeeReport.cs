using System;
using System.Collections.Generic;

namespace DIP.solid
{
    public class EmployeeReport
    {
        private readonly IReportGenerator _reportGenerator;

        public EmployeeReport(IReportGenerator reportGenerator)
        {
            _reportGenerator = reportGenerator;
        }

        public void Generate(IEmployee emp)
        {
            _reportGenerator.GenerateReport(emp);
        }
    }
}