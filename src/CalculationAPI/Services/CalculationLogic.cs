using System;
using System.Collections.Generic;

namespace CalculationAPI.Models
{
    public partial class CalculateBody 
    {
        private delegate decimal CalcOperation(decimal? a, decimal? b);
        private static Dictionary<OperationEnum, CalcOperation> opTable = new Dictionary<OperationEnum, CalcOperation>()
        { 
            { OperationEnum.AddEnum, AddDecimals},
            { OperationEnum.SubtractEnum, SubDecimals},
            { OperationEnum.MultiplyEnum, MultDecimals},
            { OperationEnum.DivideEnum, DivDecimals}
        };

        /// <summary>
        /// Adds 2 decimals
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="CalcException"></exception>
        private static decimal AddDecimals(decimal? a, decimal? b)
        {
            if (a.HasValue && b.HasValue)
                return a.Value + b.Value;

            if (a.HasValue)
                return a.Value;
            
            if (b.HasValue)
                return b.Value;

            throw new CalcException("No operands provided");
        }

        /// <summary>
        /// Subtracts 2 decimals
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="CalcException"></exception>
        private static decimal SubDecimals(decimal? a, decimal? b)
        {
            if (a.HasValue && b.HasValue)
                return a.Value - b.Value;

            if (a.HasValue)
                return a.Value;

            if (b.HasValue)
                return -b.Value;

            throw new CalcException("No operands provided");
        }
        
        /// <summary>
        /// Multiplies 2 decimals
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="CalcException"></exception>
        private static decimal MultDecimals(decimal? a, decimal? b)
        {
            if (a.HasValue && b.HasValue)
                return a.Value * b.Value;

            throw new CalcException("Not all operands provided");
        }

        /// <summary>
        /// Divides 2 operands 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="CalcException"></exception>
        private static decimal DivDecimals(decimal? a, decimal? b)
        {
            if (a.HasValue && b.HasValue)
            {
                if (b.Value == 0)
                    throw new CalcException("Can't divide by zero");

                return a.Value / b.Value;
            }

            throw new CalcException("Not all operands provided");
        }

        /// <summary>
        /// Calculates the operation result with Operand1 and Operand2
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CalcException"></exception>
        public decimal Calculate()
        {
            if (!Operation.HasValue)
                throw new CalcException("Operation parameter not provided");

            if (!opTable.ContainsKey(Operation.Value))
                throw new CalcException($"Operation {Operation.Value} not supported");

            return opTable[Operation.Value](Operand1, Operand2);
        }
    }
}
