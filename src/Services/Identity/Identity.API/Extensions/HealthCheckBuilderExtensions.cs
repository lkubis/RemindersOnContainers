using System;
using System.Data;
using Microsoft.Extensions.HealthChecks;
using Npgsql;

namespace Identity.API.Extensions
{
    public static class HealthCheckBuilderExtensions
    {
        public static HealthCheckBuilder AddPostgreSqlCheck(this HealthCheckBuilder builder, string name, string connectionString)
        {
            return AddPostgreSqlCheck(builder, name, connectionString, builder.DefaultCacheDuration);
        }

        public static HealthCheckBuilder AddPostgreSqlCheck(this HealthCheckBuilder builder, string name, string connectionString, TimeSpan cacheDuration)
        {
            builder.AddCheck($"PostgreSqlCheck({name})", async () =>
            {
                try
                {
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        return connection.State == ConnectionState.Open
                            ? HealthCheckResult.Healthy($"PostgreSqlCheck({name}): Healthy")
                            : HealthCheckResult.Unhealthy($"PostgreSqlCheck({name}): Unhealthy");
                    }
                }
                catch (Exception exception)
                {
                    return HealthCheckResult.Unhealthy($"PostgreSqlCheck({name}): Exception during check: {exception.GetType().FullName}");
                }
            }, cacheDuration);

            return builder;
        }
    }
}