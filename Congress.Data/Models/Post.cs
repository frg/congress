using Congress.Data.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Congress.Data;
using MongoDB.Driver.Builders;

namespace Congress.Data.Models
{
    public class SetterPost
    {
        public SetterPost(string title, string content, string createdByUID)
        {
            Title = title;
            Content = content;
            CreatedByUID = createdByUID;
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatedByUID { get; set; }
    }

    public class Post
    {
        public Post() { CreatedAt = DateTime.UtcNow; }

        public Post(SetterPost setterPost)
        {
            Title = setterPost.Title;
            Content = setterPost.Content;
            VoteCount = 1;
            CreatedByUID = setterPost.CreatedByUID;
            CreatedAt = DateTime.UtcNow;
            DeletedAt = null;
            Archived = false;
        }
        
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int VoteCount { get; set; }
        public string CreatedByUID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Archived { get; set; }

        public double GetScore()
        {
            //http://stackoverflow.com/questions/19219413/popularity-decay-algorithm-for-popular-website-posts
            return (this.VoteCount - 1) / Math.Pow(((DateTime.UtcNow - this.CreatedAt).TotalHours + 2), 1.3);
        }
    }

    public static class PostShepherd
    {
        public static Post Get(MongoDBConnection db, string id)
        {
            return db.Posts.FindOne(Query<Post>.Where(i => i.Id == id));
        }

        /*public static IEnumerable<Post> GetByQuery(MongoDBConnection db, string title)
        {
            return db.Posts.FindAs<Post>(Query<Post>.Where(i => i.Title == title));
        }*/

        public static WriteConcernResult Delete(MongoDBConnection db, string id)
        {
            //return db.Posts.Remove(Query.EQ("_id", id));
            return db.Posts.Update(Query.EQ("_id", new ObjectId(id)), Update<Post>.Set(i => i.DeletedAt, DateTime.UtcNow));
        }

        public static IEnumerable<Post> GetTop(MongoDBConnection db)
        {
            return db.Posts.FindAll().Where(i => i.DeletedAt == null).AsQueryable().OrderByDescending(x => x.GetScore()).ToList();//.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.GetScore()).ToList();
        }

        public static IEnumerable<Post> GetLatest(MongoDBConnection db)
        {
            return db.Posts.FindAs<Post>(Query<Post>.Where(i => i.DeletedAt == null)).SetSortOrder(SortBy<Post>.Descending(i => i.CreatedAt));
        }

        public static IEnumerable<Post> GetHighestVoteCount(MongoDBConnection db)
        {
            return db.Posts.FindAs<Post>(Query<Post>.Where(i => i.DeletedAt == null)).SetSortOrder(SortBy<Post>.Descending(i => i.CreatedAt)).SetSortOrder(SortBy<Post>.Descending(i => i.VoteCount));
        }

        public static WriteConcernResult Create(MongoDBConnection db, SetterPost setterPost)
        {
            if (IsUniquePost(db, setterPost) == null && UserPostsHoursAgo(db, setterPost.CreatedByUID).Count() < 2)
            {
                Post post = new Post(setterPost);
                var createdPostResult = db.Posts.Insert(post);

                // create vote for post by user
                db.Votes.Insert(new SetterVote(post.Id, setterPost.CreatedByUID, 1));

                return createdPostResult;
            }

            return null;
        }

        public static Post IsUniquePost(MongoDBConnection db, SetterPost setterPost)
        {
            if (db.Posts.Count() > 0)
            {
                // use db.collection.find({_id: "myId"}, {_id: 1}).limit(1) instead
                return db.Posts.FindOneAs<Post>(Query<Post>.Where(i => i.Title == setterPost.Title && i.DeletedAt == null));
            }

            return null;
        }

        public static IEnumerable<Post> UserPostsHoursAgo(MongoDBConnection db, string uid, int hoursAgo = 1)
        {
            return db.Posts.FindAllAs<Post>().Where(i => i.DeletedAt == null && i.CreatedByUID == uid && i.CreatedAt > DateTime.UtcNow.AddHours(-hoursAgo));
        }
    }
}