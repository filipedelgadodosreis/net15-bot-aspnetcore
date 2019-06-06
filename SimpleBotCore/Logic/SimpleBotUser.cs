using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace SimpleBotCore.Logic
{
    public class SimpleBotUser
    {
        private int contador = 0;
        public string Reply(SimpleMessage message)
        {
            SaveMongoDB(message);

            return $"Id: {message.Id} Usuario:{message.User} disse '{message.Text}'. Foram enviadas {GetUserByID(message)} mensagens por este usuario";
        }

        public void SaveMongoDB(SimpleMessage message)
        {
            var client = new MongoClient("mongodb://localhost:27017");

            var db = client.GetDatabase("15net");
            var col = db.GetCollection<BsonDocument>("botcount");

            var doc = new BsonDocument()
            {
                { "Id",message.Id },
                { "Usuario",message.User},
                { "Mensagem",message.Text}
            };
            
           col.InsertOne(doc);

            //contador++;
        }

        private int GetUserByID(SimpleMessage message)
        {
            int _userCount = 0;

            var client = new MongoClient("mongodb://localhost:27017");

            var db = client.GetDatabase("15net");
            var col = db.GetCollection<BsonDocument>("botcount");

            var filtro = Builders<BsonDocument>.Filter.Eq("Id", message.Id);
            var res = col.Find(filtro).ToList();            if (res.Any())
                _userCount = res.Count;            return _userCount;            
        }

    }
}