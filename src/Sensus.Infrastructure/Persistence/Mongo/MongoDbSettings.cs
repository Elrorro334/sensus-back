namespace Sensus.Infrastructure.Persistence.Mongo;

public sealed class MongoDbSettings
{
    public const string SectionName = "MongoDbSettings";

    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = "SensusDb";
    public string UsersCollectionName { get; set; } = "usuarios";
}
