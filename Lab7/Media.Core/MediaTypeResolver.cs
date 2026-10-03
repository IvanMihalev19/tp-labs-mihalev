using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Core
{
    public static class MediaTypeResolver
    {
        private static readonly HashSet<string> AudioExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".mp3", ".wav", ".flac", ".aac", ".ogg", ".wma", ".m4a", ".aiff", ".opus"
    };

        private static readonly HashSet<string> VideoExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm", ".m4v", ".mpeg", ".mpg"
    };

        private static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".tif", ".webp", ".svg", ".ico"
    };

        public static MediaType Resolve(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                return MediaType.Unknown;

            var ext = extension.StartsWith('.') ? extension : "." + extension;

            if (AudioExtensions.Contains(ext)) return MediaType.Audio;
            if (VideoExtensions.Contains(ext)) return MediaType.Video;
            if (ImageExtensions.Contains(ext)) return MediaType.Image;
            return MediaType.Unknown;
        }

        public static IReadOnlyCollection<string> GetExtensions(MediaType type) => type switch
        {
            MediaType.Audio => AudioExtensions,
            MediaType.Video => VideoExtensions,
            MediaType.Image => ImageExtensions,
            _ => Array.Empty<string>()
        };
    }
}
