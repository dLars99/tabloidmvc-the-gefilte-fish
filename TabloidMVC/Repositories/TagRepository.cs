﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(IConfiguration config) : base(config) { }
        public void Add(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    // Sql command
                    cmd.CommandText = @"INSERT INTO Tag (Name) VALUES (@name);";

                    //declaring Sql variable
                    cmd.Parameters.AddWithValue("@name", tag.Name);

                    cmd.ExecuteNonQuery();

                }
            }
        }
        public void Update(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Tag
                        SET Name = @name
                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", tag.Id);
                    cmd.Parameters.AddWithValue("@name", tag.Name);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Delete(int tagId)
        {

            //call to delete relevant items from PostTag join table
            DeletePostTag(tagId);
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                        DELETE FROM Tag
                        WHERE Id = @id";

                        cmd.Parameters.AddWithValue("@id", tagId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        public List<Tag> GetAllTags()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT 
                        Id,
                        Name
                    FROM Tag
                    ORDER BY Name
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Tag> tags = new List<Tag>();

                    while (reader.Read())
                    {
                        Tag tag = new Tag
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        tags.Add(tag);
                    }

                    reader.Close();

                    return tags;
                }
            }
        }
        public Tag GetTagById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT 
                            Id,
                            Name
                        FROM Tag
                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if(reader.Read())
                    {
                        
                        Tag tag = new Tag
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        reader.Close();
                        return tag;
                    }

                    reader.Close();
                    return null;

                }
            }
        }
        public void DeletePostTag(int tagId)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                        DELETE FROM PostTag
                        WHERE TagId = @tagId";

                        cmd.Parameters.AddWithValue("@tagId", tagId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        public void AddTagToPost(int tagId, int postId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    // Sql command
                    cmd.CommandText = @"INSERT INTO PostTag (TagId, PostId) VALUES (@TagId, @PostId);";

                    //declaring Sql variable
                    cmd.Parameters.AddWithValue("@TagId", tagId);
                    cmd.Parameters.AddWithValue("@PostId", postId);

                    cmd.ExecuteNonQuery();

                }
            }
        }
    }
}
