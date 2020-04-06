using CSF.Screenplay.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
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
    [Serializable]
    public class ScenarioTestCase : XunitTestCase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticMessageSink"></param>
        /// <param name="defaultMethodDisplay"></param>
        /// <param name="defaultMethodDisplayOptions"></param>
        /// <param name="testMethod"></param>
        /// <param name="testMethodArguments"></param>
        public ScenarioTestCase(IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay, TestMethodDisplayOptions defaultMethodDisplayOptions, ITestMethod testMethod, object[] testMethodArguments = null)
            : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, testMethodArguments)
        {
        }

        [Obsolete]
        public ScenarioTestCase() {}

        private readonly IntegrationReader integrationReader = new IntegrationReader();

        public IScenario Scenario { get; set; }

        private IScenario CreateScenario(IMethodInfo method, ITest test)
        {
            var integration = integrationReader.GetIntegration(method);
            var adapter = new ScenarioAdapter(test, method, integration);
            return adapter.CreateScenario();
        }

        private void BeforeTest(ITest test)
        {
            Scenario = CreateScenario(TestMethod.Method, test);

            var integration = integrationReader.GetIntegration(test);
            integration.BeforeScenario(Scenario);

            // resolve arguments
            //Enumerable.Range(0, TestMethod.GetParameters().Length).Select(a => (object)null).ToArray();
            TestMethodArguments = TestMethod.Method.GetParameters()
                .Select(p => Scenario.DiContainer.TryResolve(p.ParameterType.ToRuntimeType()))
                .ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticMessageSink"></param>
        /// <param name="messageBus"></param>
        /// <param name="constructorArguments"></param>
        /// <param name="aggregator"></param>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        public override Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            var test = new XunitTest(this, DisplayName);
            BeforeTest(test);

            return base.RunAsync(diagnosticMessageSink, messageBus, constructorArguments, aggregator, cancellationTokenSource)
                .ContinueWith(t =>
                {
                    var success = !aggregator.HasExceptions && t.Result.Failed == 0;
                    AfterTest(test, success);
                    return t.Result;
                });
        }

        private void AfterTest(ITest test, bool success)
        {
            var scenario = ScenarioAdapter.GetScenario(test);
            var integration = integrationReader.GetIntegration(test);
            integration.AfterScenario(scenario, success);
        }
    }
}
