public class Sort
{
    public delegate bool Condition<T>(T a, T b);

    public static void BubbleSort<T>(T[] arr, Condition<T> condition)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
            for (int j = 0; j < n - i - 1; j++)
                // Use delegate instead of normal condition
                if (condition(arr[j], arr[j + 1]))
                {
                    T temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
    }
}
