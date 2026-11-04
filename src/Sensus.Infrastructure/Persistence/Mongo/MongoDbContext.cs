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
            _logger.LogWarning("MongoDB ConnectionString no está configurada.");
            _database = null;
        }
        else
        {
            var client = new MongoClient(_settings.ConnectionString);
            _database = client.GetDatabase(_settings.DatabaseName);
        }

        ConfigureIndexes();
    }

    public IMongoCollection<UserDocument> Users =>
        _database?.GetCollection<UserDocument>(_settings.UsersCollectionName)
        ?? throw new InvalidOperationException("La conexión a MongoDB Atlas no está configurada. Configure 'MongoDbSettings:ConnectionString'.");

    public IMongoCollection<ComercioDocument> Comercios =>
        _database?.GetCollection<ComercioDocument>(_settings.ComerciosCollectionName)
        ?? throw new InvalidOperationException("La conexión a MongoDB Atlas no está configurada. Configure 'MongoDbSettings:ConnectionString'.");

    public IMongoCollection<PasswordResetTokenDocument> PasswordResetTokens =>
        _database?.GetCollection<PasswordResetTokenDocument>(_settings.PasswordResetTokensCollectionName)
        ?? throw new InvalidOperationException("La conexión a MongoDB Atlas no está configurada. Configure 'MongoDbSettings:ConnectionString'.");

    private void ConfigureIndexes()
    {
        if (_database is null) return;

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

        try
        {
            var comerciosCollection = _database.GetCollection<ComercioDocument>(_settings.ComerciosCollectionName);

            // Índice único para UsuarioId (un usuario tiene un solo comercio activo)
            var userIndexKeys = Builders<ComercioDocument>.IndexKeys.Ascending(c => c.UsuarioId);
            var userIndexOptions = new CreateIndexOptions { Unique = true };
            comerciosCollection.Indexes.CreateOne(new CreateIndexModel<ComercioDocument>(userIndexKeys, userIndexOptions));

            // Índice geoespacial 2dsphere para consultas geográficas
            var geoIndexKeys = Builders<ComercioDocument>.IndexKeys.Geo2DSphere(c => c.Ubicacion);
            comerciosCollection.Indexes.CreateOne(new CreateIndexModel<ComercioDocument>(geoIndexKeys));
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Nota: Los índices de comercios ya existen o el clúster aún no está disponible.");
        }

        try
        {
            var tokensCollection = _database.GetCollection<PasswordResetTokenDocument>(_settings.PasswordResetTokensCollectionName);

            // Índice para TokenHash
            var hashIndexKeys = Builders<PasswordResetTokenDocument>.IndexKeys.Ascending(t => t.TokenHash);
            tokensCollection.Indexes.CreateOne(new CreateIndexModel<PasswordResetTokenDocument>(hashIndexKeys));

            // Índice TTL en ExpiresAtUtc
            var ttlIndexKeys = Builders<PasswordResetTokenDocument>.IndexKeys.Ascending(t => t.ExpiresAtUtc);
            var ttlOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.Zero };
            tokensCollection.Indexes.CreateOne(new CreateIndexModel<PasswordResetTokenDocument>(ttlIndexKeys, ttlOptions));
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Nota: Los índices de tokens ya existen o el clúster aún no está disponible.");
        }
    }
}
