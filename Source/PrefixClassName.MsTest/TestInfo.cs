using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PrefixClassName.MsTest;

internal static class TestInfo
{
    private static readonly List<(Assembly Assembly, string CommonPrefix)> _commonPrefixes = [];

    public static bool RunningInTestExplorer { get; } = DetectIfRunningInTestExplorer();

    public static string GetCommonPrefix(Assembly assembly)
    {
        lock (_commonPrefixes)
        {
            foreach (var entry in _commonPrefixes)
            {
                if (entry.Assembly == assembly)
                    return entry.CommonPrefix;
            }

            string commonPrefix = null;

            foreach (var type in assembly.GetTypes().Where(t => t.IsPublic && !t.IsAbstract && t.GetCustomAttribute<TestClassAttribute>() != null))
            {
                string typeNamespace = type.Namespace;

                if (string.IsNullOrEmpty(typeNamespace))
                {
                    commonPrefix = string.Empty;
                    break;
                }

                string typeName = type.FullName!;

                if (commonPrefix == null)
                {
                    commonPrefix = typeNamespace + ".";
                    continue;
                }

                var newCommonPrefix = GetCommonPrefix(commonPrefix.AsSpan(), typeName.AsSpan());
                newCommonPrefix = newCommonPrefix[..(newCommonPrefix.LastIndexOf('.') + 1)];

                if (newCommonPrefix.Length == 0)
                {
                    commonPrefix = string.Empty;
                    break;
                }

                if (newCommonPrefix.Length != commonPrefix.Length)
                {
                    commonPrefix = newCommonPrefix.ToString();
                }
            }

            commonPrefix ??= string.Empty;
            _commonPrefixes.Add((assembly, commonPrefix));

            return commonPrefix;
        }
    }

    private static ReadOnlySpan<char> GetCommonPrefix(ReadOnlySpan<char> x, ReadOnlySpan<char> y)
    {
        int i;
        int length = Math.Min(x.Length, y.Length);

        for (i = 0; i < length; i++)
        {
            if (x[i] != y[i])
                break;
        }

        return x[..i];
    }

    private static bool DetectIfRunningInTestExplorer()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return false;

#if NET
        int processId = Environment.ProcessId;
#else
        int processId = Process.GetCurrentProcess().Id;
#endif

        var wmicStartInfo = new ProcessStartInfo
        {
            FileName = "wmic",
            Arguments = $"process where (processid={processId}) get parentprocessid",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        try
        {
            var wmicProcess = Process.Start(wmicStartInfo)!;
            wmicProcess.StandardOutput.ReadLine(); // Skip the header line
            string parentProcessIdString = wmicProcess.StandardOutput.ReadToEnd()!.Trim();

            int parentProcessId = int.Parse(parentProcessIdString);
            var parentProcess = Process.GetProcessById(parentProcessId);

            return parentProcess.ProcessName == "vstest.console";
        }
        catch
        {
            return false;
        }
    }
}