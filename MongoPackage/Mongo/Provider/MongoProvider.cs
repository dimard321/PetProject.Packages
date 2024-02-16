using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace MongoPackage.Mongo.Provider
{
    public class MongoProvider
    {
        public MongoClient Client { get; private set; }
        public IMongoDatabase Database { get; private set; }

        public MongoProvider(IOptions<MongoProviderConfiguration> options)
        {
            Init(options);
        }

        private void Init(IOptions<MongoProviderConfiguration> options)
        {
            var configuration = options.Value ?? throw new ArgumentException("MongoProviderConfiguration NotFound");

            var clientSettings = MongoClientSettings.FromConnectionString(configuration.ConnectionString);

            #region Custom Settings
            //clientSettings.ClusterConfigurator = cb =>
            //{
            //    if (configuration.TraceCommands)
            //    {
            //        var traceSource = new TraceSource(nameof(MongoProvider), SourceLevels.Verbose);
            //        cb.TraceCommandsWith(traceSource);
            //    }

            //    if (configuration.UseApm)
            //        cb.Subscribe(new MongoDbEventSubscriber());
            //};

            //if (configuration.DisableInvalidBytesThrowsOnReadEncoding)
            //{
            //    clientSettings.ReadEncoding = Utf8Encodings.Lenient;
            //}
            #endregion

            ConfigureClientSettings(configuration, clientSettings);

            Client = new MongoClient(clientSettings);

            Database = Client.GetDatabase(configuration.Database);
        }

        protected virtual void ConfigureClientSettings(MongoProviderConfiguration configuration, MongoClientSettings clientSettings) { }

        public static MongoProvider Create(string connectionString, string database)
        {
            var mongoProviderConfiguration = new MongoProviderConfiguration
            {
                ConnectionString = connectionString,
                Database = database
            };

            var options = Options.Create(mongoProviderConfiguration);

            return new MongoProvider(options);
        }
    }
}
