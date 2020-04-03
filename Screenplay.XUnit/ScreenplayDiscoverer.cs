using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Screenplay.XUnit
{
    internal class ScreenplayDiscoverer : FactDiscoverer
    {
        private readonly IMessageSink _diagnosticMessageSink;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenplayDiscoverer"/> class.
        /// </summary>
        /// <param name="diagnosticMessageSink">The message sink used to send diagnostic messages</param>
        public ScreenplayDiscoverer(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        protected override IXunitTestCase CreateTestCase(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            return new ScenarioTestCase(_diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod);
        }

        /// <summary>
        /// Discover test cases from a test method.
        /// </summary>
        /// <param name="discoveryOptions">The discovery options to be used.</param>
        /// <param name="testMethod">The test method the test cases belong to.</param>
        /// <param name="factAttribute">The fact attribute attached to the test method.</param>
        /// <returns>Returns zero or more test cases represented by the test method.</returns>
        public override IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            if (testMethod.Method.GetParameters().Count() == 0)
            {
                return base.Discover(discoveryOptions, testMethod, factAttribute);
            }

            return new[] { CreateTestCase(discoveryOptions, testMethod, factAttribute) };
        }
    }
}
