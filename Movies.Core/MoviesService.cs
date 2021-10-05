using MongoDB.Driver;
using Movies.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Core
{
    public class MoviesService
    {
        private MongoClient InitializeClient()
        {
            var client = new MongoClient("mongodb://user:pass1234@cluster0-shard-00-00.vr1tq.mongodb.net:27017,cluster0-shard-00-01.vr1tq.mongodb.net:27017,cluster0-shard-00-02.vr1tq.mongodb.net:27017/myFirstDatabase?ssl=true&replicaSet=atlas-mxqr3g-shard-0&authSource=admin&retryWrites=true&w=majority");
            return client;
        }

        public async Task<List<MoviesModel>> GetAllMovies()
        {
            var client = InitializeClient();
            var database = client.GetDatabase("sample_mflix");
            var collection = database.GetCollection<MoviesModel>("movies");
            var response = collection.Find(movie => movie.Year >= 2005).ToListAsync();

            return await response;
        }


        public async Task<List<MoviesModel>> GetSpecificMovie(string name)
        {
            var client = InitializeClient();
            var database = client.GetDatabase("sample_mflix");
            var collection = database.GetCollection<MoviesModel>("movies");
            var response = collection.Find(movie => movie.Title.ToLower() == name.ToLower()).ToListAsync();

            return await response;
        }
    }
}
