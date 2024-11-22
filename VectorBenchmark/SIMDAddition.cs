using System.Numerics;

public static class SIMDAddition
{
    public static void Add(float[] A, float[] B, float[] C)
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
}
