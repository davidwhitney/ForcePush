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
                var matchingArg = args.SingleOrDefault(x => x.ToLower().StartsWith("-" + property.Name.ToLower()));
                if (matchingArg == null)
                {
                    if (property.GetCustomAttribute<RequiredAttribute>() != null
                        && property.GetValue(instance) == null)
                    {
                        throw new Exception($"Missing required parameter: -{property.Name.ToLower()}");
                    }

                    continue;
                }

                if (property.PropertyType != typeof(string)
                    && property.PropertyType != typeof(bool))
                {
                    throw new Exception($"The binder only supports string properties. Could not bind '{matchingArg}' to '{property.PropertyType.Name}'.");
                }

                var value = matchingArg.Split('=');
                object val = value.Length > 1 ? (value[1] ?? "").Trim('"', '\'') : null;
                if (property.PropertyType == typeof(bool) && val == null) { val = true; }
                var typed = Convert.ChangeType(val, property.PropertyType);
                if (typed != null) property.SetValue(instance, typed);
            }

            return instance;
        }

        public List<string> Hint<T>()
        {
            var map = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            var msg = new List<string> {"Supported arguments:"};

            foreach (var property in map.Where(x=>x.PropertyType == typeof(string) || x.PropertyType == typeof(bool)))
            {
                var required = property.GetCustomAttribute<RequiredAttribute>();
                var requiredAsString = required == null ? "" : ", required";

                var annotation = property.GetCustomAttribute<AnnotationAttribute>();
                var annotationString = annotation != null ? "\r\n\t" + annotation.Message +"\r\n" : "";

                msg.Add($"\t-{property.Name.ToLower()}=... ({property.PropertyType.Name.ToLower()}{requiredAsString}){annotationString}");
            }

            return msg;
        }
    }
}