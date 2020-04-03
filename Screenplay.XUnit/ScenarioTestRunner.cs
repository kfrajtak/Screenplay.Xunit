using CSF.Screenplay.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Screenplay.XUnit
{
    /// <summary>
    /// 
    /// </summary>
    public class ScenarioTestRunner : XunitTestRunner
    {
        public ScenarioTestRunner(XunitTest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator exceptionAggregator, CancellationTokenSource cancellationTokenSource)
            : base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason, beforeAfterAttributes, exceptionAggregator, cancellationTokenSource) { }

        protected override Task<decimal> InvokeTestMethodAsync(ExceptionAggregator aggregator)
        {
            if (!TestMethod.GetParameters().Any())
            {
                return base.InvokeTestMethodAsync(aggregator);
            }

            return new TestInvoker(Test, MessageBus, TestClass, ConstructorArguments, TestMethod, TestMethodArguments, BeforeAfterAttributes, aggregator, CancellationTokenSource).RunAsync();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TestInvoker : XunitTestInvoker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="test"></param>
        /// <param name="messageBus"></param>
        /// <param name="testClass"></param>
        /// <param name="constructorArguments"></param>
        /// <param name="testMethod"></param>
        /// <param name="testMethodArguments"></param>
        /// <param name="beforeAfterAttributes"></param>
        /// <param name="aggregator"></param>
        /// <param name="cancellationTokenSource"></param>
        public TestInvoker(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
            : base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, beforeAfterAttributes, aggregator, cancellationTokenSource) { }

        private readonly IntegrationReader integrationReader = new IntegrationReader();

        /// <summary>
        /// Performs actions after each test.
        /// </summary>
        /// <param name="test">Test.</param>
        private void AfterTest(ITest test)
        {
            var scenario = ScenarioAdapter.GetScenario(test);
            var outcome = !Aggregator.HasExceptions;// test.TestCase. GetOutcome(test);
            var integration = integrationReader.GetIntegration(test);
            integration.AfterScenario(scenario, outcome);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Task AfterTestMethodInvokedAsync()
        {
            AfterTest(Test);
            return base.AfterTestMethodInvokedAsync();
        }
    }
}
