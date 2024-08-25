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
            var createTableSql = @"CREATE TABLE IF NOT EXISTS messages
                                    (
                                        number INT,
                                        text TEXT,
                                        date BIGINT 
                                    );";
            var result = connection.Execute(createTableSql);

        }
        using (var connection = dbContext.CreateConnection())
        {
            var createTableSql = @"INSERT INTO messages (text, number,date)
                                    SELECT 'Тестовые данные', 111111,1724417106
                                    WHERE NOT EXISTS (
                                    SELECT 
                                    FROM messages 
                                    WHERE text = 'Тестовые данные' AND number = 111111
                                    );";
            var result = connection.Execute(createTableSql);

        }
    }
}
