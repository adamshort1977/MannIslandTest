using System;
namespace MannIslandTest.Models
{
    public class Result
    {
        public Result()
        {
        }

        public bool IsValid { get; set; }
        public ValidationException Exception { get; set; }
    }
}
