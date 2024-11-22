using ILGPU;
using ILGPU.Runtime;

public class ILGPUAddition
{
    private static Context context;
    private static Accelerator accelerator;
    private static Action<Index1D, ArrayView<float>, ArrayView<float>, ArrayView<float>> kernel;

    // Initialize the ILGPU context and accelerator once
    public static void Initialize()
    {
        // Create a context for ILGPU (can select a specific device or default)
        context = Context.CreateDefault();
        accelerator = context.Devices[2].CreateAccelerator(context); // Select device 2 (can be adjusted)

        // Define the kernel for vector addition
        kernel = accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<float>, ArrayView<float>, ArrayView<float>>(AddVectors);
    }

    // Method to perform vector addition
    public static void Add(float[] A, float[] B, float[] C)
    {
        // Allocate memory on the accelerator
        using var bufferA = accelerator.Allocate1D(A);
        using var bufferB = accelerator.Allocate1D(B);
        using var bufferC = accelerator.Allocate1D<float>(A.Length);

        // Launch the kernel
        kernel((int)A.Length, bufferA.View, bufferB.View, bufferC.View);

        // Copy the result back to the host
        bufferC.CopyToCPU(C);
    }

    // Kernel function for vector addition on the GPU
    private static void AddVectors(Index1D index, ArrayView<float> A, ArrayView<float> B, ArrayView<float> C)
    {
        C[index] = A[index] + B[index];
    }

    // Cleanup the ILGPU context and accelerator
    public static void Cleanup()
    {
        accelerator.Dispose();
        context.Dispose();
    }
}
