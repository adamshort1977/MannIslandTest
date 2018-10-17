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
        private SortCodeRange sortCodeRange;

        public JsonResult Validate(string sortcode, string accountnumber)
        {
            fullAccountNumber = sortcode + accountnumber;
            var result = new Result();
            result.IsValid = true; 
            //default to accepting in the case that there is no matching sort code (flow chart p10).
            var fileService = new FileService();
            fileService.ImportValacdos();
            sortCodeRange = fileService.SortCodeRanges.FirstOrDefault(x => x.MinCode.CompareTo(sortcode) <= 0 && x.MaxCode.CompareTo(sortcode) >= 0);
            if (sortCodeRange!=null) 
            {
                result.Exception = new ValidationException(sortCodeRange.Ex);
                if (result.Exception.IsImplemented)
                {
                    //handle exceptions first
                    if (sortCodeRange.Ex == 4)
                    {
                        result.IsValid = validateException4();
                    }
                    else if (sortCodeRange.Ex == 7)
                    {
                        result.IsValid = validateException7();
                    }
                    else
                    {
                        if (sortCodeRange.ValidationMethod == ValidationMethod.mod11)
                            result.IsValid = validateMod11();
                        else if (sortCodeRange.ValidationMethod == ValidationMethod.dblal)
                            result.IsValid = validateDblal();
                        else
                            result.IsValid = validateMod10();
                    }
                }
                else
                    result.IsValid = false; //The exception is unhandled, so technically we don't know if the account number is valid.
            }
            else 
            {
                result.Exception = new ValidationException(0); //No exception because there's no sort code
                                                               //match, should be treated as valid per page 10.
            }
            return new JsonResult(result);
        }

        private bool validateDblal()
        {
            return sumIndividualDigits() % 10 == 0;
        }

        private bool validateMod10()
        {
            return sumWeightings() % 10 == 0;
        }

        private bool validateMod11()
        {
            return sumWeightings() % 11 == 0;
        }

        private bool validateException4()
        {
            return sumWeightings() % 11 == int.Parse(string.Concat(fullAccountNumber[12], fullAccountNumber[13]));
        }

        private bool validateException7()
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
                    return validateDblal();
                case ValidationMethod.mod10:
                    return validateMod10();
                default:
                    return validateMod11();
            }

        }

        private int sumIndividualDigits() 
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

        private int sumWeightings()
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
