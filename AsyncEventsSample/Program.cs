/*
Copyright (c) 2019 Integrative Software LLC
MIT License
Created: 12/2019
Author: Pablo Carbonell
*/

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
