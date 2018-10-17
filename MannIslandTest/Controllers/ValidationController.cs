using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MannIslandTest.Services;
using Microsoft.AspNetCore.Mvc;
using MannIslandTest.Models;
namespace MannIslandTest.Controllers
{
    public class ValidationController : Controller
    {
        private string fullAccountNumber;
        private IList<SortCodeRange> sortCodeRanges;

        public JsonResult Validate(string sortcode, string accountnumber)
        {
            fullAccountNumber = sortcode + accountnumber;
            var result = new Result();
            result.IsValid = true; 
            //default to accepting in the case that there is no matching sort code (flow chart p10).
            var fileService = new FileService();
            fileService.ImportValacdos();
            sortCodeRanges = fileService.SortCodeRanges.Where(x => x.MinCode.CompareTo(sortcode) <= 0 && x.MaxCode.CompareTo(sortcode) >= 0).ToList();
            foreach(var sortCodeRange in sortCodeRanges)
            {
                var validationException = new ValidationException(sortCodeRange.Ex);
                result.Exception.Add(validationException);
                if (validationException.IsImplemented)
                {
                    //handle exceptions first
                    if (sortCodeRange.Ex == 4)
                    {
                        result.IsValid = result.IsValid && validateException4(sortCodeRange);
                    }
                    else if (sortCodeRange.Ex == 7)
                    {
                        result.IsValid = result.IsValid && validateException7(sortCodeRange);
                    }
                    else
                    {
                        if (sortCodeRange.ValidationMethod == ValidationMethod.mod11)
                            result.IsValid = result.IsValid && validateMod11(sortCodeRange);
                        else if (sortCodeRange.ValidationMethod == ValidationMethod.dblal)
                            result.IsValid = result.IsValid && validateDblal(sortCodeRange);
                        else
                            result.IsValid = result.IsValid && validateMod10(sortCodeRange);
                    }
                }
                else
                    result.IsValid = false; //The exception is unhandled, so technically we don't know if the account number is valid.
            }
            return new JsonResult(result);
        }

        private bool validateDblal(SortCodeRange sortCodeRange)
        {
            return sumIndividualDigits(sortCodeRange) % 10 == 0;
        }

        private bool validateMod10(SortCodeRange sortCodeRange)
        {
            return sumWeightings(sortCodeRange) % 10 == 0;
        }

        private bool validateMod11(SortCodeRange sortCodeRange)
        {
            return sumWeightings(sortCodeRange) % 11 == 0;
        }

        private bool validateException4(SortCodeRange sortCodeRange)
        {
            return sumWeightings(sortCodeRange) % 11 == int.Parse(string.Concat(fullAccountNumber[12], fullAccountNumber[13]));
        }

        private bool validateException7(SortCodeRange sortCodeRange)
        {
            if (fullAccountNumber[12]==9)
            {
                sortCodeRange.U = 0;
                sortCodeRange.V = 0;
                sortCodeRange.W = 0;
                sortCodeRange.X = 0;
                sortCodeRange.Y = 0;
                sortCodeRange.Z = 0;
                sortCodeRange.A = 0;
                sortCodeRange.B = 0;
            }
            switch (sortCodeRange.ValidationMethod)
            {
                case ValidationMethod.dblal:
                    return validateDblal(sortCodeRange);
                case ValidationMethod.mod10:
                    return validateMod10(sortCodeRange);
                default:
                    return validateMod11(sortCodeRange);
            }

        }

        private int sumIndividualDigits(SortCodeRange sortCodeRange) 
        {
            var weighted_u = sortCodeRange.U * fullAccountNumber[0];
            var weighted_v = sortCodeRange.V * fullAccountNumber[1];
            var weighted_w = sortCodeRange.W * fullAccountNumber[2];
            var weighted_x = sortCodeRange.X * fullAccountNumber[3];
            var weighted_y = sortCodeRange.Y * fullAccountNumber[4];
            var weighted_z = sortCodeRange.Z * fullAccountNumber[5];
            var weighted_a = sortCodeRange.A * fullAccountNumber[6];
            var weighted_b = sortCodeRange.B * fullAccountNumber[7];
            var weighted_c = sortCodeRange.C * fullAccountNumber[8];
            var weighted_d = sortCodeRange.D * fullAccountNumber[9];
            var weighted_e = sortCodeRange.E * fullAccountNumber[10];
            var weighted_f = sortCodeRange.F * fullAccountNumber[11];
            var weighted_g = sortCodeRange.G * fullAccountNumber[12];
            var weighted_h = sortCodeRange.H * fullAccountNumber[13];
            var weightedString = string.Concat(weighted_u, weighted_v, weighted_w,
                                              weighted_x, weighted_y, weighted_z,
                                              weighted_a, weighted_b, weighted_c,
                                              weighted_d, weighted_e, weighted_f,
                                               weighted_g, weighted_h);
            var sum = 0;
            for (int i = 0; i < 14; i++)
            {
                sum += weightedString[i];
            }
            return sum;
        }

        private int sumWeightings(SortCodeRange sortCodeRange)
        {
            var weighted_u = sortCodeRange.U * fullAccountNumber[0];
            var weighted_v = sortCodeRange.V * fullAccountNumber[1];
            var weighted_w = sortCodeRange.W * fullAccountNumber[2];
            var weighted_x = sortCodeRange.X * fullAccountNumber[3];
            var weighted_y = sortCodeRange.Y * fullAccountNumber[4];
            var weighted_z = sortCodeRange.Z * fullAccountNumber[5];
            var weighted_a = sortCodeRange.A * fullAccountNumber[6];
            var weighted_b = sortCodeRange.B * fullAccountNumber[7];
            var weighted_c = sortCodeRange.C * fullAccountNumber[8];
            var weighted_d = sortCodeRange.D * fullAccountNumber[9];
            var weighted_e = sortCodeRange.E * fullAccountNumber[10];
            var weighted_f = sortCodeRange.F * fullAccountNumber[11];
            var weighted_g = sortCodeRange.G * fullAccountNumber[12];
            var weighted_h = sortCodeRange.H * fullAccountNumber[13];

            return weighted_u + weighted_v + weighted_w + weighted_x + weighted_y + weighted_z
                + weighted_a + weighted_b + weighted_c + weighted_d + weighted_e + weighted_f + weighted_g
                + weighted_h;

        }
    }
}
