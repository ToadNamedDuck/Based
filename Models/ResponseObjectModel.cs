using System.Text.Json.Serialization;

namespace Based.Models
{
    public class ResponseObjectModel
    {
        public string id { get; set; }//Id from the API
        public string b { get; set; }//Book Id
        public string c { get; set; }//Chapter Id
        public string v { get; set; }//Verse Id
        public string t { get; set; }//Actual text of the verse.
    }
}
