using System;
using Xunit.Sdk;

namespace Screenplay.XUnit
{
    /// <summary>
    /// Applied totest this indicates that a test is Screenplay test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("Screenplay.XUnit.ScreenplayDiscoverer", "Screenplay.XUnit")] 
    public class ScreenplayAttribute : Xunit.FactAttribute
    {
    }
}