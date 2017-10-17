using System;
using System.Configuration;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace ig.log4net.Settings
{
    public partial class SettingsReader
    {
        public enum Running
        {
            Azure, Vm, Local
        }
        
        public class ApplicationSettingsSet
        {
            public string System => SitenameParser.EvaluateSitename(SitenameComponent.AppSystem);

            public string Region => SitenameParser.EvaluateSitename(SitenameComponent.AppRegion);

            public string EnvKey => SitenameParser.EvaluateSitename(SitenameComponent.AppEnvKey);

            public string Type => SitenameParser.EvaluateSitename(SitenameComponent.AppType);

            public string Name => SitenameParser.EvaluateSitename(SitenameComponent.AppName);

            public string MachineName
            {
                get
                {
                    string name;
                    try
                    {
                        name = RoleEnvironment.DeploymentId;
                    }
                    catch (Exception)
                    {
                        name = Environment.MachineName;
                    }
                    return name;
                }
            }

            public string WebSiteName
            {
                get
                {
                    if (!string.IsNullOrEmpty(GetWebsiteSiteName()))
                    {
                        return GetWebsiteSiteName();
                    }
                    return "[WEBSITE_SITE_NAME undefined]";
                }
            }

            public string GetEnvironment() => $"Website: {WebSiteName}, Running on: {Mode}, Env: {EnvKey}";

            public static bool IsRunningOnAzure()
            {
                var x = GetWebsiteSiteName();
                return Mode == Running.Azure;
            }

            public static Running Mode { get; private set; } = Running.Local;

            public static string GetWebsiteSiteName()
            {
                var sitename = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");
                if (!string.IsNullOrEmpty(sitename))
                {
                    Mode = Running.Azure;
                    return sitename;
                }
                sitename = ConfigurationManager.AppSettings.Get("WEBSITE_SITE_NAME");
                if (!string.IsNullOrEmpty(sitename))
                {
                    if (sitename.Equals("local"))
                        Mode = Running.Local;
                    else
                        Mode = Running.Vm;
                    return sitename;
                }
                return string.Empty;
            }

        }

    }
}
