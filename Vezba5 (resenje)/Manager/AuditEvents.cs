using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Manager
{
    public enum AuditEventTypes
    {
        UserAuthenticationSuccess = 0,
        UserAuthorizationSuccess = 1,
        UserAuthorizationFailed = 2,
        CreateDBSuccess = 3,
        CreateDBFailed = 4,
        DeleteDBSuccess = 5,
        DeleteDBFailed = 6,
        AddEntitySuccess = 7,
        AddEntityFailed = 8,
        UpdateEntitySuccess = 9,
        UpdateEntityFailed = 10
    }

    public class AuditEvents
    {
        private static ResourceManager resourceManager = null;
            private static object resourceLock = new object();

            private static ResourceManager ResourceMgr
            {
                get
                {
                    lock (resourceLock)
                    {
                        if (resourceManager == null)
                        {
                            resourceManager = new ResourceManager(typeof(AuditEventsFile).FullName, Assembly.GetExecutingAssembly());
                        }

                        return resourceManager;
                    }
                }
            }

            public static string UserAuthenticationSuccess
            {
                get
                {
                    return ResourceMgr.GetString(AuditEventTypes.UserAuthenticationSuccess.ToString());
                }
            }

            public static string UserAuthorizationFailed
            {
                get
                {
                    return ResourceMgr.GetString(AuditEventTypes.UserAuthorizationFailed.ToString());
                }
            }

            public static string UserAuthorizationSuccess
            {
                get
                {
                    return ResourceMgr.GetString(AuditEventTypes.UserAuthorizationSuccess.ToString());
                }
            }

        public static string CreateDBSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.CreateDBSuccess.ToString());
            }
        }

        public static string CreateDBFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.CreateDBFailed.ToString());
            }
        }

        public static string DeleteDBSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.DeleteDBSuccess.ToString());
            }
        }

        public static string DeleteDBFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.DeleteDBFailed.ToString());
            }
        }

        public static string AddEntitySuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AddEntitySuccess.ToString());
            }
        }

        public static string AddEntityFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AddEntityFailed.ToString());
            }
        }

        public static string UpdateEntitySuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.UpdateEntitySuccess.ToString());
            }
        }

        public static string UpdateEntityFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.UpdateEntityFailed.ToString());
            }
        }
    }
    }
