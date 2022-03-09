using System;
using System.Collections.Generic;
using System.Text;

namespace CC_Proj1_Alireza.Domains
{
    public class CommentEntity
    {
        public CommentEntity(string movieId, string comment)
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            MovieId = movieId;
            Comment = comment;
        }

        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string MovieId { get; set; }
        public string Comment { get; set; }

    }
}