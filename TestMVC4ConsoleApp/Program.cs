using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    public class Program
    {
        public static void Main()
        {
            StreamWriter writer = null;
            try
            {
                // Attempt to open output file.
                writer = new StreamWriter("log.txt");
                writer.AutoFlush = true;
                // Redirect standard output from the console to the output file.
                Console.SetOut(writer);
            }
            catch (IOException e)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine(e.Message);
            }

            var testUser = new TestSuiteUser();
            testUser.RunAllTests();

            if (writer != null)
            {
                writer.Close();
                writer = null;
            }
        }
    }
}