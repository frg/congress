using Congress.App.Helpers;
using Congress.Data;
using Congress.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Congress.App.Controllers
{
    public class VoteController : ApiController
    {
        private readonly MongoDBConnection db = new MongoDBConnection();

        // GET: api/Vote
        public IEnumerable<Vote> Get()
        {
            // gets all user votes
            return VoteShepherd.Get(db, Helper.GenerateClientUID());
        }

        // POST: api/Vote
        public bool Post(SetterVote setterVote)
        {
            // to delete a vote set the value to 0
            setterVote.CreatedByUID = Helper.GenerateClientUID();
            return (VoteShepherd.Create(db, setterVote) != null);
        }
    }
}
