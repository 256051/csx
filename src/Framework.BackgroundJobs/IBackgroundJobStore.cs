using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.BackgroundJobs
{
    public interface IBackgroundJobStore
    {
        Task<BackgroundJobInfo> FindAsync(Guid jobId);

        Task InsertAsync(BackgroundJobInfo jobInfo);

        Task<IEnumerable<BackgroundJobInfo>> GetWaitingJobsAsync(int maxResultCount);

        Task DeleteAsync(Guid jobId);

        Task UpdateAsync(BackgroundJobInfo jobInfo);
    }
}
