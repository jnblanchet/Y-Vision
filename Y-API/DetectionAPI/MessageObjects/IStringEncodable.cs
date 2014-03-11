namespace Y_API.DetectionAPI.MessageObjects
{
    public interface IStringEncodable
    {
        string Encode();

        // A constructor new(string code) is also strongly recommended to Decode the object
    }
}
