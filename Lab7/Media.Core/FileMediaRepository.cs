using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Core
{
    public sealed class FileMediaRepository : IMediaRepository
    {
        private readonly string _filePath;

        public FileMediaRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым.", nameof(filePath));
            _filePath = filePath;
        }

        public void Save(IEnumerable<MediaFile> files)
        {
            ArgumentNullException.ThrowIfNull(files);
            var lines = files.Select(f => $"{f.FullPath}|{f.SizeBytes}");
            File.WriteAllLines(_filePath, lines);
        }

        public IReadOnlyList<MediaFile> Load()
        {
            if (!File.Exists(_filePath))
                return [];

            var result = new List<MediaFile>();
            foreach (var line in File.ReadAllLines(_filePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split('|');
                if (parts.Length < 2) continue;
                if (!long.TryParse(parts[1], out var size)) size = 0;
                try
                {
                    result.Add(new MediaFile(parts[0], size));
                }
                catch
                {
                }
            }
            return result;
        }

        public void Clear()
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }
    }
}
