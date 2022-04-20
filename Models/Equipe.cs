using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Equipe
    {

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Codigo { get; set; }
        public List<Pessoa> Pessoa { get; set; }
        public Cidade Cidade { get; set; }
    }
}
