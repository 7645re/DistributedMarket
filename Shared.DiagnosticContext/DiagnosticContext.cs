using System.Diagnostics;
using Prometheus;

namespace Shared.DiagnosticContext;

public class DiagnosticContext : IDiagnosticContext
{
    private readonly Counter _metricCounterDuration = Metrics.CreateCounter(
        "method_execution_duration",
        "Counts the duration of method executions in milliseconds",
        new CounterConfiguration
        {
            LabelNames = new[] { "method_name" }
        }
    );
    
    private readonly Counter _metricCounterExecution = Metrics.CreateCounter(
        "method_execution_count",
        "Counts the number of method executions",
        new CounterConfiguration
        {
            LabelNames = new[] { "method_name" }
        }
    );
    

    public IDisposable Measure(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        return new DisposableAction(() =>
        {
            stopwatch.Stop();
            _metricCounterDuration.WithLabels(key).Inc((int)stopwatch.ElapsedMilliseconds);
            _metricCounterExecution.WithLabels(key).Inc();
        });
    }
}

class DisposableAction : IDisposable
{
    private readonly Action _action;

    public DisposableAction(Action action)
    {
        _action = action;
    }

    public void Dispose()
    {
        _action.Invoke();
    }
}