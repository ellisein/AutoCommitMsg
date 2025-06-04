using System.Diagnostics;
using System.Text;

namespace AutoCommitMsg.Services;

public static class GitService
{
    public static void CheckGitRepository(string? path)
    {
        if (!IsGitRepository(path))
            throw new Exception($"The path '{path}' is not a valid Git repository.");
    }

    public static bool IsGitRepository(string? path)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        try
        {
            var command = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = "rev-parse --is-inside-work-tree",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = path
            };

            using var process = Process.Start(command);
            if (process == null)
                return false;

            var output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            return output == "true";
        }
        catch
        {
            return false;
        }
    }

    public static List<string> GetGitLogs(string? path, int count)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("Path cannot be null or empty.", nameof(path));

        var logs = new List<string>();
        try
        {
            var command = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"log -n {count} --pretty=format:\"%s\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = path,
                StandardOutputEncoding = Encoding.UTF8
            };
            using var process = Process.Start(command);
            if (process == null)
                return logs;
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                if (line != null)
                    logs.Add(line);
            }
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve git logs.", ex);
        }
        return logs;
    }

    public static string GetGitDiff(string? path)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("Path cannot be null or empty.", nameof(path));

        try
        {
            var command = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = "diff",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = path,
                StandardOutputEncoding = Encoding.UTF8
            };
            using var process = Process.Start(command);
            if (process == null)
                return string.Empty;
            var output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();
            return output;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve git diff.", ex);
        }
    }
}
