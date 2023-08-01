namespace Memy.Server.Data.Error
{
    public class Log
    {
        private readonly ILogger _logger;
        private readonly ILogData _data;

        public Log(ILogger logger, ILogData data)
        {
            _logger = logger;
            _data = data;
        }


        public async Task SaveLogError(Exception ex, string? message = null, params string[]? args)
        {
            string? error = ex.Message;
            string? trace = "";
            if (!string.IsNullOrWhiteSpace(message) && args is not null)
            {
                error += Environment.NewLine;
                error += string.Format(message, args);
            }
            else if (!string.IsNullOrWhiteSpace(message))
            {
                error += Environment.NewLine;
                error += message;
            }
            if (!string.IsNullOrWhiteSpace(ex.StackTrace))
            {
                trace = ex.StackTrace;
            }
            try
            {
                await _data.SaveLog(trace, error);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error when log is save");
            }

            if (args is not null)
                _logger.LogError(ex, message, args);
            else
                _logger.LogError(ex, message);



        }
    }
}
