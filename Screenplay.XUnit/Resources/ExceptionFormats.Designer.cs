﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Screenplay.XUnit.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionFormats {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionFormats() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Screenplay.XUnit.Resources.ExceptionFormats", typeof(ExceptionFormats).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All test methods decorated with `{0}&apos; must be contained within assemblies which are decorated with `{1}&apos;..
        /// </summary>
        internal static string AssemblyMustBeDecoratedWithScreenplay {
            get {
                return ResourceManager.GetString("AssemblyMustBeDecoratedWithScreenplay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The test method must be inside a compiled assembly..
        /// </summary>
        internal static string MethodMustHaveAnAssembly {
            get {
                return ResourceManager.GetString("MethodMustHaveAnAssembly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The test must specify a test method..
        /// </summary>
        internal static string TestMustHaveAMethod {
            get {
                return ResourceManager.GetString("TestMustHaveAMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The test must contain an instance of `0&apos; in its test properties..
        /// </summary>
        internal static string TestMustHaveAScenarioInProperties {
            get {
                return ResourceManager.GetString("TestMustHaveAScenarioInProperties", resourceCulture);
            }
        }
    }
}