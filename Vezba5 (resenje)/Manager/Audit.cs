using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Manager
{
    public class Audit : IDisposable
    {
        private static EventLog customLog = null;
        const string SourceName = "Manager.Audit";
        const string LogName = "DataKeep";          //naziv naseg Log fajla u Windows Event Log

        static Audit()
        {
            try
            {
                //pravi se customLog handle
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }

                customLog = new EventLog(LogName, Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }

        public static void AuthenticationSuccess(string userName)
        {
            if (customLog != null)
            {
                //poziva metodu AuditEvents da bi ispisao poruku u Log fajl
                customLog.WriteEntry(string.Format(AuditEvents.UserAuthenticationSuccess, userName), EventLogEntryType.SuccessAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event with id {0} to event log", (int)AuditEventTypes.UserAuthenticationSuccess));
            }
        }

        public static void AuthorizationSuccess(string userName, string serviceName)
        {
            if (customLog != null)
            {
                //poziva metodu AuditEvents da bi ispisao poruku u Log fajl
                customLog.WriteEntry(string.Format(AuditEvents.UserAuthorizationSuccess,userName,serviceName),EventLogEntryType.SuccessAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event with id {0} to event log", (int)AuditEventTypes.UserAuthorizationSuccess));
            }
        }

        public static void AuthorizationFailed(string userName, string serviceName, string reason)
        {
            if (customLog != null)
            {
                //poziva metodu AuditEvents da bi ispisao poruku u Log fajl
                customLog.WriteEntry(string.Format(AuditEvents.UserAuthorizationFailed,userName,serviceName,reason), EventLogEntryType.Error);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event with id {0} to event log", (int)AuditEventTypes.UserAuthorizationFailed));
            }
        }

        public static void CreateSuccess(string dbName)
        {
            if (customLog != null)
            {
                //poziva metodu AuditEvents da bi ispisao poruku u Log fajl
                customLog.WriteEntry(string.Format(AuditEvents.CreateDBSuccess, dbName), EventLogEntryType.SuccessAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event with id {0} to event log", (int)AuditEventTypes.CreateDBSuccess));
            }
        }

        public static void CreateFailed(string dbName,string reason)
        {
            if (customLog != null)
            {
                //poziva metodu AuditEvents da bi ispisao poruku u Log fajl
                customLog.WriteEntry(string.Format(AuditEvents.CreateDBFailed, dbName, reason), EventLogEntryType.Error);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event with id {0} to event log", (int)AuditEventTypes.CreateDBFailed));
            }
        }

        public static void DeleteSuccess(string dbName)
        {
            if (customLog != null)
            {
                //poziva metodu AuditEvents da bi ispisao poruku u Log fajl
                customLog.WriteEntry(string.Format(AuditEvents.DeleteDBSuccess, dbName), EventLogEntryType.SuccessAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event with id {0} to event log", (int)AuditEventTypes.DeleteDBSuccess));
            }
        }

        public static void DeleteFailed(string dbName,string reason)
        {
            if (customLog != null)
            {
                //poziva metodu AuditEvents da bi ispisao poruku u Log fajl
                customLog.WriteEntry(string.Format(AuditEvents.DeleteDBFailed, dbName, reason), EventLogEntryType.Error);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event with id {0} to event log", (int)AuditEventTypes.DeleteDBFailed));
            }
        }

        public static void AddSuccess()
        {
            if (customLog != null)
            {
                //poziva metodu AuditEvents da bi ispisao poruku u Log fajl
                customLog.WriteEntry(string.Format(AuditEvents.AddEntitySuccess), EventLogEntryType.SuccessAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event with id {0} to event log", (int)AuditEventTypes.AddEntitySuccess));
            }
        }

        public static void AddFailed(string reason)
        {
            if (customLog != null)
            {
                //poziva metodu AuditEvents da bi ispisao poruku u Log fajl
                customLog.WriteEntry(string.Format(AuditEvents.AddEntityFailed, reason), EventLogEntryType.Error);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event with id {0} to event log", (int)AuditEventTypes.AddEntityFailed));
            }
        }

        public static void UpdateSuccess()
        {
            if (customLog != null)
            {
                //poziva metodu AuditEvents da bi ispisao poruku u Log fajl
                customLog.WriteEntry(string.Format(AuditEvents.UpdateEntitySuccess), EventLogEntryType.SuccessAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event with id {0} to event log", (int)AuditEventTypes.UpdateEntitySuccess));
            }
        }

        public static void UpdateFailed(string reason)
        {
            if (customLog != null)
            {
                //poziva metodu AuditEvents da bi ispisao poruku u Log fajl
                customLog.WriteEntry(string.Format(AuditEvents.UpdateEntityFailed, reason), EventLogEntryType.Error);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event with id {0} to event log", (int)AuditEventTypes.UpdateEntityFailed));
            }
        }

        public static void ReadSuccess(string dbName)
        {
            if (customLog != null)
            {
                //poziva metodu AuditEvents da bi ispisao poruku u Log fajl
                customLog.WriteEntry(string.Format(AuditEvents.ReadDBSuccess, dbName), EventLogEntryType.SuccessAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event with id {0} to event log", (int)AuditEventTypes.ReadDBSuccess));
            }
        }

        public static void ReadFailed(string dbName, string reason)
        {
            if (customLog != null)
            {
                //poziva metodu AuditEvents da bi ispisao poruku u Log fajl
                customLog.WriteEntry(string.Format(AuditEvents.ReadDBFailed, dbName, reason), EventLogEntryType.Error);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event with id {0} to event log", (int)AuditEventTypes.ReadDBFailed));
            }
        }

        public static void CertificateSuccess()
        {
            if (customLog != null)
            {
                //poziva metodu AuditEvents da bi ispisao poruku u Log fajl
                customLog.WriteEntry(string.Format(AuditEvents.CertificateSuccess), EventLogEntryType.SuccessAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event with id {0} to event log", (int)AuditEventTypes.CertificateSuccess));
            }
        }

        public static void CertificateFailed()
        {
            if (customLog != null)
            {
                //poziva metodu AuditEvents da bi ispisao poruku u Log fajl
                customLog.WriteEntry(string.Format(AuditEvents.CertificateFailed), EventLogEntryType.Error);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event with id {0} to event log", (int)AuditEventTypes.CertificateFailed));
            }
        }

        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}