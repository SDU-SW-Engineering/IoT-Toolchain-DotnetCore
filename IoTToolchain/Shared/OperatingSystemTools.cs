using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace IoTToolchain {
    public static class OperatingSystemTools {
        public static bool IsWindows() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static bool IsMacOS() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static bool IsLinux() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public static OperatingSystemType Identify() {
            if (IsWindows()) return OperatingSystemType.Windows;
            if (IsMacOS()) return OperatingSystemType.MacOS;
            if (IsLinux()) return OperatingSystemType.Linux;
            return OperatingSystemType.Other;
        }

        public static string RunInBash(this string cmd, string workingDirectory = "./") {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process() {
                StartInfo = new ProcessStartInfo {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        public static string RunInCmd(this string cmd, string workingDirectory = "./") {
            var process = new Process() {
                StartInfo = new ProcessStartInfo {
                    FileName = "cmd.exe",
                    Arguments = $"/c {cmd}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        public static string RunInWsl(this string cmd, string workingDirectory = "./") {
            var process = new Process() {
                StartInfo = new ProcessStartInfo {
                    FileName = "cmd.exe",
                    Arguments = $"/c wsl {cmd}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        public static string RunInOSAgnosticTerminal(this string cmd, string workingDirectory = "./") {
            switch (OperatingSystemTools.Identify()) {
                case OperatingSystemType.Windows:
                    return RunInCmd(cmd, workingDirectory);
                case OperatingSystemType.Linux: //same for linux and mac
                case OperatingSystemType.MacOS:
                    return RunInBash(cmd, workingDirectory);
                case OperatingSystemType.WSL:
                    return RunInWsl(cmd, workingDirectory);
                default:
                    throw new NotSupportedException("The operating system is not supported");
            }
        }


        public static Thread StartAsThread(Action methodToRunInThread) {
            var thread = new Thread(new ThreadStart(() => {
                methodToRunInThread.DynamicInvoke();
            })) {
                IsBackground = true
            };
            thread.Start();
            return thread;
        }
    }

    public enum OperatingSystemType {
        Windows,
        Linux,
        MacOS,
        Other,
        WSL
    }
}