﻿using System;
using UniRx;

namespace Feofun.Extension
{
    public static class ObservableExt
    {
        public static IObservable<float> Diff(this IObservable<float> source) =>
            source.Buffer(2, 1).Select(it => it[1] - it[0]);
        
        public static IObservable<int> Diff(this IObservable<int> source) =>
                source.Buffer(2, 1).Select(it => it[1] - it[0]);
    }
}