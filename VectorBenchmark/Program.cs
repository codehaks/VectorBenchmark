using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Numerics;

[MemoryDiagnoser]
public class VectorAdditionBenchmark
{
    private float[] A;
    private float[] B;
    private float[] C;

    // Set up the test data
    [GlobalSetup]
    public void Setup()
    {
        int size = 1000000;
        A = new float[size];
        B = new float[size];
        C = new float[size];

        for (int i = 0; i < size; i++)
        {
            A[i] = i;
            B[i] = i * 2;
        }
    }

    // Traditional method: Element-wise addition using a simple for-loop
    [Benchmark]
    public void TraditionalAdd()
    {
        for (int i = 0; i < A.Length; i++)
        {
            C[i] = A[i] + B[i];
        }
    }

    // SIMD method: Using Vector<T> for SIMD-based addition
    [Benchmark]
    public void SIMDAdd()
    {
        int vectorSize = Vector<float>.Count;
        int i = 0;
        for (; i <= A.Length - vectorSize; i += vectorSize)
        {
            var v1 = new Vector<float>(A, i);
            var v2 = new Vector<float>(B, i);
            var sum = v1 + v2;
            sum.CopyTo(C, i);
        }

        // Handle remaining elements (less than vectorSize)
        for (; i < A.Length; i++)
        {
            C[i] = A[i] + B[i];
        }
    }

    // Main method to run the benchmarks
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<VectorAdditionBenchmark>();
    }
}
