using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HTTPlease.TestHarness
{
    /// <summary>
    /// Quick-and-dirty test harness for use when Visual Studio refuses to debug unit tests.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main program entry-point.
        /// </summary>
        static void Main()
        {
            Trace.Listeners.Add(
                new TextWriterTraceListener(Console.Out)
            );

            try
            {
                UriTemplate template = new UriTemplate(
                    "api/{controller}/{action}/{id?}/properties?propertyIds={propertyGroupIds}&diddly={dee?}&foo=bar"
                );

                Uri generatedUri = template.Populate(
                    baseUri: new Uri("http://test-host/"),
                    templateParameters: new Dictionary<string, string>
                    {
                        ["controller"] = "organizations",
                        ["action"] = "distinct",
                        ["dee"] = "hello",
                        ["propertyGroupIds"] = "System.OrganizationCommercial;EnterpriseMobility.OrganizationAirwatch"
                    }
                );

                Debug.WriteLine(generatedUri.AbsoluteUri);
            }
            catch (Exception unexpectedError)
            {
                Console.WriteLine(unexpectedError);
            }
        }
    }
}
