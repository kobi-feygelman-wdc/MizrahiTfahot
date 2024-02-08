using System;

namespace CalculationAPI.Models
{
    public class CalcException : Exception
    {
        public CalcException(string msg) : base(msg)
        {
        }
    }
}
