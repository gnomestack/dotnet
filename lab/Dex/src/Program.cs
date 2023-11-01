// See https://aka.ms/new-console-template for more information

using GnomeStack;

using static GnomeStack.Dex.Flows.FlowRunner;



Task("default", "default", new [] { "test" }, _ =>
{
    Console.WriteLine("Jeff was here!");
    return Nil.Value;
});

Task("test", _ =>
{
    Console.WriteLine("Jeff was here!");
    return Nil.Value;
});

return await ParseAndRunAsync(args);