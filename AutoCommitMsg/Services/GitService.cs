using System.Diagnostics;

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
}
