namespace Framework.BackgroundJobs.Abstractions
{
    public interface IBackgroundJobNameProvider
    {
        string Name { get; }
    }
}
