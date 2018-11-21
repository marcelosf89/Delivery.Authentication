using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Delivery.Authentication.Performance.Tests
{

    [ExcludeFromCodeCoverage]
    public class ConfigurationHelper
    {
        private static readonly ConfigurationHelper _instance = new ConfigurationHelper();
        public static ConfigurationHelper GetInstance(string[] args)
        {
            _instance.SetArguments(args);
            return _instance;
        }
        public static ConfigurationHelper Instance
        {
            get { return _instance; }
        }

        private IDictionary<string, string> _args;

        private ConfigurationHelper()
        {
            _args = new Dictionary<string, string>();
        }

        private void SetArguments(string[] args)
        {
            string[] itemSplit;
            foreach (var item in args)
            {
                itemSplit = item.Split('=');
                _args.Add(itemSplit[0].ToLower(), itemSplit[1]);
            }            
        }

        public string GetArguments(string key, string defaultValue = "")
        {
            if(!_args.ContainsKey(key))
            {
                return defaultValue;
            }

            return _args[key.ToLower()];
        }
    }
}
