using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ForcePush.CliParsing
{
    public class ArgumentBinder
    {
        public T Bind<T>(string[] args) where T : class, new()
        {
            var map = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            var instance = new T();
            foreach (var property in map)
            {
                foreach (var arg in args)
                {
                    if (!arg.ToLower().StartsWith("-" + property.Name.ToLower())) continue;

                    if (property.PropertyType != typeof(string))
                    {
                        throw new Exception($"The binder only supports string properties. Could not bind '{arg}' to '{property.PropertyType.Name}'.");
                    }

                    var value = arg.Split('=');
                    object val = value.Length > 1 ? (value[1]??"").Trim('"', '\'') : null;
                    
                    property.SetValue(instance, val);
                }
            }

            return instance;
        }

        public List<string> Hint<T>()
        {
            var map = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            var msg = new List<string> {"Supported arguments:"};

            foreach (var property in map.Where(x=>x.PropertyType == typeof(string)))
            {
                var optional = property.GetCustomAttribute<OptionalAttribute>();
                var optionalString = optional != null ? ", optional" : "";

                msg.Add($"\t\t\t-{property.Name.ToLower()}=... ({property.PropertyType.Name.ToLower()}{optionalString})");
            }

            return msg;
        }
    }
}