/*
Copyright (c) 2019 Integrative Software LLC
MIT License
Created: 12/2019
Author: Pablo Carbonell
*/

using System;
using System.Threading.Tasks;
using Xunit;

namespace Integrative.Async.Testing
{
    public class AsyncEventTests
    {
        AsyncEvent _ev = new AsyncEvent();
        int _counter = 0;

        [Fact]
        public async void RunActionHandler()
        {
            _ev.Subscribe(AddAction);
            await _ev.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            await _ev.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            Assert.Equal(2, _counter);
            _ev.Unsubscribe(AddAction);
            await _ev.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            await _ev.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            Assert.Equal(2, _counter);
        }

        private void AddAction() => _counter++;

        [Fact]
        public async void RunTaskAction()
        {
            _ev.Subscribe(AddTask);
            await _ev.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            await _ev.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            Assert.Equal(2, _counter);
            _ev.Unsubscribe(AddTask);
            await _ev.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            await _ev.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            Assert.Equal(2, _counter);
        }

        private Task AddTask()
        {
            _counter++;
            return Task.CompletedTask;
        }

        [Fact]
        public async void RunAsyncEvent()
        {
            var x1 = new AsyncEvent();
            x1.Subscribe(AddTask);
            var x2 = new AsyncEvent();
            x2.Subscribe(x1);
            await x2.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            await x2.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            Assert.Equal(2, _counter);
            x2.Unsubscribe(x1);
            await x2.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            await x2.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            Assert.Equal(2, _counter);
        }

        [Fact]
        public async void RunAsyncEventHandler()
        {
            _ev.Subscribe(AddHandler);
            await _ev.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            await _ev.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            Assert.Equal(2, _counter);
            _ev.Unsubscribe(AddHandler);
            await _ev.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            await _ev.InvokeAsync(this, new EventArgs()).ConfigureAwait(false);
            Assert.Equal(2, _counter);
        }

        private Task AddHandler(object sender, EventArgs args)
        {
            _counter++;
            return Task.CompletedTask;
        }
    }
}
