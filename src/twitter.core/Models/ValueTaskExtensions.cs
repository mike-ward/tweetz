using System.Threading.Tasks;

namespace twitter.core.Models
{
    public static class ValueTaskExtensions
    {
        public static ValueTask AsValueTask<T>(this ValueTask<T> valueTask)
        {
            if (valueTask.IsCompletedSuccessfully)
            {
                valueTask.GetAwaiter().GetResult();
                return default;
            }

            return new ValueTask(valueTask.AsTask());
        }
    }
}