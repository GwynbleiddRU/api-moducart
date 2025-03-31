// using System.Collections.Generic;
// using MongoDB.Driver;
// using ProductService.API.Models;

// namespace ProductService.API.Data
// {
//     public class ApplicationDbContext
//     {
//         private readonly IMongoDatabase _database;

//         public ApplicationDbContext(IMongoClient mongoClient, string databaseName)
//         {
//             _database = mongoClient.GetDatabase(databaseName);
//         }

//         public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
//         public IMongoCollection<Category> Categories =>
//             _database.GetCollection<Category>("Categories");
//     }
// }
