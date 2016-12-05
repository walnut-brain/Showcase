using System;
using System.Diagnostics.CodeAnalysis;
using LanguageExt;
using LanguageExt.SomeHelp;

namespace WalnutBrain.Showcase
{
    public static class Matcher
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public static Matcher<T> match<T>(T obj)
        {
            return new Matcher<T>(obj);
        }

        
    }

    public class Matcher<T>
    {
        private readonly bool _isComplete;
        private T Value { get;  }

        internal Matcher(T value, bool isComplete = false) 
        {
            _isComplete = isComplete;
            Value = value;
        }

        public Matcher<T> When<TMiddle>(Func<T, Option<TMiddle>> condition, Action<TMiddle> action)
        {
            if (_isComplete) return this;
            var cr = condition(Value);
            return cr.Match(
                p =>
                    {
                        action(p);
                        return new Matcher<T>(Value, true);
                    },
                () => this);
        }

        public Matcher<T, TResult> When<TMiddle, TResult>(Func<T, Option<TMiddle>> condition, Func<TMiddle, TResult> func)
        {
            if (_isComplete) new Matcher<T, TResult>(Value);
            var cr = condition(Value);
            return cr.Match(
                p => new Matcher<T, TResult>(Value, func(p)),
                () => new Matcher<T, TResult>(Value));
        }

        public Matcher<T> Is<T1>(Action<T1> action)
            where T1 : T
        {
            return When(p =>
            {
                if (p is T1) return ((T1) p).ToSome();
                return Option<T1>.None;
            }, action);
        }

        public Matcher<T, TResult> Is<T1, TResult>(Func<T1, TResult> func)
            where T1 : T
        {
            return When(p =>
            {
                if (p is T1) return ((T1)p).ToSome();
                return Option<T1>.None;
            }, func);
        }

        public Matcher<T, TResult> As<TResult>()
        {
            return new Matcher<T, TResult>(Value);
        }
    }

    public class Matcher<T, TResult> 
    {
        internal T Value { get; }
        internal bool IsComplete { get; }
        internal TResult Result { get; }

        internal Matcher(T value)
        {
            Value = value;
            IsComplete = false;
        }

        internal Matcher(T value, TResult result)
        {
            Value = value;
            IsComplete = true;
            Result = result;
        }

        public Matcher<T, TResult> When<TMiddle>(Func<T, Option<TMiddle>> condition, Func<TMiddle, TResult> func)
        {
            if (IsComplete) return this;
            var sr = condition(Value);
            return sr.Match(p => new Matcher<T, TResult>(Value, func(p)), () => this);
        }

        public TResult Else(Func<T, TResult> func)
        {
            return IsComplete ? Result : func(Value);
        }

        public TResult Else(TResult result)
        {
            return IsComplete ? Result : result;
        }

        public Matcher<T, TResult> Is<T1>(Func<T1, TResult> func)
            where T1 : T
        {
            return When(p =>
            {
                if (p is T1) return ((T1)p).ToSome();
                return Option<T1>.None;
            }, func);
        }
    }
}