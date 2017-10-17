using System;
using System.IO;
using System.Reflection;

namespace ig.log4net.Extensions
{
    public static class AssemblyExtensions
    {

        public static string AssemblyDirectory(this Type type)
        {
            string codeBase = Assembly.GetAssembly(type).CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

    }
}
