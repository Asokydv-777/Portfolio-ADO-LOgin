using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Portfolio.Models
{
    public class ContactDataAccessLayer
    {
        private readonly string _connectionString;

        public ContactDataAccessLayer(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public List<ContactMessage> GetAllMessages()
        {
            var messages = new List<ContactMessage>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM ContactMessages ORDER BY SubmittedAt DESC", con);
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                messages.Add(new ContactMessage
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    Message = reader["Message"].ToString()!,
                    SubmittedAt = (DateTime)reader["SubmittedAt"]
                });
            }
            return messages;
        }

        public ContactMessage? GetMessageById(int id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM ContactMessages WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new ContactMessage
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    Message = reader["Message"].ToString()!,
                    SubmittedAt = (DateTime)reader["SubmittedAt"]
                };
            }
            return null;
        }

        public void AddMessage(ContactMessage message)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(
                "INSERT INTO ContactMessages (Name, Email, Message, SubmittedAt) VALUES (@Name, @Email, @Message, @SubmittedAt)",
                con);
            cmd.Parameters.AddWithValue("@Name", message.Name);
            cmd.Parameters.AddWithValue("@Email", message.Email);
            cmd.Parameters.AddWithValue("@Message", message.Message);
            cmd.Parameters.AddWithValue("@SubmittedAt", message.SubmittedAt);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteMessage(int id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("DELETE FROM ContactMessages WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}