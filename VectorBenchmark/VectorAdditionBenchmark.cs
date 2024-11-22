using BenchmarkDotNet.Attributes;

public class VectorAdditionBenchmark
{
    private float[] A;
    private float[] B;
    private float[] C;

    // Set up the test data
    [GlobalSetup]
    public void Setup()
    {
        int size = 10_000_000;
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
        TraditionalAddition.Add(A, B, C);
    }

    // SIMD method: Using Vector<T> for SIMD-based addition
    [Benchmark]
    public void SIMDAdd()
    {
        SIMDAddition.Add(A, B, C);
    }

    // Initialize ILGPU context once before benchmark runs
    static VectorAdditionBenchmark()
    {
        ILGPUAddition.Initialize();
    }

    [Benchmark]
    public void ILGPUAdd()
    {
        ILGPUAddition.Add(A, B, C);
    }

    // Cleanup ILGPU resources after benchmarking
    [GlobalCleanup]
    public void Cleanup()
    {
        ILGPUAddition.Cleanup();
    }
}
