using System;

namespace DDD.Domain.Services
{
    public interface ICurrentUserService : IDisposable
    {
        string UserId { get; } 
    }
}
