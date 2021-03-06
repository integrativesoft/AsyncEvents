# AsyncEvents for C#

This library adds support for asynchronous events in C#

```csharp
// BEFORE: Declaration for a regular synchronous event:
public event MyEvent OnChange;

// AFTER: Declaration for an asynchronous event:
public AsyncEvent OnChangeAsync { get; } = new AsyncEvent();
```

# Sample application

```csharp
using Integrative.Async;
using System;
using System.Threading.Tasks;

namespace AsyncEventsSample
{
    class Program
    {
        // instance that raises async events
        readonly static MyEventSource source = new MyEventSource();

        // program's Main
        static async Task Main()
        {
            Console.WriteLine("Hello World!");
            source.OnChangeAsync.Subscribe(ShowCounterAsync);
            await source.IncreaseAsync();
            Console.WriteLine("Done!");
        }

        // handler for events
        static Task ShowCounterAsync(object sender, CounterChangedEventArgs args)
        {
            Console.WriteLine($"Counter value: {args.Counter}");
            return Task.CompletedTask;
        }
    }

    // class for async event parameters
    class CounterChangedEventArgs : EventArgs
    {
        public int Counter { get; set; }
    }

    // class that raises events
    class MyEventSource
    {
        int counter;

        // async event declaration
        public AsyncEvent<CounterChangedEventArgs> OnChangeAsync { get; }
            = new AsyncEvent<CounterChangedEventArgs>();

        // method that triggers events
        public async Task IncreaseAsync()
        {
            counter++;
            await OnChangeAsync.InvokeAsync(this, new CounterChangedEventArgs
            {
                Counter = counter
            });
        }
    }
}

```
