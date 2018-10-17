using System;
namespace MannIslandTest.Models
{
    public class ValidationException
    {
        public bool IsImplemented { get; set; }
        public int ExceptionNumber { get; set; }

        public ValidationException(int exceptionNumber)
        {
            ExceptionNumber = exceptionNumber;
            IsImplemented = exceptionNumber==0 || exceptionNumber == 4 || exceptionNumber == 7;
        }
    }
}
