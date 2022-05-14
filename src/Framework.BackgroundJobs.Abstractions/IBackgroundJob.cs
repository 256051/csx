using System.Threading.Tasks;

namespace Framework.BackgroundJobs.Abstractions
{
    public interface IAsyncBackgroundJob<in TArgs>
    {
        Task ExecuteAsync(TArgs args);
    }
}
