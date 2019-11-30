using System;

namespace OsuHelper.Logic
{
    internal static class Extensions
    {
        public static TOut Pipe<TIn, TOut>(this TIn value, Func<TIn, TOut> transform) => transform(value);
    }
}