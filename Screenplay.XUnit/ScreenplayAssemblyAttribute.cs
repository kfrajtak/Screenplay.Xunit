using System;
using CSF.Screenplay.Integration;
using Xunit.Sdk;

namespace Screenplay.XUnit
{
    /// <summary>
    /// Indicates that the assembly contains Screenplay tests.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    [TestFrameworkDiscoverer("Screenplay.XUnit.AssemblyFixture.ScenarioTestFrameworkDiscoverer", "Screenplay.XUnit")]
    public class ScreenplayAssemblyAttribute : Attribute, ITestFrameworkAttribute
    {
        static Lazy<IScreenplayIntegration> integration;

        /// <summary>
        /// Gets the current Screenplay integration.
        /// </summary>
        /// <value>The integration.</value>
        public IScreenplayIntegration Integration => integration.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Screenplay.XUnit.ScreenplayAssemblyAttribute"/> class.
        /// </summary>
        /// <param name="configType">Integration type.</param>
        public ScreenplayAssemblyAttribute(Type configType)
        {
            integration = integration ?? new Lazy<IScreenplayIntegration>(() => new IntegrationFactory().Create(configType));
        }        
    }
}