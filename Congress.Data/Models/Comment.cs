using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Congress.Data.Models
{
    public class SetterComment
    {
        public SetterComment(string postId, string createdByUID, string content, string commentId = null)
        {
            PostId = postId;
            CommentId = commentId;
            CreatedByUID = createdByUID;
            Content = content;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string CommentId { get; set; }

        public string CreatedByUID { get; set; }
        public string Content { get; set; }
    }

    public class Comment
    {
        public Comment() { CreatedAt = DateTime.UtcNow; }

        public Comment(SetterComment SetterComment)
        {
            PostId = SetterComment.PostId;
            CommentId = SetterComment.CommentId;
            CreatedByUID = SetterComment.CreatedByUID;
            Content = SetterComment.Content;
            CreatedAt = DateTime.UtcNow;
            DeletedAt = null;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string CommentId { get; set; }

        public string CreatedByUID { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public static class CommentShepherd
    {
        public static IEnumerable<Comment> Get(MongoDBConnection db, string uid)
        {
            return db.Comments.FindAll().Where(i => i.CreatedByUID == uid);
        }

        public static WriteConcernResult Delete(MongoDBConnection db, string id)
        {
            return db.Comments.Remove(Query.EQ("_id", id));
        }

        //public static WriteConcernResult Create(MongoDBConnection db, SetterComment SetterComment)
        //{
        //    // TODO
        //}

        public static Comment IsUniqueComment(MongoDBConnection db, SetterComment SetterComment)
        {
            if (db.Comments.Count() > 0)
            {
                return db.Comments.FindOne(Query<Comment>.Where(i => i.PostId == SetterComment.PostId && (i.PostId == SetterComment.PostId && i.CreatedByUID == SetterComment.CreatedByUID)));
            }

            return null;
        }
    }
}
