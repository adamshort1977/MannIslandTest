using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace MannIslandTest.Controllers
{
    public class ValidationController : Controller
    {
        public JsonResult Validate(string sortcode, string accountnumber)
        {
            var test = "this is a test";
            return new JsonResult(test);
        }
    }
}
