import { getOrDefault } from "https://deno.land/x/quasar@0.0.7/os/env.ts";
import { env, path } from "../deps.ts";

export interface IContext {
    projectDir: string;
    slnName: string;
    rootDir: string;
    sln: string;
    projectName: string;
    tf: boolean;
    ci: boolean;
    dconfig: string;
    artifactsDir: string;
    testResultsDir: string;
    output: string;
    packagesDir: string;
}

export class GitHub 
{
    get token() {
        return Deno.env.get("GITHUB_TOKEN");
    }

    get pat() {
        return Deno.env.get("GH_PAT");
    }

    get username() {
        return Deno.env.get("GH_USER");
    }

    get org() {
        return Deno.env.get("GH_ORG");
    }
}

export class DockerHub
{
    get username() {
        return Deno.env.get("DOCKER_HUB_USER");
    }

    get password() {
        return Deno.env.get("DOCKER_HUB_PASSWORD");
    }
}

export class Nuget
{
    get apiKey() {
        return Deno.env.get("NUGET_API_KEY");
    }
}

export class DotNet
{
    #isCi: boolean;
    #configuration: string;
    #collect: string;
    #coverageFormat: string;

    constructor(isCi: boolean)
    {
        this.#isCi = isCi;
        Deno.env.set("DOTNET_NOLOGO", "true");
        Deno.env.set("DOTNET_CLI_TELEMETRY_OPTOUT", "true");
        Deno.env.set("DOTNET_SKIP_FIRST_TIME_EXPERIENCE", "true");
        Deno.env.set("DOTNET_GENERATE_ASPNET_CERTIFICATE", "false");
        Deno.env.set("DOTNET_ADD_GLOBAL_TOOLS_TO_PATH", "false");
        Deno.env.set("DOTNET_MULTILEVEL_LOOKUP", "0");
        Deno.env.set("DOTNET_SYSTEM_CONSOLE_ALLOW_ANSI_COLOR_REDIRECTION", "true");
        Deno.env.set("TERM", "xterm");
        this.#collect = getOrDefault("DOTNET_TEST_COVERAGE", "XPlat Code Coverage");
        this.#coverageFormat = getOrDefault("DOTNET_TEST_COVERAGE_FORMAT", "cobertura");
        this.#configuration = getOrDefault("DOTNET_BUILD_CONFIG", this.#isCi ? "Release" : "Debug");
    }

    get configuration() {
        return this.#configuration;
    }

    get collect() {
        return this.#collect;
    }

    get coverageFormat() {
        return this.#coverageFormat;
    }
}

export class BuildContext {
    #rootDir: string;
    #projectDir: string;
    #sln: string
    #github: GitHub;
    #dockerHub: DockerHub;
    #nuget: Nuget;
    #artifactDir: string;
    #testResultsDir: string;
    #packagesDir: string;
    #isCi: boolean;
    #dotnet: DotNet;

    constructor(rootDir: string, projectDir: string, slnName?: string, projectName?: string) {
        this.#rootDir = rootDir;
        if (!path.isAbsolute(projectDir)) {
            projectDir = path.join(rootDir, projectDir);
        }

        if (!projectName) {
            projectName = path.basename(projectDir);
        }

        if (!slnName) {
            slnName = `${projectName}.sln`;
        }

        const tf = env.getOrDefault("TF_BUILD", "false") === "true";
        const ci = env.getOrDefault("CI", "false") === "true" || tf;
        this.#isCi = ci;

        let artifactsDir = path.join(rootDir, ".artifacts");
        if (env.get("ARTIFACTS_DIR")) {
            artifactsDir = env.get("ARTIFACTS_DIR")!;
        }

        this.#artifactDir = artifactsDir;
        this.#testResultsDir = path.join(this.#artifactDir, "test-results", projectName);
        this.#packagesDir = path.join(this.#artifactDir, "packages");
        this.#projectDir = projectDir;
        this.#sln = path.join(projectDir, slnName);
        this.#dockerHub = new DockerHub();
        this.#github = new GitHub();
        this.#nuget = new Nuget();
        this.#dotnet = new DotNet(ci);
    }

    get ci()
    {
        return this.#isCi;
    }

    get projectDir() {
        return this.#projectDir;
    }

    get sln() {
        return this.#sln;
    }

    get rootDir() {
        return this.#rootDir;
    }

    get github() {
        return this.#github;
    }

    get dockerHub() {
        return this.#dockerHub;
    }

    get nuget() {
        return this.#nuget;
    }

    get artifactDir() {
        return this.#artifactDir;
    }

    get testResultsDir() {
        return this.#testResultsDir;
    }

    get packagesDir() {
        return this.#packagesDir;
    }

    get dotnet() {
        return this.#dotnet;
    }
}