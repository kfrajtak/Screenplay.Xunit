using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using CSF.Screenplay.Integration;
using CSF.Screenplay.Scenarios;
using Xunit.Abstractions;

namespace Screenplay.XUnit
{
    /// <summary>
    /// Adapter type which provides a simple API for getting feature/scenario identification information.
    /// </summary>
    public class ScenarioAdapter
    {
        readonly ITest featureSuite;
        readonly IMethodInfo scenarioMethod;
        readonly IScreenplayIntegration integration;

        /// <summary>
        /// Gets the name of the scenario.
        /// </summary>
        /// <value>The name of the scenario.</value>
        public string ScenarioName => GetDescription(((IReflectionMethodInfo)scenarioMethod).MethodInfo);

        /// <summary>
        /// Gets the scenario identifier.
        /// </summary>
        /// <value>The scenario identifier.</value>
        public string ScenarioId => $"{FeatureId}.{scenarioMethod.Name}";

        /// <summary>
        /// Gets the name of the feature.
        /// </summary>
        /// <value>The name of the feature.</value>
        public string FeatureName
        {
            get
            {
                var fixtureType = ((IReflectionMethodInfo)scenarioMethod).MethodInfo.DeclaringType;
                if (fixtureType == null)
                {
                    return null;
                }

                return GetDescription(fixtureType);
            }
        }

        /// <summary>
        /// Gets the feature identifier.
        /// </summary>
        /// <value>The feature identifier.</value>
        public string FeatureId => featureSuite.GetHashCode().ToString();// .FullName;

        /// <summary>
        /// Creates a new Screenplay scenario using the state of the current instance, and the given integration.
        /// </summary>
        /// <returns>The scenario.</returns>
        public IScenario CreateScenario()
        {
            var factory = integration.GetScenarioFactory();
            return factory.GetScenario(FeatureIdAndName, ScenarioIdAndName);
        }

        private string GetDescription(MemberInfo member)
        {
            var attrib = member.GetCustomAttribute<DescriptionAttribute>();
            if (attrib == null)
            {
                return member.Name;
            }

            var prop = attrib.Description;
            if (prop == null)
            {
                return member.Name;
            }

            return prop.ToString();
        }

        private string GetDescription(IMethodInfo member)
        {
            var attrib = member.GetCustomAttributes(typeof(DescriptionAttribute)).Cast<DescriptionAttribute>().FirstOrDefault();
            if (attrib == null)
            {
                return member.Name;
            }

            var prop = attrib.Description;
            if (prop == null)
            {
                return member.Name;
            }

            return prop.ToString();
        }

        IdAndName FeatureIdAndName => new IdAndName(FeatureId, FeatureName);

        IdAndName ScenarioIdAndName => new IdAndName(ScenarioId, ScenarioName);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Screenplay.XUnit.ScenarioAdapter"/> class.
        /// </summary>
        /// <param name="featureSuite">Feature suite.</param>
        /// <param name="scenarioMethod">Scenario method.</param>
        /// <param name="integration">Screenplay integration.</param>
        public ScenarioAdapter(ITest featureSuite, IMethodInfo scenarioMethod, IScreenplayIntegration integration)
        {
            this.scenarioMethod = scenarioMethod ?? throw new ArgumentNullException(nameof(scenarioMethod));
            this.featureSuite = featureSuite ?? throw new ArgumentNullException(nameof(featureSuite));
            this.integration = integration ?? throw new ArgumentNullException(nameof(integration));
        }
        
        /// <summary>
        /// Gets an <see cref="IScenario"/> from a given NUnit test instance.
        /// </summary>
        /// <returns>The scenario.</returns>
        /// <param name="test">Test.</param>
        public static IScenario GetScenario(ITest test)
        {
            if (test.TestCase is ScenarioTestCase scenarioTestCase)
            {
                var scenario = scenarioTestCase.Scenario;
                if (scenario != null)
                {
                    return scenario;
                }
            }

            var message = String.Format("The test must contain an instance of {0}; in its test properties ...", nameof(IScenario));
            throw new ArgumentException(message, nameof(test));
        }

        /// <summary>
        /// Gets an <see cref="IScenario"/> from the arguments to an NUnit test method.
        /// </summary>
        /// <returns>The scenario.</returns>
        /// <param name="methodArguments">Method arguments.</param>
        public static IScenario GetScenario(object[] methodArguments)
        {
            return (IScenario)methodArguments.FirstOrDefault(x => x is IScenario);
        }
    }
}