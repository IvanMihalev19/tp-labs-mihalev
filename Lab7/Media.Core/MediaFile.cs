using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Core
{
    public sealed class MediaFile
    {
        public string FullPath { get; }
        public string FileName { get; }
        public string Extension { get; }
        public MediaType Type { get; }
        public long SizeBytes { get; }

        public MediaFile(string fullPath, long sizeBytes)
        {
            if (string.IsNullOrWhiteSpace(fullPath))
                throw new ArgumentException("Путь не может быть пустым.", nameof(fullPath));

            FullPath = Path.GetFullPath(fullPath);
            FileName = Path.GetFileName(FullPath);
            Extension = Path.GetExtension(FullPath).ToLowerInvariant();
            Type = MediaTypeResolver.Resolve(Extension);
            SizeBytes = sizeBytes < 0 ? 0 : sizeBytes;
        }

        public override string ToString() =>
            $"{FileName} [{Type}] ({SizeBytes} байт)";
    }
}
