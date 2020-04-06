using System;
using System.Linq;
using System.Reflection;
using CSF.Screenplay.Integration;
using Xunit.Abstractions;

namespace Screenplay.XUnit
{
    /// <summary>
    /// Helper type which provides access to the current Screenplay integration.
    /// </summary>
    public class IntegrationReader
    {
        static object syncRoot;
        static IScreenplayIntegration integration;

        /// <summary>
        /// Gets the integration from a given NUnit test method.
        /// </summary>
        /// <returns>The integration.</returns>
        /// <param name="method">Method.</param>
        public IScreenplayIntegration GetIntegration(IMethodInfo method)
        {
            lock (syncRoot)
            {
                var assembly = ((IReflectionMethodInfo)method)?.MethodInfo.DeclaringType?.Assembly;
                return GetIntegration(assembly);
            }
        }

        public IScreenplayIntegration GetIntegration(Assembly assembly)
        {
            lock (syncRoot)
            {
                if (integration == null)
                {
                    if (assembly == null)
                    {
                        throw new ArgumentException("The test method must be inside a compiled assembly.");
                    }

                    var assemblyAttrib = assembly.GetCustomAttributes(typeof(ScreenplayAssemblyAttribute)).Cast<ScreenplayAssemblyAttribute>().FirstOrDefault();
                    if (assemblyAttrib == null)
                    {
                        var message = string.Format("All test methods decorated with `{0}` must be contained within assemblies which are decorated with `{1}`; ...",
                            nameof(ScreenplayAttribute), nameof(ScreenplayAssemblyAttribute));
                        throw new InvalidOperationException(message);
                    }

                    integration = assemblyAttrib.Integration;
                }
            }

            return integration;
        }

        /// <summary>
        /// Gets the integration from a given NUnit test instance.
        /// </summary>
        /// <returns>The integration.</returns>
        /// <param name="test">Test.</param>
        public IScreenplayIntegration GetIntegration(ITest test)
        {
            if (test.TestCase.TestMethod == null)
            {
                throw new ArgumentException("The test must specify a test method.", nameof(test));
            }

            return GetIntegration(test.TestCase.TestMethod.Method);
        }

        /// <summary>
        /// Initializes the <see cref="T:Screenplay.XUnit.IntegrationReader"/> class.
        /// </summary>
        static IntegrationReader()
        {
            syncRoot = new object();
        }
    }
}