using System.Collections.Generic;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Screenplay.XUnit.AssemblyFixture
{
    /// <summary>
    /// 
    /// </summary>
    public class ScenarioTestFrameworkExecutor : XunitTestFrameworkExecutor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="sourceInformationProvider"></param>
        /// <param name="diagnosticMessageSink"></param>
        public ScenarioTestFrameworkExecutor(AssemblyName assemblyName, ISourceInformationProvider sourceInformationProvider, IMessageSink diagnosticMessageSink)
            : base(assemblyName, sourceInformationProvider, diagnosticMessageSink) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testCases"></param>
        /// <param name="executionMessageSink"></param>
        /// <param name="executionOptions"></param>
        protected override async void RunTestCases(IEnumerable<IXunitTestCase> testCases, IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions)
        {
            using (var assemblyRunner = new ScenarioTestAssemblyRunner(TestAssembly, testCases, DiagnosticMessageSink, executionMessageSink, executionOptions))
            {
                await assemblyRunner.RunAsync();
            }
        }
    }
}
