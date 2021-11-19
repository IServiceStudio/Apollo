using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Core;
using Com.Ctrip.Framework.Apollo.Model;
using System;
using System.Threading;

namespace ConsoleApp_Framework
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GetApolloValue();
        }

        #region 获取默认Namespace的Value

        /// <summary>
        /// 获取默认Namespace的Value
        /// </summary>
        private static void GetApolloValue()
        {
            IConfig config = ApolloConfigurationManager.GetAppConfig().Result;

            var connectionString = "ConnectionString";
            while (true)
            {
                var connectionStringValue = config.GetProperty(connectionString, "127.0.0.1");
                Console.WriteLine($"Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},ConnectionStringValue:{connectionStringValue}");

                Thread.Sleep(1000);
            }
        }

        #endregion

        #region 监听Apollo Key的Value变更事件

        /// <summary>
        /// 监听Apollo Key的Value变更事件
        /// </summary>
        private static void GetApolloValueChanges()
        {
            IConfig config = ApolloConfigurationManager.GetAppConfig().Result;
            config.ConfigChanged += new ConfigChangeEvent(Config_ConfigChanged);

            var connectionString = "ConnectionString";
            while (true)
            {
                var connectionStringValue = config.GetProperty(connectionString, "127.0.0.1");
                Console.WriteLine($"Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},ConnectionStringValue:{connectionStringValue}");

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 监听Key的变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="changeEvent"></param>
        private static void Config_ConfigChanged(IConfig sender, ConfigChangeEventArgs changeEvent)
        {
            foreach (string key in changeEvent.ChangedKeys)
            {
                ConfigChange change = changeEvent.GetChange(key);
                Console.WriteLine("Change - key: {0}, oldValue: {1}, newValue: {2}, changeType: {3}", change.PropertyName, change.OldValue, change.NewValue, change.ChangeType);
            }
        }

        #endregion

        #region 获取公共的Namespace的配置

        private static void GetPublishNamespace()
        {
            IConfig config = ApolloConfigurationManager.GetAppConfig().Result;

            string publishNamespace = "TEST1.Redis";
            IConfig publishConfig = ApolloConfigurationManager.GetConfig(publishNamespace).Result;

            var connectionString = "ConnectionString";
            while (true)
            {
                var connectionStringValue = config.GetProperty(connectionString, "127.0.0.1");
                Console.WriteLine($"Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},ConnectionStringValue:{connectionStringValue}");

                var publishValue = publishConfig.GetProperty("Name", "Ace.Lv");
                Console.WriteLine($"Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},PublishValue:{publishValue}");

                Thread.Sleep(1000);
            }
        }

        #endregion

        #region 获取多个Namespace的合并结果

        private static void GetMutipleNamespace()
        {
            string customerNamespace = "TEST1.Redis";
            IConfig config = ApolloConfigurationManager.GetConfig(new[] { customerNamespace, ConfigConsts.NamespaceApplication }).Result;
            while (true)
            {
                string value = config.GetProperty("Name", "Ace.Lv");
                Console.WriteLine($"Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},ConnectionStringValue:{value}");
                Thread.Sleep(1000);
            }
        }

        #endregion
    }
}
