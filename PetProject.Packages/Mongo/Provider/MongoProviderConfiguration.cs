using System.ComponentModel.DataAnnotations;

namespace MongoPackage.Mongo.Provider
{
    /// <summary>
    /// Bind with MongoDatabase section in appsettings
    /// </summary>
    public class MongoProviderConfiguration
    {
        [Required]
        public string ConnectionString { get; set; }

        [Required]
        public string Database { get; set; }
    }
}
