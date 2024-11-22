using ILGPU;
using ILGPU.Runtime;

public static class ILGPUAddition
{
    public static void Add(float[] A, float[] B, float[] C)
    {
        // Create a context for ILGPU
        using var context = Context.CreateDefault();
        using var accelerator = context.Devices[2].CreateAccelerator(context);

        // Define the kernel
        var kernel = accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<float>, ArrayView<float>, ArrayView<float>>(AddVectors);

        // Allocate memory on the accelerator
        using var bufferA = accelerator.Allocate1D(A);
        using var bufferB = accelerator.Allocate1D(B);
        using var bufferC = accelerator.Allocate1D<float>(A.Length);

        // Launch the kernel
        kernel((int)A.Length, bufferA.View, bufferB.View, bufferC.View);

        // Copy the result back to the host
        bufferC.CopyToCPU(C);
    }

    // Define the kernel function for vector addition on the GPU
    static void AddVectors(Index1D index, ArrayView<float> A, ArrayView<float> B, ArrayView<float> C)
    {
        C[index] = A[index] + B[index];
    }
}