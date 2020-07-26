using Microsoft.Extensions.Hosting;

namespace proj1.Lifetime
{
    public interface IMyApplicationLifetime
    {
        IHostApplicationLifetime HostApplicationLifetime { get; }

        bool IsStarted { get; }

        bool IsStarting { get; }

        bool IsStopped { get; }

        bool IsStopping { get; }
    }
}