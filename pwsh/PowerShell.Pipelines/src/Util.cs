using GnomeStack.Extras.Strings;
using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Pipelines;

internal static class Util
{
    private static readonly Lazy<bool> s_isTfBuild = new(() =>
    {
        if (Env.TryGet("TF_BUILD", out var tfBuild) && tfBuild.Equals("true", StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    });

    private static readonly Lazy<bool> s_isGithubActions = new(() =>
    {
        if (Env.TryGet("GITHUB_ACTIONS", out var ghActions) &&
                    ghActions.Equals("true", StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    });

    private static Lazy<string> s_pipelineWorkspace = new(() =>
    {
        if (Env.TryGet("PIPELINE_WORKSPACE", out var workspace))
            return workspace;

        if (Env.Has("JENKINS_URL") && Env.TryGet("WORKSPACE", out workspace))
            return workspace;

        if (Env.TryGet("GITHUB_WORKSPACE", out workspace))
            return workspace;

        if (Env.TryGet("CI_PROJECT_DIR", out workspace))
            return workspace;

        string? cwd = Environment.CurrentDirectory;
        while (!cwd.IsNullOrWhiteSpace())
        {
            var git = Path.Combine(cwd, ".git");
            if (Directory.Exists(git))
                return cwd;

            cwd = Path.GetDirectoryName(cwd);
        }

        return Environment.CurrentDirectory;
    });

    private static Lazy<string> s_artifactsDirectory = new(() =>
    {
        if (Env.TryGet("PIPELINE_ARTIFACTS", out var artifacts))
            return artifacts;

        if (Env.TryGet("BUILD_ARTIFACTSTAGINGDIRECTORY", out artifacts))
            return artifacts;

        return Path.Combine(s_pipelineWorkspace.Value, ".artifacts");
    });

    public static bool IsTfBuild => s_isTfBuild.Value;

    public static bool IsGitHubActions => s_isGithubActions.Value;

    public static string PipelineWorkspace
    {
        get => s_pipelineWorkspace.Value;
        set
        {
            if (!Env.Has("PIPELINE_WORKSPACE"))
                Env.Set("PIPELINE_WORKSPACE", value);

            s_pipelineWorkspace = new Lazy<string>(() => value);
        }
    }

    public static string ArtifactsDirectory
    {
        get => s_artifactsDirectory.Value;
        set
        {
            if (!Env.Has("PIPELINE_ARTIFACTS"))
                Env.Set("PIPELINE_ARTIFACTS", value);

            s_artifactsDirectory = new Lazy<string>(() => value);
        }
    }
}