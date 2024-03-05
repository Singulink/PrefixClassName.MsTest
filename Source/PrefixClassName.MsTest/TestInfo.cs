﻿using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.Win32;
using Windows.Win32.System.Diagnostics.ToolHelp;

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
#if NET
        if (!OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600))
            return false;

        int processId = Environment.ProcessId;
#else
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return false;

        int processId = Process.GetCurrentProcess().Id;
#endif
        if (!TryGetParentProcess(processId, out var testRunnnerProcess))
            return false;

        if (!testRunnnerProcess.ProcessName.Equals("vstest.console", StringComparison.OrdinalIgnoreCase) ||
            !TryGetParentProcess(testRunnnerProcess.Id, out var hostProcess))
        {
            return false;
        }

        return hostProcess.ProcessName.Equals("ServiceHub.TestWindowStoreHost", StringComparison.OrdinalIgnoreCase);
    }

    [SupportedOSPlatform("windows5.1.2600")]
    private static bool TryGetParentProcess(int processId, [MaybeNullWhen(false)] out Process process)
    {
        if (TryGetParentProcessId(processId, out int parentProcessId))
        {
            process = Process.GetProcessById(parentProcessId);
            return true;
        }

        process = default;
        return false;
    }

    [SupportedOSPlatform("windows5.1.2600")]
    private static unsafe bool TryGetParentProcessId(int processId, out int parentProcessId)
    {
        using var hSnapshot = PInvoke.CreateToolhelp32Snapshot_SafeHandle(CREATE_TOOLHELP_SNAPSHOT_FLAGS.TH32CS_SNAPPROCESS, (uint)processId);

        if (hSnapshot.IsInvalid)
            throw new Win32Exception();

        var process = new PROCESSENTRY32 { dwSize = (uint)sizeof(PROCESSENTRY32) };

        if (!PInvoke.Process32First(hSnapshot, ref process))
        {
            parentProcessId = default;
            return false;
        }

        do
        {
            if (process.th32ProcessID == (uint)processId)
            {
                parentProcessId = (int)process.th32ParentProcessID;
                return true;
            }
        } while (PInvoke.Process32Next(hSnapshot, ref process));

        parentProcessId = default;
        return false;
    }
}