﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Manager {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class AuditEventsFile {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AuditEventsFile() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Manager.AuditEventsFile", typeof(AuditEventsFile).Assembly);
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
        ///   Looks up a localized string similar to Adding entity failed. Reason {0}..
        /// </summary>
        internal static string AddEntityFailed {
            get {
                return ResourceManager.GetString("AddEntityFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Entity successfully added..
        /// </summary>
        internal static string AddEntitySuccess {
            get {
                return ResourceManager.GetString("AddEntitySuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Creating database {0} failed. Reason {1}..
        /// </summary>
        internal static string CreateDBFailed {
            get {
                return ResourceManager.GetString("CreateDBFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database {0} successfully created..
        /// </summary>
        internal static string CreateDBSuccess {
            get {
                return ResourceManager.GetString("CreateDBSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deleting database {0} failed. Reason {1}..
        /// </summary>
        internal static string DeleteDBFailed {
            get {
                return ResourceManager.GetString("DeleteDBFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database {0} successfully deleted..
        /// </summary>
        internal static string DeleteDBSuccess {
            get {
                return ResourceManager.GetString("DeleteDBSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Updating entity failed. Reason {0}..
        /// </summary>
        internal static string UpdateEntityFailed {
            get {
                return ResourceManager.GetString("UpdateEntityFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Entity successfully updated..
        /// </summary>
        internal static string UpdateEntitySuccess {
            get {
                return ResourceManager.GetString("UpdateEntitySuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User {0} successfully authenticated..
        /// </summary>
        internal static string UserAuthenticationSuccess {
            get {
                return ResourceManager.GetString("UserAuthenticationSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Authorization failed. User {0} failed to access {1}. Reason {2}..
        /// </summary>
        internal static string UserAuthorizationFailed {
            get {
                return ResourceManager.GetString("UserAuthorizationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User {0} successfully accessed to {1}..
        /// </summary>
        internal static string UserAuthorizationSuccess {
            get {
                return ResourceManager.GetString("UserAuthorizationSuccess", resourceCulture);
            }
        }
    }
}
