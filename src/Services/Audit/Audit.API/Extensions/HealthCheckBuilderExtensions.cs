using System;
using Microsoft.Extensions.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;

namespace Audit.API.Extensions
{
    public static  class HealthCheckBuilderExtensions
    {
        public static HealthCheckBuilder AddMongoDbCheck(this HealthCheckBuilder builder, string name, string connectionString)
        {
            return AddMongoDbCheck(builder, name, connectionString, builder.DefaultCacheDuration);
        }

        public static HealthCheckBuilder AddMongoDbCheck(this HealthCheckBuilder builder, string name, string connectionString, TimeSpan cacheDuration)
        {
            builder.AddCheck($"MongoDbCheck({name})", async () =>
            {
                try
                {
                    var client = new MongoClient(connectionString);
                    var databases = await client.ListDatabasesAsync();
                    
                    // Force MongoDB to connect to the database.
                    await databases.MoveNextAsync(); 

                    return client.Cluster.Description.State == ClusterState.Connected
                        ? HealthCheckResult.Healthy($"MongoDbCheck({name}): Healthy")
                        : HealthCheckResult.Unhealthy($"MongoDbCheck({name}): Unhealthy");
                }
                catch (Exception exception)
                {
                    return HealthCheckResult.Unhealthy($"MongoDbCheck({name}): Exception during check: {exception.GetType().FullName}");
                }
            }, cacheDuration);

            return builder;
        }
    }
}