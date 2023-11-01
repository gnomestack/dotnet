using System.Diagnostics.CodeAnalysis;

namespace GnomeStack.Dex.Flows;

[SuppressMessage("ReSharper", "TypeParameterCanBeVariant")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public interface IExecutionDescriptorBuilder<B, D>
    where B : IExecutionDescriptorBuilder<B, D>
    where D : IExecutionDescriptor
{
    B AddDeps(params string[] deps);

    B Set(Action<D> update);

    B WithName(string name);

    B WithDescription(string description);

    B WithTimeout(EvaluateAsync<int> eval);

    B WithTimeout(Evaluate<int> eval);

    B WithTimeout(int timeout);

    B WithForce(EvaluateAsync<bool> eval);

    B WithForce(Evaluate<bool> eval);

    B WithForce(bool force = true);

    B WithSkip(EvaluateAsync<bool> eval);

    B WithSkip(Evaluate<bool> eval);

    B WithSkip(bool skip = true);

    B WithDeps(params string[] deps);

    B WithDeps(IEnumerable<string> deps);
}