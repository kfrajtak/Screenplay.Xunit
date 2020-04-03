using CSF.Screenplay.Scenarios;
using System;
using System.Collections.Generic;
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
	public class ScenarioTestCaseRunner : XunitTestCaseRunner
	{
		/*/// <summary>
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
		public ScenarioTestCaseRunner(ScenarioTestCase testCase, string displayName, string skipReason, object[] constructorArguments, object[] testMethodArguments, IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
			: base(testCase, displayName, skipReason, constructorArguments, testMethodArguments, messageBus, aggregator, cancellationTokenSource)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="test"></param>
		/// <param name="messageBus"></param>
		/// <param name="testClass"></param>
		/// <param name="constructorArguments"></param>
		/// <param name="testMethod"></param>
		/// <param name="testMethodArguments"></param>
		/// <param name="skipReason"></param>
		/// <param name="beforeAfterAttributes"></param>
		/// <param name="aggregator"></param>
		/// <param name="cancellationTokenSource"></param>
		/// <returns></returns>
		protected override XunitTestRunner CreateTestRunner(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
		{
			if (testMethod.GetParameters().Length == 0)
			{
				return base.CreateTestRunner(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason, beforeAfterAttributes, new ExceptionAggregator(aggregator), cancellationTokenSource); 
			}
			return new ScenarioTestRunner(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason, beforeAfterAttributes, new ExceptionAggregator(aggregator), cancellationTokenSource);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override Task<RunSummary> RunTestAsync()
		{
			System.IO.File.AppendAllText(@"C:\work\playground\c#\LpWAcceptanceTesting\out.txt", "Runner " + DisplayName + System.Environment.NewLine);
			return CreateTestRunner(
				CreateTest(TestCase, DisplayName), 
				MessageBus, TestClass, ConstructorArguments, TestMethod, TestMethodArguments, SkipReason, BeforeAfterAttributes, Aggregator, CancellationTokenSource).RunAsync();
		}*/

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
			: base(testCase, displayName, skipReason, constructorArguments, testMethodArguments, messageBus, aggregator, cancellationTokenSource) { }

		private readonly IntegrationReader integrationReader = new IntegrationReader();

		/*private IEnumerable<TestMethod> BuildFrom(IMethodInfo method, ITest suite)
		{
			var scenario = CreateScenario(method, suite);
			//suite.Properties.Add(ScenarioAdapter.ScreenplayScenarioKey, scenario);
			return BuildFrom(method, suite, scenario);
		}*/

		private IScenario CreateScenario(IMethodInfo method, ITest test)
		{
			var integration = integrationReader.GetIntegration(method);
			var adapter = new ScenarioAdapter(test, method, integration);
			return adapter.CreateScenario();
		}

		/*IEnumerable<TestMethod> BuildFrom(IMethodInfo method, ITest suite, IScenario scenario)
		{
			var builder = new NUnitTestCaseBuilder();
			var resolvedParameters = method.GetParameters()
										   .Select(p => scenario.DiContainer.TryResolve(p.ParameterType))
										   .ToArray();

			var tcParams = new TestCaseParameters(resolvedParameters);
			var testMethod = builder.BuildTestMethod(method, suite, tcParams);
			testMethod.Properties.Add(ScenarioAdapter.ScreenplayScenarioKey, scenario);

			return new[] { testMethod };
		}*/

		private void BeforeTest(ITest test)
		{
			if (TestCase is ScenarioTestCase scenarioTestCase)
			{
				if (scenarioTestCase != null)
				{
					scenarioTestCase.Scenario = CreateScenario(test.TestCase.TestMethod.Method, test);
				}

				var scenario = scenarioTestCase.Scenario ?? ScenarioAdapter.GetScenario(test);
				// .Traits.Add(ScenarioAdapter.ScreenplayScenarioKey, scenario);
				var integration = integrationReader.GetIntegration(test);
				integration.BeforeScenario(scenario);

				// resolve arguments
				//Enumerable.Range(0, TestMethod.GetParameters().Length).Select(a => (object)null).ToArray();
				TestMethodArguments = TestMethod.GetParameters()
					.Select(p => scenario.DiContainer.TryResolve(p.ParameterType))
					.ToArray();

				return;
			}

			throw new NotImplementedException("!!!");
		}

		protected override Task<RunSummary> RunTestAsync()
		{
			if (!TestMethod.GetParameters().Any())
			{
				return base.RunTestAsync();
			}

			var test = new XunitTest(TestCase, DisplayName);

			BeforeTest(test);

			return base.RunTestAsync();
			//return new ScenarioTestRunner(test, MessageBus, TestClass, ConstructorArguments, TestMethod, TestMethodArguments, SkipReason, BeforeAfterAttributes, new ExceptionAggregator(Aggregator), CancellationTokenSource).RunAsync();
		}
	}
}
