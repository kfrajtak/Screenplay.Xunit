using CSF.Screenplay.Reporting;
using CSF.Screenplay.Reporting.Builders;
using CSF.Screenplay.Scenarios;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Screenplay.XUnit
{
    internal class ScenarioTestRunner : XunitTestRunner
    {
        private readonly IGetsReportModel reportModel;

        public ScenarioTestRunner(ITest test, IMessageBus messageBus, Type testClass, 
			object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments, 
			string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator, 
			CancellationTokenSource cancellationTokenSource) 
			: base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason, beforeAfterAttributes, aggregator, cancellationTokenSource)
        {
        }

		public IScenario Scenario { get; set; }

        protected override async Task<Tuple<decimal, string>> InvokeTestAsync(ExceptionAggregator aggregator)
        {
			string output = string.Empty;

			var testOutputHelper = TestMethodArguments.OfType<TestOutputHelper>().FirstOrDefault();
			if (testOutputHelper == null)
			{
				testOutputHelper = ConstructorArguments.OfType<TestOutputHelper>().FirstOrDefault();				
			}

			if (testOutputHelper == null)
            {
				testOutputHelper = new TestOutputHelper();
            }

			testOutputHelper?.Initialize(MessageBus, Test);

			decimal item = await InvokeTestMethodAsync(aggregator);
			if (testOutputHelper != null)
			{
				output = testOutputHelper.Output;

				var scenarioRunOutput = RenderScenarioReport(Scenario);
				if (scenarioRunOutput != null)
				{
					if (output.Length == 0)
                    {
						scenarioRunOutput = scenarioRunOutput.Trim();
					}

					output += scenarioRunOutput;
				}

				testOutputHelper.Uninitialize();
			}

			return Tuple.Create(item, output);
		}

		private string RenderScenarioReport(IScenario scenario)
        {
			if (scenario == null)
            {
				return null;
            }

			var reportModel = Scenario.DiContainer.TryResolve<IHandlesReportableEvents>() as IGetsReportModel;
			if (reportModel == null)
            {
				return null;
            }

			var builder = reportModel.GetType().BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(f => f.Name == "builder").GetValue(reportModel) as ReportBuilder;
			var scenarioBuilders = builder.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(f => f.Name == "scenarioBuilders").GetValue(builder) as ConcurrentDictionary<Guid, IBuildsScenario>;
			foreach (var b in scenarioBuilders)
			{
				if (b.Key == Scenario.Identity)
				{
					var integration = new IntegrationReader().GetIntegration(GetType().Assembly);

					var buildsScenario = (IBuildsScenario)builder.GetType().GetMethod("GetScenarioBuilder", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(builder, new object[] { Scenario.Identity });
					var reportFactory = (IGetsReport)builder.GetType().GetField("reportFactory", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(builder);
					var report = reportFactory.GetReport(new[] { buildsScenario });
					//var di = ir.GetIntegration(null).GetType().GetProperties(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).FirstOrDefault(f => f.Name == "builder").GetValue(this) as ReportBuilder;
					//d.TryResolve<ITestOutputHelper>()?.WriteLine(sb.ToString());
					//ir.GetIntegration().d
					//	ir.GetIntegration().
					//(b.Value.GetScenario() as CSF.Screenplay.Scenarios.Scenario).DiContainer.Resolve<ITestOutputHelper>
					var sb = new StringBuilder();
					using (var textWriter = new StringWriter(sb))
					{
						var textReportRenderer = new TextReportRenderer(textWriter, false);
						textReportRenderer.Render(report);
						return sb.ToString();
					}
				}
			}

			return null;
		}
    }
}
