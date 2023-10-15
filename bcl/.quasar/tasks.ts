import { addDotnetTasks, path, parseAndRun } from "../../.eng/tasks/dotnet.ts";

const cwd = path.dirname(path.fromFileUrl(import.meta.url));
const projectDir = path.resolve(path.join(cwd, ".."));
await addDotnetTasks(projectDir);


if (import.meta.main) {
    const exitCode = await parseAndRun(Deno.args);
    Deno.exit(exitCode);
}