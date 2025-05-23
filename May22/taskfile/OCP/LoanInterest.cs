using System;


namespace OCP.solid
{
    public class HomeLoan : IInterestCalculator
    {
        public double CalculateInterest(double principle)
        {
            return principle * 0.8;
        }
    }

    public class PersonalLoan : IInterestCalculator
    {
        public double CalculateInterest(double principle)
        {
            return principle * 0.12;
        }
    }

}