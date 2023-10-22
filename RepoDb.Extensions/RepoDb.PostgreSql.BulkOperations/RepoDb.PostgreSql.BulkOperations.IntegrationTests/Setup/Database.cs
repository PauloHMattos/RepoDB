﻿using Npgsql;
using System;

namespace RepoDb.IntegrationTests.Setup
{
    /// <summary>
    /// A class used as a startup setup for for RepoDb test database.
    /// </summary>
    public static class Database
    {
        /// <summary>
        /// Initialize the creation of the database.
        /// </summary>
        public static void Initialize()
        {
            // Set the connection string
            ConnectionStringForPosgres = Environment.GetEnvironmentVariable("REPODB_CONSTR_POSTGRESDB", EnvironmentVariableTarget.Process) ??
                "Server=127.0.0.1;Port=5433;Database=postgres;User Id=postgres;Password=admin;";
            ConnectionStringForRepoDb = Environment.GetEnvironmentVariable("REPODB_CONSTR", EnvironmentVariableTarget.Process)??
                "Server=127.0.0.1;Port=5433;Database=RepoDb;User Id=postgres;Password=admin;";
            
            // Initialize PostgreSql
            GlobalConfiguration.Setup().UsePostgreSql();

            // Create databases
            CreateDatabase();

            // Create tables
            CreateTables();
        }

        /// <summary>
        /// Gets or sets the connection string to be used for Postgres database.
        /// </summary>
        public static string ConnectionStringForPosgres { get; private set; }

        /// <summary>
        /// Gets or sets the connection string to be used.
        /// </summary>
        public static string ConnectionStringForRepoDb { get; private set; }

        #region Methods

        /// <summary>
        /// Creates a test database for RepoDb.
        /// </summary>
        public static void CreateDatabase()
        {
            using (var connection = new NpgsqlConnection(ConnectionStringForPosgres))
            {
                var recordCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM pg_database WHERE datname = 'RepoDb';");
                if (recordCount <= 0)
                {
                    connection.ExecuteNonQuery(@"CREATE DATABASE ""RepoDb""
                        WITH OWNER = ""postgres""
                        ENCODING = ""UTF8""
                        CONNECTION LIMIT = -1;");
                }
            }
        }

        /// <summary>
        /// Create the necessary tables for testing.
        /// </summary>
        public static void CreateTables()
        {
            CreateBulkOperationIdentityTable();
            CreateEnumTable();
        }

        /// <summary>
        /// Clean up all the table.
        /// </summary>
        public static void Cleanup()
        {
            using (var connection = new NpgsqlConnection(ConnectionStringForRepoDb))
            {
                connection.Truncate("BulkOperationIdentityTable");
                connection.Truncate("EnumTable");
            }
        }

        #endregion

        #region BulkOperationIdentityTable

        /// <summary>
        /// Creates an identity table that has some important fields. All fields are nullable.
        /// </summary>
        public static void CreateBulkOperationIdentityTable()
        {
            var commandText = @"CREATE TABLE IF NOT EXISTS public.""BulkOperationIdentityTable""
                (
                        ""Id"" bigint GENERATED ALWAYS AS IDENTITY,
                        ""ColumnChar"" ""char"",
                        ""ColumnBigInt"" bigint,
                        ""ColumnBit"" bit(1),
                        ""ColumnBoolean"" boolean,
                        ""ColumnDate"" date,
                        ""ColumnInteger"" integer,
                        ""ColumnMoney"" money,
                        ""ColumnNumeric"" numeric,
                        ""ColumnReal"" real,
                        ""ColumnSerial"" integer,
                        ""ColumnSmallInt"" smallint,
                        ""ColumnSmallSerial"" smallint,
                        ""ColumnText"" text COLLATE pg_catalog.""default"",
                        ""ColumnTimeWithTimeZone"" time with time zone,
                        ""ColumnTimeWithoutTimeZone"" time without time zone,
                        ""ColumnTimestampWithTimeZone"" timestamp with time zone,
                        ""ColumnTimestampWithoutTimeZone"" timestamp without time zone,
                        CONSTRAINT ""BulkOperationIdentityTable_PrimaryKey"" PRIMARY KEY (""Id"")
                    )

                    TABLESPACE pg_default;

                    ALTER TABLE public.""BulkOperationIdentityTable""
                        OWNER to postgres;";
            using (var connection = new NpgsqlConnection(ConnectionStringForRepoDb))
            {
                connection.ExecuteNonQuery(commandText);
            }
        }

        #endregion

        #region EnumTable

        private static void CreateEnumTable()
        {
            using (var connection = new NpgsqlConnection(ConnectionStringForRepoDb))
            {
                connection.ExecuteNonQuery(@"
                    DO $$
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'hand') THEN
                            CREATE TYPE hand AS ENUM ('Unidentified', 'Left', 'Right');
                        END IF;
                    END
                    $$;

                    CREATE TABLE IF NOT EXISTS public.""EnumTable""
                    (
                        ""Id"" bigint GENERATED ALWAYS AS IDENTITY,
                        ""ColumnEnumText"" text null COLLATE pg_catalog.""default"",
                        ""ColumnEnumInt"" integer null,
                        ""ColumnEnumHand"" hand null,
                        CONSTRAINT ""EnumTable_PrimaryKey"" PRIMARY KEY (""Id"")
                    );");
                connection.ReloadTypes();
            }
        }

        #endregion
    }
}
