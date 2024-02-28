namespace Shared.DiagnosticContext;

public interface IDiagnosticContextStorage
{
    IDisposable Measure(string key);
}