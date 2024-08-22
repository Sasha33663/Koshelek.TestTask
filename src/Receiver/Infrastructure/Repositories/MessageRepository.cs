using Dapper;
using Domain;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Repositories;
public class MessageRepository : IMessageRepository
{
    private readonly DbContext _dbContext;
    public MessageRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task <int> CheckNumberAsync(int number)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var query = @"SELECT COUNT(*) FROM messages 
                         WHERE number = @Number;";
            var count = await connection.ExecuteScalarAsync<int>(query, new { Number = number });
            return count;
        }
    }

    public async Task CreateMessageAsync(Message message)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            
            var query = @"INSERT INTO messages (text,number)
                         VALUES (@Text,@Number);";
            await connection.ExecuteScalarAsync<int>(query, new { Text=message.Text, Number = message.Number });
        }
    }
}
