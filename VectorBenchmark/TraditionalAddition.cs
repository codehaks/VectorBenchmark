public static class TraditionalAddition
{
    public static void Add(float[] A, float[] B, float[] C)
    {
        for (int i = 0; i < A.Length; i++)
        {
            C[i] = A[i] + B[i];
        }
    }
}
