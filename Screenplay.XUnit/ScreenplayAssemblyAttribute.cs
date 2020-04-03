using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Screenplay.Integration;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Screenplay.XUnit
{
    /// <summary>
    /// Indicates that the assembly contains Screenplay tests.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class ScreenplayAssemblyAttribute : Attribute
    {
        static Lazy<IScreenplayIntegration> integration;

        /// <summary>
        /// Gets the current Screenplay integration.
        /// </summary>
        /// <value>The integration.</value>
        public IScreenplayIntegration Integration => integration.Value;

        /// <summary>
        /// Gets the targets for this attribute (the affected tests).
        /// </summary>
        /// <value>The targets.</value>
        //public override ActionTargets Targets => ActionTargets.Suite;

        /// <summary>
        /// Executes actions after any tests in the current assembly.
        /// </summary>
        /// <param name="test">Test.</param>
        /*public override void AfterTest(ITest test)
        {
            Integration.AfterExecutedLastScenario();
        }*/

        /// <summary>
        /// Executes actions before any tests in the current assembly.
        /// </summary>
        /// <param name="test">Test.</param>
        /*public override void BeforeTest(ITest test)
        {
            Integration.BeforeExecutingFirstScenario();
        }*/
        
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