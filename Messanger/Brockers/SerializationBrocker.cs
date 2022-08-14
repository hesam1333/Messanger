
using Newtonsoft.Json;

namespace Messenger.Brockers
{
    public class SerializationBrocker : ISerializationBroker
    {
        public T Deserilize<T>(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }

        public string Serilize<T>(T obj)
        {
           return  JsonConvert.SerializeObject(obj);
        }
    }
}
