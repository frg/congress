using Congress.App.Helpers;
using Congress.Data;
using Congress.Data.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Congress.App.Controllers
{
    public class PostController : ApiController
    {
        private readonly MongoDBConnection db = new MongoDBConnection();

        public PostController()
        {
            //db.Posts.RemoveAll();
            //new Seeder(db).Posts();
        }

        // GET: api/Post
        public IEnumerable<Post> Get()
        {
            return PostShepherd.GetTop(db);
        }

        // GET: api/Post/5
        public Post Get(string id)
        {
            return PostShepherd.Get(db, id);
        }

        // POST: api/Post
        public bool Post(SetterPost setterPost)
        {
            setterPost.CreatedByUID = Helper.GenerateClientUID();
            return (PostShepherd.Create(db, setterPost) != null);
        }

        // PUT: api/Post/5
        //public void Put(int id, [FromBody]string value) { }

        // DELETE: api/Post/5
        public bool Delete(string id)
        {
            return (PostShepherd.Delete(db, id) != null);
        }
    }
}
