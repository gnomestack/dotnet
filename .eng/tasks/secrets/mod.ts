import { path, env, fs, dotenv } from "../deps.ts";

export async function loadSecrets(projectDir: string, rootDir: string) {
   
    if (!rootDir) {
        const cwd = path.dirname(path.fromFileUrl(import.meta.url));
        rootDir = path.resolve(path.join(cwd, "..", "..", ".."));
    }

    if (!path.isAbsolute(projectDir)) {
        projectDir = path.join(rootDir, projectDir);
    }

    const files = [
        path.join(rootDir, "secrets.env"),
        path.join(projectDir, "secrets.env"),
    ]

    for (const file of files) {
        if (await fs.exists(file)) {
            const envConfig = await Deno.readTextFile(file);
            const envConfigParsed = dotenv.parse(envConfig);
            for (const k in envConfigParsed) {
                env.set(k, envConfigParsed[k]);
            }
        }
    }
}

export function loadSecretsSync(projectDir: string) {
    const root = path.join(projectDir, "..");
    const files = [
        path.join(root, "secrets.env"),
        path.join(projectDir, "secrets.env"),
    ]

    for (const file of files) {
        if (fs.existsSync(file)) {
            const envConfig = Deno.readTextFileSync(file);
            const envConfigParsed = dotenv.parse(envConfig);
            for (const k in envConfigParsed) {
                env.set(k, envConfigParsed[k]);
            }
        }
    }
}