using System.Text.Json;

namespace Pixelz.OrderStore;

public abstract class PixelzEvent
{
    public abstract string EventId { get; }
    public abstract string EventType { get; }

    public byte[] Serialize()
    {
        return JsonSerializer.SerializeToUtf8Bytes(this);
    }

    public static T Deserialize<T>(byte[] data) where T : PixelzEvent
    {
        return JsonSerializer.Deserialize<T>(data) ?? throw new InvalidOperationException("Failed to deserialize event");
    }
}