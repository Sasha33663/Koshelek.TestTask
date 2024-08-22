using Dapper;
using Domain;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace Infrastructure;
public static class Seeder
{
    public static void  Start(DbContext dbContext)
    {
        using (var connection = dbContext.CreateConnection())
        {
            var createTableSql = @"CREATE TABLE IF NOT EXISTS ASD
                                    (
                                        Id SERIAL PRIMARY KEY,
                                        Column1 VARCHAR(50),
                                        Column2 INT
                                    );";
            var result = connection.Execute(createTableSql);

        }
        using (var connection = dbContext.CreateConnection())
        {
            var createTableSql = @"INSERT INTO messages (text, number)
                                    SELECT 'Тес213ные', 222222
                                    WHERE NOT EXISTS (
                                    SELECT 1 
                                    FROM messages 
                                    WHERE text = 'Тестовые данные' AND number = 111111
                                    );";
            var result = connection.Execute(createTableSql);

        }
    }
}
