using System;
using System.Threading.Tasks;

namespace OpenBase.VisualStudio.Services;

public interface ILoggingService
{
    Task LogAsync(string message);
    Task LogErrorAsync(string message, Exception ex = null);
}
