using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Screenplay.XUnit.AssemblyFixture
{
    /// <summary>
    /// 
    /// </summary>
    public class ScenarioTestFramework : XunitTestFramework
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageSink"></param>
        public ScenarioTestFramework(IMessageSink messageSink) : base(messageSink) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
            => new ScenarioTestFrameworkExecutor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
    }
}
