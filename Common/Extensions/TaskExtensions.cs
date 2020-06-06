using System.Threading.Tasks;

namespace Common.Extensions
{
	public static class TaskExtensions
	{
		public static TResult ToSync<TResult>(this Task<TResult> task)
		{
			return task.GetAwaiter().GetResult();
		}
	}
}
