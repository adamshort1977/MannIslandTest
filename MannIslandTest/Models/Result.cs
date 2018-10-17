using System;
using System.Collections.Generic;

namespace MannIslandTest.Models
{
    public class Result
    {
        public Result()
        {
        }

        public bool IsValid { get; set; }
        public IList<ValidationException> Exception = new List<ValidationException>();
    }
}
