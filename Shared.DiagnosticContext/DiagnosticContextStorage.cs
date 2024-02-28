using System.Diagnostics;
using Prometheus;

namespace Shared.DiagnosticContext;

public class DiagnosticContextStorage : IDiagnosticContextStorage
{
    private readonly IDictionary<string, DiagnosticContext> _storage = new Dictionary<string, DiagnosticContext>();
    
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
        {
            throw new ArgumentException("Key cannot be null or empty", nameof(key));
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        return new DisposableAction(() =>
        {
            stopwatch.Stop();

            if (_storage.ContainsKey(key))
            {
                _storage[key].ElapsedMilliseconds += (int)stopwatch.ElapsedMilliseconds;
                _storage[key].Count++;
                _metricCounterDuration.WithLabels(key).Inc(_storage[key].ElapsedMilliseconds);
                _metricCounterExecution.WithLabels(key).Inc(_storage[key].Count);
                Console.WriteLine($"Method: {key}, Milliseconds: {_storage[key].ElapsedMilliseconds} ms, Count: {_storage[key].Count}");
                return;
            }

            _storage.Add(key, new DiagnosticContext
            {
                ElapsedMilliseconds = (int)stopwatch.ElapsedMilliseconds,
                Count = 1
            });
            _metricCounterDuration.WithLabels(key).Inc(_storage[key].ElapsedMilliseconds);
            _metricCounterExecution.WithLabels(key).Inc(_storage[key].Count);
            Console.WriteLine($"Method: {key}, Milliseconds: {_storage[key].ElapsedMilliseconds} ms, Count: {_storage[key].Count}");
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