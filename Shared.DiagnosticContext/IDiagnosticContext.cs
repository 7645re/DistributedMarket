namespace Shared.DiagnosticContext;

public interface IDiagnosticContext
{
    IDisposable Measure(string key);
}