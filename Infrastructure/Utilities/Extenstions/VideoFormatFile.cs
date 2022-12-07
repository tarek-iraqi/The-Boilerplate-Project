using FileSignatures;

namespace Utilities.Extenstions;

public class VideoFormatFile : FileFormat
{
    public VideoFormatFile() : base(new byte[] { 0x66, 0x74, 0x79, 0x70 }, "video/mp4", "mp4", 4)
    {

    }
}