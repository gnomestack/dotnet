import { IS_DARWIN } from "https://deno.land/x/quasar@0.0.6/mod.ts";
import { BuildContext } from "./context/mod.ts";
import { env, path, ps, fs, task, parseAndRun } from "./deps.ts";
import { getBuildArgs, getCleanArgs, getGithubNugetSourceArgs, getPackArgs, getRestoreArgs, getTestArgs, getToolRestoreArgs } from "./dotnet/mod.ts";
import { loadSecrets } from "./secrets/mod.ts";

export { task, path, ps, env, parseAndRun }

export async function addDotnetTasks(projectDir: string, rootDir?: string, slnName?: string) {
    
    if (!rootDir) {
        const cwd = path.dirname(path.fromFileUrl(import.meta.url));
        rootDir = path.resolve(path.join(cwd, "..", ".."));
    }
    if (!path.isAbsolute(projectDir)) {
        projectDir = path.join(rootDir, projectDir);
    }

    await loadSecrets(projectDir, rootDir);

    const ctx = new BuildContext(rootDir, projectDir, slnName);

    task("tools:restore", () => {
        const o = getToolRestoreArgs(ctx);
        return ps.exec("dotnet", o.splat, o.options);
    });

    task("artifacts:clean", async () => {
        if (await fs.exists(ctx.artifactDir)) {
            await fs.remove(ctx.artifactDir, { recursive: true});
        }
    })

    task("dotnet:clean", () => {
        const o = getCleanArgs(ctx);
        return ps.exec("dotnet", o.splat, o.options);
    });

    task("clean", ["artifacts:clean", "dotnet:clean"]);

    task("dotnet:restore", () => {
        const o = getRestoreArgs(ctx);
        return ps.exec("dotnet", o.splat, o.options);
    });

    task("restore", ["dotnet:restore"]);

    task("dotnet:build", () => {
        const o = getBuildArgs(ctx);
        return ps.exec("dotnet", o.splat, o.options).then(o => o.throwOrContinue());
    })

    task("build", ["dotnet:build"]);

    task("dotnet:test", async () => {
        const o = getTestArgs(ctx);
        const r = await ps.exec("dotnet", o.splat, o.options);
        if (!IS_DARWIN) {
            return r;
        }
    })

    task("test", ["dotnet:test"]);

    task("dotnet:pack", () => { 
        const o = getPackArgs(ctx);
        return ps.exec("dotnet", o.splat, o.options).then(o => o.throwOrContinue()); 
    });

    task("pack", ["dotnet:pack"]);


    task("github:publish", async () => {
        const o1 = getGithubNugetSourceArgs(ctx);
        await ps.exec("dotnet", o1.splat, o1.options)
            .then(o => o.throwOrContinue());

        const packages = ctx.packagesDir;
        for await (const next of fs.readDirectory(packages))
        {
            if (next.isFile && next.name.endsWith(".nupkg")) {
                const fullPath = path.join(packages, next.name);
                console.log(`nuget push ${fullPath}`)
                const splat = [
                    "nuget", 
                    "push", 
                    fullPath, 
                    "--source", "github",
                    "--api-key", ctx.github.pat!];

                await ps.exec("dotnet", splat, { cwd: ctx.rootDir })
                    .then(o => o.throwOrContinue());
            } else {
                console.log(`skipping ${next.name}`)
            }
        }
    });

    task("nuget:publish", async () => {
        const packages = ctx.packagesDir;
        for await (const next of fs.readDirectory(packages))
        {
            if (next.isFile && next.name.endsWith(".nupkg")) {
                const fullPath = path.join(packages, next.name);
                console.log(`nuget push ${fullPath}`)
                const splat = [
                    "nuget", 
                    "push", 
                    fullPath, 
                    "--source", "nuget.org",
                    "--api-key", ctx.nuget.apiKey!];

                await ps.exec("dotnet", splat, { cwd: ctx.rootDir })
                    .then(o => o.throwOrContinue());
            } else {
                console.log(`skipping ${next.name}`)
            }
        }
    })
}