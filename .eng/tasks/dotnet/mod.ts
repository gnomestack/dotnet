import { IExecSyncOptions } from "https://deno.land/x/quasar@0.0.7/shell/core/mod.ts";
import { BuildContext } from "../context/mod.ts";


export function getCleanArgs(ctx: BuildContext) {
    const splat = ["clean", ctx.sln, "-c", ctx.dotnet.configuration];
    const options : IExecSyncOptions = { cwd: ctx.projectDir };
    return { splat, options };    
}

export function getBuildArgs(ctx: BuildContext) {
    const splat = ["build", ctx.sln, "-c", ctx.dotnet.configuration];
    if (ctx.ci) {
        splat.push("--no-restore");
    }

    const options : IExecSyncOptions = { cwd: ctx.projectDir };
    return { splat, options };
}

export function getTestArgs(ctx: BuildContext) {
    const splat = [
        "test", 
        ctx.sln, 
        "-c", ctx.dotnet.configuration, 
        "--logger", "trx", 
        "--results-directory", ctx.testResultsDir];
    
    if (ctx.ci) {
    
        splat.push("/p:CollectCoverage=true")
        splat.push(`/p:CoverletOutputFormat=${ctx.dotnet.coverageFormat}`);
        splat.push(`/p:CoverletOutput=${ctx.testResultsDir}`);
        splat.push("--no-restore");
        splat.push("--no-build");
    }

    const options : IExecSyncOptions = { cwd: ctx.projectDir };
    return { splat, options };
}

export function getRestoreArgs(ctx: BuildContext) {
    const { sln, projectDir } = ctx;
    const splat = ["restore", sln];
    const options : IExecSyncOptions = { cwd: projectDir };
    return { splat, options };
}

export function getToolRestoreArgs(ctx: BuildContext) {
    const { rootDir } = ctx;

    const splat = ["tool", "restore"];
    const options : IExecSyncOptions = { cwd: rootDir };
    return { splat, options };
}

export function getPackArgs(ctx: BuildContext) {
    const splat = ["pack", ctx.sln, "-c", ctx.dotnet.configuration];
    if (ctx.ci) {
        splat.push("--no-restore");
        splat.push("--no-build");
    }

    const options : IExecSyncOptions = { cwd: ctx.projectDir };
    return { splat, options };
}

export function getPublishArgs(ctx: BuildContext) {
    const splat = ["publish", ctx.sln, "-c", ctx.dotnet.configuration, "--output", ctx.artifactDir];
    if (ctx.ci) {
        splat.push("--no-restore");
        splat.push("--no-build");
    }

    const options : IExecSyncOptions = { cwd: ctx.projectDir };
    return { splat, options };
}

export function getGithubNugetSourceArgs(ctx: BuildContext) {
    if (!ctx.github.username)
        throw new Error("GITHUB_USER not set");

    if (!ctx.github.pat)
        throw new Error("GITHUB_TOKEN not set");

    if (!ctx.github.org)
        throw new Error("GITHUB_ORG not set");

    const splat = [
        "nuget", 
        "add", 
        "source", 
        "--username", ctx.github.username,
        "--password", ctx.github.pat,
        "--store-password-in-clear-text",
        "--name", "github",
        `https://nuget.pkg.github.com/${ctx.github.org}/index.json`];
    const options : IExecSyncOptions = { cwd: ctx.rootDir };
    return { splat, options };
}