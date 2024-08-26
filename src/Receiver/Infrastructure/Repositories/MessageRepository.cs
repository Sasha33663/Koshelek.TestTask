using Dapper;
using Domain;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<MessageRepository> _logger;
    public MessageRepository(DbContext dbContext, ILogger<MessageRepository> logger )
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<int> CheckNumberAsync(int number)
    {
        _logger.LogInformation("Проверка наличия номера {Number} в базе данных.", number);

        using (var connection = _dbContext.CreateConnection())
        {
            var query = @"SELECT COUNT(*) FROM messages 
                     WHERE number = @Number;";
            var count = await connection.ExecuteScalarAsync<int>(query, new { Number = number });

            _logger.LogInformation("Найдено {Count} записей с номером {Number}.", count, number);

            return count;
        }
    }
    public async Task CreateMessageAsync(Message message)
    {
        _logger.LogInformation("Создание нового сообщения с номером {Number} в базе данных.", message.Number);

        using (var connection = _dbContext.CreateConnection())
        {
            var query = @"INSERT INTO messages (text,number,date)
                         VALUES (@Text,@Number,@Date);";
            await connection.ExecuteScalarAsync<int>(query, new { Text = message.Text, Number = message.Number, Date = message.Date });

            _logger.LogInformation("Сообщение с номером {Number} успешно создано.", message.Number);
        }
    }

    public async Task<List<Message>> GetMessagesAsync(long time, long lastTenMin)
    {
        _logger.LogInformation("Получение сообщений за последние 10 минут.");

        using (var connection = _dbContext.CreateConnection())
        {
            var query = @"SELECT * FROM messages  
                      WHERE date <= @Date AND date >= @LastTenMin";
            var messages = (await connection.QueryAsync<Message>(query, new { Date = time, LastTenMin = lastTenMin })).ToList();

            _logger.LogInformation("Найдено {Count} сообщений за указанный период.", messages.Count);

            return messages;
        }
    }
}
