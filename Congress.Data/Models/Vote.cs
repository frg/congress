using Congress.Data.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Congress.Data.Models
{
    public class SetterVote
    {
        public SetterVote(string postId, string createdByUID, int value)
        {
            PostId = postId;
            CreatedByUID = createdByUID;
            Value = VoteShepherd.SanitizeVoteValue(value);
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }
        public string CreatedByUID { get; set; }
        public int Value { get; set; }
    }

    public class Vote
    {
        public Vote() { CreatedAt = DateTime.UtcNow; }

        public Vote(SetterVote setterVote)
        {
            PostId = setterVote.PostId;
            CreatedByUID = setterVote.CreatedByUID;
            Value = setterVote.Value;
            CreatedAt = DateTime.UtcNow;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }
        //public Post Post { 
        //    get { 
        //        return PostShepherd.Get(this.PostId);
        //    };
        //}
        public string CreatedByUID { get; set; }
        public int Value { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public static class VoteShepherd 
    {
        public static IEnumerable<Vote> Get(MongoDBConnection db, string uid)
        {
            return db.Votes.FindAll().Where(i => i.CreatedByUID == uid);
        }

        public static int SanitizeVoteValue(int voteValue)
        {
            return (voteValue > 1) ? 1 : (voteValue < -1) ? -1 : voteValue;
        }

        public static WriteConcernResult Delete(MongoDBConnection db, string id)
        {
            return db.Votes.Remove(Query.EQ("_id", id));
        }

        public static WriteConcernResult Create(MongoDBConnection db, SetterVote setterVote)
        {
            Vote voteExists = IsUniqueVote(db, setterVote);
            if (voteExists == null)
            {
                // update post vote count so score does not have to be calculated each time
                db.Posts.Update(Query.EQ("_id", new ObjectId(setterVote.PostId)), Update<Post>.Inc(i => i.VoteCount, setterVote.Value));

                return db.Votes.Insert(new Vote(setterVote));
            }
            else
            {
                // if vote exists
                // modify vote
                db.Posts.Update(Query.EQ("_id", new ObjectId(setterVote.PostId)), Update<Post>.Inc(i => i.VoteCount, (-voteExists.Value + setterVote.Value)));

                return db.Votes.Update(Query.EQ("_id", new ObjectId(voteExists.Id)), Update<Vote>.Set(i => i.Value, setterVote.Value));
            }
        }

        public static Vote IsUniqueVote(MongoDBConnection db, SetterVote setterVote)
        {
            if (db.Votes.Count() > 0)
            {
                return db.Votes.FindOne(Query<Vote>.Where(i => i.PostId == setterVote.PostId && (i.PostId == setterVote.PostId && i.CreatedByUID == setterVote.CreatedByUID)));
            }

            return null;
        }
    }
}