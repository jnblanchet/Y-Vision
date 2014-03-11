namespace Y_API.DetectionAPI.MessageObjects
{
    public class EmptyFrameMessage : IStringEncodable
    {
        public string Encode()
        {
            return "empty";
        }
    }
}
