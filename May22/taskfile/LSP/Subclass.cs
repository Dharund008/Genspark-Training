using System;

namespace LSP.solid
{
    public class FulltimeEmployee : IWorkingHours
    {
        public int CalculateWorkingHours()
        {
            return 6;
        }
    }
    public class ParttimeEmployee : IWorkingHours
    {
        public int CalculateWorkingHours()
        {
            return 8;
        }
    }

    public class ContractEmployee : IWorkingHours
    {
        public int CalculateWorkingHours()
        {
            throw new NotSupportedException("Contract employees do not have fixed working hours.");

        }
    }
}