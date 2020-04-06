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
    public class ScenarioTestCollectionRunner : XunitTestCollectionRunner
    {
        readonly Dictionary<Type, object> assemblyFixtureMappings;
        readonly IMessageSink diagnosticMessageSink;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyFixtureMappings"></param>
        /// <param name="testCollection"></param>
        /// <param name="testCases"></param>
        /// <param name="diagnosticMessageSink"></param>
        /// <param name="messageBus"></param>
        /// <param name="testCaseOrderer"></param>
        /// <param name="aggregator"></param>
        /// <param name="cancellationTokenSource"></param>
        public ScenarioTestCollectionRunner(Dictionary<Type, object> assemblyFixtureMappings,
                                                            ITestCollection testCollection,
                                                            IEnumerable<IXunitTestCase> testCases,
                                                            IMessageSink diagnosticMessageSink,
                                                            IMessageBus messageBus,
                                                            ITestCaseOrderer testCaseOrderer,
                                                            ExceptionAggregator aggregator,
                                                            CancellationTokenSource cancellationTokenSource)
            : base(testCollection, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource)
        {
            this.assemblyFixtureMappings = assemblyFixtureMappings;
            this.diagnosticMessageSink = diagnosticMessageSink;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testClass"></param>
        /// <param name="class"></param>
        /// <param name="testCases"></param>
        /// <returns></returns>
        protected override Task<RunSummary> RunTestClassAsync(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases)
        {
            // Don't want to use .Concat + .ToDictionary because of the possibility of overriding types,
            // so instead we'll just let collection fixtures override assembly fixtures.
            var combinedFixtures = new Dictionary<Type, object>(assemblyFixtureMappings);
            foreach (var kvp in CollectionFixtureMappings)
            {
                combinedFixtures[kvp.Key] = kvp.Value;
            }

            // We've done everything we need, so let the built-in types do the rest of the heavy lifting
            return new XunitTestClassRunner(testClass, @class, testCases, diagnosticMessageSink, MessageBus, TestCaseOrderer, new ExceptionAggregator(Aggregator), CancellationTokenSource, combinedFixtures).RunAsync();
        }
    }
}
