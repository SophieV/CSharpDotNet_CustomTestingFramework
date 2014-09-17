using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    public class Program
    {
        public static void Main()
        {
            var testUser = new TestSuiteUser();
            testUser.RunAllTests();
        }
    }
}