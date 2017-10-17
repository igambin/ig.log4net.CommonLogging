using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ig.log4net.Caching;
using ig.log4net.Extensions;
using ig.log4net.UnitySetup;
using Microsoft.Practices.Unity;

namespace ig.log4net.Settings
{
    public partial class SettingsReader
    {
        public static ApplicationSettingsSet ApplicationSettings { get; }
        public static LoggingSettings Logging { get; }


        #region Singleton
        private static ICachable Cache { get; }
        public static SettingsReader GetInstance { get; } = new SettingsReader();

        static SettingsReader()
        {
            Cache = UnityConfig.Container.Resolve<ICachable>();
            ApplicationSettings = new ApplicationSettingsSet();
            Logging = new LoggingSettings();
        }

        private SettingsReader()
        {
        }

        #endregion

        #region Database-Reader

        private const string ConnectionStringName = "SettingsConnectionString";
        private const string AppSettingTableName = "AppSettingsTable";

        private T ReadDbSetting<T>(string key, T defaultValue = default(T)) where T : IConvertible
            => Cache.GetOrSet(key, () => ReadDbSettingLoader(key, defaultValue), TimeSpan.FromMinutes(1));

        private T ReadDbSettingLoader<T>(string key, T defaultValue) where T : IConvertible
        {
            if (ConfigurationManager.ConnectionStrings[ConnectionStringName] == null)
            {
                return defaultValue;
            }
            var connstring = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
            var value = defaultValue;
            using (var conn = new SqlConnection(connstring))
            {
                conn.Open();
                var cmd = new SqlCommand($"select [Value] from {AppSettingTableName} where [Key] = @key", conn);
                cmd.Parameters.AddWithValue("@key", key);
                var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    value = reader.GetString(0).GetTOrDefault(defaultValue);
                }
            }
            return value;
        }

        private IEnumerable<AppSetting> ReadMultipleDbAppSettings(string keyPrefix)
            => Cache.GetOrSet(keyPrefix, () => ReadMultipleDbAppSettingsLoader(keyPrefix), TimeSpan.FromMinutes(1));
        
        private IEnumerable<AppSetting> ReadMultipleDbAppSettingsLoader(string keyPrefix)
        {
            var result = new List<AppSetting>();
            if (ConfigurationManager.ConnectionStrings[ConnectionStringName] == null)
            {
                return result;
            }

            var connstring = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
            using (var conn = new SqlConnection(connstring))
            {
                conn.Open();
                var cmd = new SqlCommand($"select [Key], [Value] from {AppSettingTableName} where [Key] like @key", conn);
                cmd.Parameters.AddWithValue("@key", keyPrefix + "%");
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new AppSetting { Key = reader.GetString(0), Value = reader.GetString(1) });
                }
            }
            return result;
        }

        #endregion

        #region Config-Reader
        private T ReadCfgSetting<T>(string key, T defaultValue = default(T)) where T : IConvertible
            => Cache.GetOrSet(key, () => ReadCfgSettingLoader(key, defaultValue), TimeSpan.FromMinutes(1));
       

        private T ReadCfgSettingLoader<T>(string key, T defaultValue) where T : IConvertible
            =>  ConfigurationManager.AppSettings[key].GetTOrDefault(defaultValue);
        #endregion


    }
}
