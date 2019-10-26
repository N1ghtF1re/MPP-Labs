using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MPP3.config;
using NLog;


namespace MPP3
{
    public delegate bool BooleanDelegate(Type type);

    public class DllUtils
    {
        private readonly Assembly _dll;

        private static readonly Logger Logger = NLogConfiguration.GetLogger("DllUtils");
        
        public DllUtils(string dllPath)
        {
            Logger.Info("Loading dll " + dllPath);

            try
            {
                this._dll = Assembly.LoadFile(dllPath);
            }
            catch
            {
                Logger.Error("Dll loading exception...");
                throw new ArgumentException("Unable to load dll: " + dllPath);
            }
        }

        private static void InitializeNameSpace(IDictionary<string, List<string>> dictionary, 
                                                string namespaceName)
        {
            if (!dictionary.ContainsKey(namespaceName))
            {
                dictionary.Add(namespaceName, new List<string>());
                Logger.Info("Initializing a new namespace " + namespaceName);
            }
        }

        public IDictionary<string, List<string>> GetPublicTypes()
        {
            return GetTypes(type => type.IsPublic);
        }
        
        public IDictionary<string, List<string>> GetTypes(BooleanDelegate filter)
        {
            var typesMap = new SortedDictionary<string, List<string>>();
            
            foreach (var type in _dll.GetExportedTypes())
            {
                InitializeNameSpace(typesMap, type.Namespace);
                var list = typesMap[type.Namespace];

                if (filter(type))
                {
                    Logger.Info("Adding type " + type.FullName);
                    list.Add(type.Name);
                }
                else
                {
                    Logger.Info("Found type which doesn't match to conditions' "+ type.FullName);
                }
            }

            return typesMap;
        }
    }
}