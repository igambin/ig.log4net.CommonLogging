using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ig.log4net.Settings
{
    public enum SitenameComponent
    {
        [DefaultValue("app")]
        AppSystem,
        [DefaultValue("local")]
        AppEnvKey,
        [DefaultValue("undefined")]
        AppType,
        [DefaultValue("undefined")]
        AppRegion,
        [DefaultValue("unnamed")]
        AppName
    }

    public static class SitenameComponentExtensions
    {

        public static T GetDefaultValue<T>(this SitenameComponent enumValue)
        {
            var type = typeof(SitenameComponent);
            var member = type.GetMember(enumValue.ToString());
            if (member[0].GetCustomAttribute(typeof(DefaultValueAttribute)) is DefaultValueAttribute attribute)
            {
                return (T)attribute.Value;
            }
            return default(T);
        }
    }

    public class SitenameParser
    {
        private static readonly Regex SiteNamePattern = new Regex($"^(?<{SitenameComponent.AppSystem}>.+?)-(?<{SitenameComponent.AppEnvKey}>.+?)-(?<{SitenameComponent.AppType}>.+?)-(?<{SitenameComponent.AppRegion}>.+?)-(?<{SitenameComponent.AppName}>.+)$");

        /// <summary>
        /// Evaluates the Sitename and returns the specified component
        /// </summary>
        /// <param name="component">the component that shall be returned</param>
        /// <param name="sitename">the sitename that is to be evaluated. If not specified the method will read the EnvironmentVariable "WEBSITE_SITE_NAME" which will be set when running on azure.</param>
        /// <returns>the specified component of the sitename </returns>
        public static string EvaluateSitename(SitenameComponent component, string sitename = null)
        {
            var result = component.GetDefaultValue<string>();

            sitename = sitename ?? SettingsReader.ApplicationSettings.WebSiteName;
            Match m = SiteNamePattern.Match(sitename);
            if (m.Success)
            {
                result = m.Groups[component.ToString()].Value;
            }

            return result;
        }

        public static Dictionary<SitenameComponent, string> EvaluateSitename(string sitename = null)
        {
            var result = new Dictionary<SitenameComponent, string>();

            sitename = sitename ?? SettingsReader.ApplicationSettings.WebSiteName;
            Match m = SiteNamePattern.Match(sitename);
            if (m.Success)
            {
                foreach (var enm in Enum.GetValues(typeof(SitenameComponent)).OfType<SitenameComponent>())
                {
                    result.Add(enm, m.Groups[enm.ToString()].Value);
                }
            }

            return result;
        }
    }
}
