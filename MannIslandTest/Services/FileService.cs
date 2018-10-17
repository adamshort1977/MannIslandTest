using System;
using System.Collections.Generic;
using System.IO;
using MannIslandTest.Models;

namespace MannIslandTest.Services
{
    public class FileService
    {
        public IList<SortCodeRange> SortCodeRanges = new List<SortCodeRange>();

        public void ImportValacdos()
        {
            using (var fileStream = new StreamReader("Data/valacdos.txt"))
            {
                string line;
                SortCodeRanges.Clear();
                while ((line = fileStream.ReadLine()) != null)
                {
                    var lineArr = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                    var ex = 0; //for our purposes, 0 means "no exception, behave normally".
                    if (lineArr.Length == 18)
                        ex = int.Parse(lineArr[17]);
                    var val = ValidationMethod.mod10;
                    if (lineArr[2] == "MOD11")
                    {
                        val = ValidationMethod.mod11;
                    }
                    else if (lineArr[2] == "DBLAL")
                    {
                        val = ValidationMethod.dblal;
                    }

                    SortCodeRanges.Add(new SortCodeRange
                    {
                        MinCode = lineArr[0],
                        MaxCode = lineArr[1],
                        ValidationMethod = val,
                        U = int.Parse(lineArr[3]),
                        V = int.Parse(lineArr[4]),
                        W = int.Parse(lineArr[5]),
                        X = int.Parse(lineArr[6]),
                        Y = int.Parse(lineArr[7]),
                        Z = int.Parse(lineArr[8]),
                        A = int.Parse(lineArr[9]),
                        B = int.Parse(lineArr[10]),
                        C = int.Parse(lineArr[11]),
                        D = int.Parse(lineArr[12]),
                        E = int.Parse(lineArr[13]),
                        F = int.Parse(lineArr[14]),
                        G = int.Parse(lineArr[15]),
                        H = int.Parse(lineArr[16]),
                        Ex = ex
                    });
                }
            }
        }
    }
}
