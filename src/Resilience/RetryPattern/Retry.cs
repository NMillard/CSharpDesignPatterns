using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetryPattern {
    public class Retry {
        private readonly Dictionary<Type, Func<Exception, bool>> canHandleExceptions;

        private readonly RetryPolicy policy = new() {
            MaximumAttempts = 3,
            IntervalBetweenRetries = TimeSpan.FromSeconds(3)
        };

        private readonly RetryResult retryResult = new();

        public Retry() {
            canHandleExceptions = new Dictionary<Type, Func<Exception, bool>>();
        }

        public Retry CanHandle<TException>() where TException : Exception => CanHandle<TException>(_ => true);
        
        public Retry CanHandle<TException>(Func<Exception, bool> predicate) where TException : Exception {
            canHandleExceptions[typeof(TException)] = predicate;
            return this;
        }

        public async Task<RetryResult> RunAsync(Action action) {
            for (var i = 0; i < policy.MaximumAttempts; i++) {
                try {
                    if (i > 0) await Task.Delay(policy.IntervalBetweenRetries);
                    action();
                } catch (Exception e) when (canHandleExceptions[e.GetType()](e)) {
                    retryResult.AddException(e);
                }
            }

            return retryResult;
        }
    }

    public class RetryPolicy {
        public int MaximumAttempts { get; init; }
        public TimeSpan IntervalBetweenRetries { get; init; }
    }

    public class RetryResult {
        private readonly List<Exception> exceptions = new();
        
        public AggregateException Exception => new(exceptions);
        public bool IsSuccessful => !exceptions.Any();
        public void AddException(Exception exception) => exceptions.Add(exception);
    }
}