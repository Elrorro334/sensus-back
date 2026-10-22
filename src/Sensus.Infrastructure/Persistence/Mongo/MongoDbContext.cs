using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sensus.Infrastructure.Persistence.Mongo.Documents;

namespace Sensus.Infrastructure.Persistence.Mongo;

/// <summary>
/// Contexto de conexión hacia MongoDB Atlas.
/// </summary>
public sealed class MongoDbContext
{
    private readonly IMongoDatabase? _database;
    private readonly ILogger<MongoDbContext> _logger;
    private readonly MongoDbSettings _settings;

    public MongoDbContext(
        IOptions<MongoDbSettings> settings,
        ILogger<MongoDbContext> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        if (string.IsNullOrWhiteSpace(_settings.ConnectionString))
        {
            _logger.LogWarning("MongoDB ConnectionString no está configurada. Operando en modo local/desconectado en memoria.");
            _database = null;
        }
        else
        {
            var client = new MongoClient(_settings.ConnectionString);
            _database = client.GetDatabase(_settings.DatabaseName);
            ConfigureIndexes();
        }
    }

    public IMongoCollection<UserDocument>? Users =>
        _database?.GetCollection<UserDocument>(_settings.UsersCollectionName);

    private void ConfigureIndexes()
    {
        if (Users is null) return;

        try
        {
            var indexKeysDefinition = Builders<UserDocument>.IndexKeys.Ascending(u => u.Correo);
            var indexOptions = new CreateIndexOptions { Unique = true };
            var indexModel = new CreateIndexModel<UserDocument>(indexKeysDefinition, indexOptions);

            Users.Indexes.CreateOne(indexModel);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Nota: El índice único de correos ya existe o el clúster aún no está disponible.");
        }
    }
}
