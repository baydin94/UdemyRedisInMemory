using Microsoft.Extensions.Options;
using StackExchange.Redis;
using StackExchange.RedisAPI.Web.Settings;

namespace StackExchange.RedisAPI.Web.Services
{
    public class RedisService
    {
        private readonly RedisSettings _settings;

        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
        public RedisService(IOptions<RedisSettings> options)
        {
            _settings = options.Value;

            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                var config = new ConfigurationOptions
                {
                    EndPoints = { $"{_settings.Host}:{_settings.Port}" },
                    AbortOnConnectFail = false,
                    ConnectRetry = 5,
                    ConnectTimeout = 5000,
                    KeepAlive = 100,
                    AllowAdmin = true, // Production ortamda false bırakılır, Geliştirme veya test ortamında true kullanılabilir. (güvenlik riski!)

                };

                return ConnectionMultiplexer.Connect(config);
            });
        }

        private ConnectionMultiplexer Connection => _lazyConnection.Value;

        public IDatabase GetDb(int db = 0) => Connection.GetDatabase(db);

        public IServer GetServer()
        {
            var endpoint = Connection.GetEndPoints().First();
            return Connection.GetServer(endpoint);
        }

    }
}
