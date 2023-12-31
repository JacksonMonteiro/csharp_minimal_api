﻿using MySqlConnector;
using System.Data.SqlClient;
using static TaskAPI.Data.TaskAPIContext;

namespace TaskAPI.Extensions {
    public static class ServiceCollectionExtensions {
        public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder) {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddScoped<GetConnection>(sp => async () => {
                var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();
                return connection;
            });

            return builder;
        }
    }
}
