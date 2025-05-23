using System;
//sample of OCP

/*
Every time a new employee type comes (like intern,contractor)...
have to edit bonusCalculator class ..
->to calcualte bonus of an employee 

solution: 
create the  bonus calc as an interface 
so that this will be an structure that can be used whenever an type is been added!.
*/



public class BonusCalculator
{
    public double CalculateBonus(string employeeType, double salary)
    {
        if (employeeType == "Permanent")
            return salary * 0.1;
        else if (employeeType == "Temporary")
            return salary * 0.05;
        else
            return 0;
    }
}
