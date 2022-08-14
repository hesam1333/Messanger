namespace Messenger.Brockers
{
    public interface ISerializationBroker
    {
        string Serilize<T>(T obj);
        T Deserilize<T>(string obj);
    }
}
