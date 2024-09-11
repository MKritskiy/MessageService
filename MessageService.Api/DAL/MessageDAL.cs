using MessageService.Api.Models;
using Npgsql;

namespace MessageService.Api.DAL
{
    public class MessageDAL : IMessageDAL
    {
        private readonly string _connectionString;

        public MessageDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<int> SaveMessageAsync(Message message)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await using (var cmd = new NpgsqlCommand(@"INSERT INTO Message (Text, Timestamp, SequenceNumber) 
                                                           VALUES (@Text, @Timestamp, @SequenceNumber)
                                                           RETURNING Id", connection))
                {
                    cmd.Parameters.AddWithValue("Text", message.Text);
                    cmd.Parameters.AddWithValue("Timestamp", message.Timestamp);
                    cmd.Parameters.AddWithValue("SequenceNumber", message.SequenceNumber);
                    return (int) (await  cmd.ExecuteScalarAsync() ?? 0);

                }
            }
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(DateTime startDate, DateTime endDate)
        {
            List<Message> messages = new List<Message>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await using (var cmd = new NpgsqlCommand(@"SELECT * FROM Message 
                                                    WHERE Timestamp BETWEEN @StartDate AND @EndDate", connection))
                {
                    cmd.Parameters.AddWithValue("StartDate", startDate);
                    cmd.Parameters.AddWithValue("EndDate", endDate);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            messages.Add(new Message
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Text = reader.GetString(reader.GetOrdinal("Text")),
                                Timestamp = reader.GetDateTime(reader.GetOrdinal("Timestamp")),
                                SequenceNumber = reader.GetInt32(reader.GetOrdinal("SequenceNumber"))
                            });
                        }
                    }
                }
                return messages;
            }

        }
    }
}
