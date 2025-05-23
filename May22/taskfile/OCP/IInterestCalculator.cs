using System;

namespace OCP.solid
{
    public interface IInterestCalculator
    {//interface for calculation the bonus, so that easy to use whenever required 
        double CalculateInterest(double principal);
    }
}