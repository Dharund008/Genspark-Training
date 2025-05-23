 using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;


namespace ExtensionFunctions.Misc
{
    public static class ExtensionFunction
    {
        public static bool StringValidationCheck(this string str)//this important //use to execute with any string data
        {
            if (str.Substring(0, 1).ToLower() == "s" && str.Length == 6) //substring(start_index,length)
                return true;
            return false;
        }
    }
}