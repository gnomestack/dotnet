import { ps,  task, parseAndRun, env, path } from "https://deno.land/x/qtr@0.0.5/mod.ts";

const cwd = path.dirname(path.fromFileUrl(import.meta.url));
const rootDir = path.resolve(path.join(cwd, ".."));
const sln = path.join(rootDir, "dfx.sln");
const tf = env.getOrDefault("TF_BUILD", "false") === "true";
const ci = env.getOrDefault("CI", "false") === "true" || tf;
const dconfig = env.getOrDefault("DOTNET_BUILD_CONFIG", ci ? "Release" : "Debug");

task("clean", () => {
    return ps.exec("dotnet", ["clean", sln, "-c", dconfig]);
});

task("restore", () => {
    return ps.exec("dotnet", ["restore", sln]);
});

task("build", () => {
    const args = ["build", sln, "-c", dconfig];
    if (ci)
        args.push("--no-restore");

    return ps.exec("dotnet", args);
});

task("test", () => {
    const args = ["test", sln, "-c", dconfig];
    if (ci) {
        args.push("--no-restore");
        args.push("--no-build");
    }

    return ps.exec("dotnet", args);
});

if (import.meta.main) {
    const exitCode = await parseAndRun(Deno.args);
    Deno.exit(exitCode);
}