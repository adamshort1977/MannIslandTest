using System;
namespace MannIslandTest.Models
{
    public class SortCodeRange
    {
        public SortCodeRange()
        {
        }

        public string MinCode { get; set; }
        public string MaxCode { get; set; }
        public ValidationMethod ValidationMethod { get; set; }
        public int U { get; set; }
        public int V { get; set; }
        public int W { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }
        public int E { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public int Ex { get; set; }
    }

    public enum ValidationMethod 
    {
        mod10,
        mod11,
        dblal
    }
}
