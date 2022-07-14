using CSF.FlexDi;
using CSF.Screenplay.Scenarios;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Screenplay.XUnit
{
	/// <summary>
	/// 
	/// </summary>
    internal class ScenarioTestCaseRunner : XunitTestCaseRunner
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="testCase"></param>
		/// <param name="displayName"></param>
		/// <param name="skipReason"></param>
		/// <param name="constructorArguments"></param>
		/// <param name="testMethodArguments"></param>
		/// <param name="messageBus"></param>
		/// <param name="aggregator"></param>
		/// <param name="cancellationTokenSource"></param>
		public ScenarioTestCaseRunner(IXunitTestCase testCase, string displayName, string skipReason, object[] constructorArguments, object[] testMethodArguments, IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
			: base(testCase, displayName, skipReason, constructorArguments, testMethodArguments, messageBus, aggregator, cancellationTokenSource)
		{
			DisplayName = displayName;
			SkipReason = skipReason;
			ConstructorArguments = constructorArguments;
			TestClass = base.TestCase.TestMethod.TestClass.Class.ToRuntimeType();
			TestMethod = base.TestCase.Method.ToRuntimeMethod();
			ParameterInfo[] parameters = TestMethod.GetParameters();
			Type[] parameterTypes = new Type[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				parameterTypes[i] = parameters[i].ParameterType;
			}
			TestMethodArguments = Reflector.ConvertArguments(testMethodArguments, parameterTypes);
		}

        protected override ITest CreateTest(IXunitTestCase testCase, string displayName)
        {
            var test = base.CreateTest(testCase, displayName);
			BeforeTest(test, testCase);
			return test;
        }

        private readonly IntegrationReader integrationReader = new IntegrationReader();

		private IScenario Scenario { get; set; }

		private IScenario CreateScenario(IMethodInfo method, ITest test)
		{
			var integration = integrationReader.GetIntegration(method);
			var adapter = new ScenarioAdapter(test, method, integration);
			return adapter.CreateScenario();
		}

		private void BeforeTest(ITest test, IXunitTestCase testCase)
		{
			Scenario = CreateScenario(testCase.Method, test);

			var integration = integrationReader.GetIntegration(test);
			integration.BeforeScenario(Scenario);

			// resolve arguments
			TestMethodArguments = testCase.Method.GetParameters()
				.Select(p => Scenario.DiContainer.TryResolve(p.ParameterType.ToRuntimeType()) ?? TryResolve(p, Scenario.DiContainer))
				.ToArray();
		}

		private void AfterTest(ITest test, bool success)
		{
			var scenario = ScenarioAdapter.GetScenario(test);
			var integration = integrationReader.GetIntegration(test);
			integration.AfterScenario(scenario, success);			
		}

		private object TryResolve(IParameterInfo parameter, IContainer container)
		{
			if (parameter.ParameterType.ToRuntimeType() == typeof(ITestOutputHelper))
			{
				if (container.TryResolve<ITestOutputHelper>(out var helper))
				{
					return helper;
				}

				return new TestOutputHelper();
			}

			return null;
		}

        protected override Task<RunSummary> RunTestAsync()
        {
			var test = CreateTest(TestCase, DisplayName);
			((ScenarioTestCase)TestCase).Scenario = Scenario;

			return new ScenarioTestRunner(
				test, MessageBus, TestClass, 
				ConstructorArguments, TestMethod, TestMethodArguments, 
				SkipReason, BeforeAfterAttributes, 
				Aggregator, CancellationTokenSource)
            {
				Scenario = Scenario
            }
				.RunAsync()
				.ContinueWith(t =>
				{
					var success = !Aggregator.HasExceptions && t.Result.Failed == 0;
					AfterTest(test, success);
					return t.Result;
				});
        }
    }
}
