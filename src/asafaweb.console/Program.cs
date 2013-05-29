using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NDesk.Options;
using asafaweb.console.Logic;

namespace asafaweb.console
{
    class Program
    {
        private static string _url;
        private static bool _validParams = true;
        private static bool _throwExceptionOnFail = false;
        static void Main(string[] args)
        {
            StatusLogic logic = LoadParams(args);
            if (_validParams)
            {
                Console.Write("Selected failures: ");
                if (logic.FailOnFailure)
                {
                    Console.Write("Failures ");
                }
                if (logic.FailOnWarning)
                {
                    Console.Write("Warnings ");
                }
                if (logic.FailOnNotTested)
                {
                    Console.Write("Not Tested");
                }
                if (!logic.FailOnNotTested && !logic.FailOnWarning && !logic.FailOnFailure)
                {
                    Console.Write("None, this will always pass");
                }
                Console.WriteLine();
                if (logic.IgnoredTests.Count > 0)
                {
                    Console.WriteLine("Ignored Tests: {0}", logic.IgnoredTests.Aggregate((i, j) => i + ", " + j));
                }
                Console.WriteLine("Scanning {0}", _url);
                var results = logic.AnalyseResults(StatusLogic.GetTestResults(_url));
                if (results.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("ERROR DETECTED:");
                    foreach (var asafaResult in results)
                    {
                        sb.AppendLine(string.Format("The {0} test has failed with the status {1}", asafaResult.Key, asafaResult.Value));
                    }
                    sb.AppendLine();
                    sb.AppendLine(string.Format("For more information visit https://asafaweb.com/Scan?Url={0}", HttpUtility.UrlEncode(_url)));
                    Console.WriteLine(sb.ToString());
                    if (_throwExceptionOnFail)
                    {
                        throw new Exception(sb.ToString());
                    }
                }
                Console.WriteLine("No errors found");
            }
        }

        private static StatusLogic LoadParams(IEnumerable<string> args)
        {
            bool showHelp = false;
            bool failonerror = false;
            bool failonwarning = false;
            bool failonnottested = false;
            List<string> ignoredTests = new List<string>();
            var p = new OptionSet
                {
                    {
                        "u|url=", "The url to test",
                        s => _url = s
                    },
                    {
                        "f|failonfailures", "Fail the test if any failures occur",
                        v => { if (v != null) failonerror = true; }
                    },
                    {
                        "w|failonwarning", "Fail the test if any warnings occur",
                        v => { if (v != null) failonwarning = true; }
                    },
                    {
                        "n|failonnottested", "Fail the test if any tests arn't completed",
                        v => { if (v != null) failonnottested = true; }
                    },
                    {
                        "x|throwexceptiononfail", "Throws an exception if the tests fail",
                        v => { if (v != null) _throwExceptionOnFail = true; }
                    },
                    {
                        "i|ignore=", "A comma seperated list of {TESTS} to ignore. Tests include:\nTracing\nCustomErrors\nStackTrace\nRequestValidation\nHttpToHttps\nHashDosPatch\nElmahLog\nExcessiveHeaders\nHttpOnlyCookies\nSecureCookies\nClickjacking",
                        i => { if (!string.IsNullOrEmpty(i)) ignoredTests.AddRange(i.Split(',')); }
                    },
                    {
                        "h|help", "show this message and exit",
                        v => showHelp = v != null
                    }
                };

            List<string> extra;
            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException e)
            {
                _validParams = false;
                Console.Write("error: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `--help' for more information.");
                return new StatusLogic();
            }

            if (showHelp)
            {
                _validParams = false;
                ShowHelp(p);
                return new StatusLogic();
            }

            if (extra.Count > 0)
            {
                _validParams = false;
                string message = string.Join(" ", extra.ToArray());
                Console.WriteLine("Unexpected parameters: {0}", message);
            }

            if (string.IsNullOrEmpty(_url))
            {
                _validParams = false;
                Console.WriteLine("No URL provided. Please provide with the option -url. Try -help for more info...");
            }

            return new StatusLogic
                {
                    FailOnFailure = failonerror,
                    FailOnNotTested = failonnottested,
                    FailOnWarning = failonwarning,
                    IgnoredTests = ignoredTests
                };
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: asafaweb.console [OPTIONS]+");
            Console.WriteLine("Scans the url provided against the security tests defined on www.asafaweb.com.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
