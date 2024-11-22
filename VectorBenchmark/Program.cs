using BenchmarkDotNet.Running;
using System;

BenchmarkRunner.Run<VectorAdditionBenchmark>();

//var benchmark = new VectorAdditionBenchmark();
//benchmark.Setup();
//benchmark.ILGPUAdd();
