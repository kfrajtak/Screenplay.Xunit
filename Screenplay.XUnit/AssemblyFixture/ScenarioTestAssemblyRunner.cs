using Screenplay.XUnit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Screenplay.XUnit.AssemblyFixture
{
    /// <summary>
    /// 
    /// </summary>
    public class ScenarioTestAssemblyRunner : XunitTestAssemblyRunner
    {
        private readonly Dictionary<Type, object> assemblyFixtureMappings = new Dictionary<Type, object>();
        private readonly IntegrationReader integrationReader = new IntegrationReader();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testAssembly"></param>
        /// <param name="testCases"></param>
        /// <param name="diagnosticMessageSink"></param>
        /// <param name="executionMessageSink"></param>
        /// <param name="executionOptions"></param>
        public ScenarioTestAssemblyRunner(ITestAssembly testAssembly,
                                                          IEnumerable<IXunitTestCase> testCases,
                                                          IMessageSink diagnosticMessageSink,
                                                          IMessageSink executionMessageSink,
                                                          ITestFrameworkExecutionOptions executionOptions)
            : base(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task AfterTestAssemblyStartingAsync()
        {
            // Let everything initialize
            await base.AfterTestAssemblyStartingAsync();

            var integration = integrationReader.GetIntegration(((ReflectionAssemblyInfo)TestAssembly.Assembly).Assembly);
            integration.BeforeExecutingFirstScenario();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Task BeforeTestAssemblyFinishedAsync()
        {
            var integration = integrationReader.GetIntegration(((ReflectionAssemblyInfo)TestAssembly.Assembly).Assembly);
            integration.AfterExecutedLastScenario();

            return base.BeforeTestAssemblyFinishedAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageBus"></param>
        /// <param name="testCollection"></param>
        /// <param name="testCases"></param>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        protected override Task<RunSummary> RunTestCollectionAsync(IMessageBus messageBus,
                                                                   ITestCollection testCollection,
                                                                   IEnumerable<IXunitTestCase> testCases,
                                                                   CancellationTokenSource cancellationTokenSource)
            => new ScenarioTestCollectionRunner(assemblyFixtureMappings, testCollection, testCases, DiagnosticMessageSink, messageBus, TestCaseOrderer, new ExceptionAggregator(Aggregator), cancellationTokenSource).RunAsync();
    }
}
