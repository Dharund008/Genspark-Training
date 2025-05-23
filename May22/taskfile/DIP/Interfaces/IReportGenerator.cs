using System;


namespace DIP.solid
{
    public interface IReportGenerator
    {
        void GenerateReport(IEmployee employee);
    }
}