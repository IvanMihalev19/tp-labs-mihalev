using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Core
{
    public sealed class MediaCatalogService
    {
        private readonly IMediaRepository _repository;
        private List<MediaFile> _files = [];

        public MediaCatalogService(IMediaRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _files = _repository.Load().ToList();
        }

        public IReadOnlyList<MediaFile> AllFiles => _files.AsReadOnly();

        public int ScanDirectory(string directoryPath, bool recursive = true)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException("Путь к папке не может быть пустым.", nameof(directoryPath));

            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Папка не найдена: {directoryPath}");

            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var found = new List<MediaFile>();

            foreach (var path in Directory.EnumerateFiles(directoryPath, "*.*", searchOption))
            {
                try
                {
                    var info = new FileInfo(path);
                    var media = new MediaFile(info.FullName, info.Length);
                    if (media.Type != MediaType.Unknown)
                        found.Add(media);
                }
                catch
                {

                }
            }

            var existingPaths = new HashSet<string>(_files.Select(f => f.FullPath), StringComparer.OrdinalIgnoreCase);
            foreach (var f in found)
            {
                if (!existingPaths.Contains(f.FullPath))
                {
                    _files.Add(f);
                    existingPaths.Add(f.FullPath);
                }
            }

            Persist();
            return found.Count;
        }

        public IReadOnlyList<MediaFile> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _files.AsReadOnly();

            var key = keyword.Trim();
            return _files
                .Where(f => f.FileName.Contains(key, StringComparison.OrdinalIgnoreCase)
                         || f.FullPath.Contains(key, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public IReadOnlyList<MediaFile> GetByType(MediaType type) =>
            _files.Where(f => f.Type == type).ToList();

        public CatalogStats GetStats()
        {
            var groups = _files.GroupBy(f => f.Type)
                .ToDictionary(g => g.Key, g => (Count: g.Count(), Size: g.Sum(x => x.SizeBytes)));

            return new CatalogStats(
                TotalFiles: _files.Count,
                TotalSizeBytes: _files.Sum(f => f.SizeBytes),
                AudioCount: groups.GetValueOrDefault(MediaType.Audio).Count,
                VideoCount: groups.GetValueOrDefault(MediaType.Video).Count,
                ImageCount: groups.GetValueOrDefault(MediaType.Image).Count,
                AudioSize: groups.GetValueOrDefault(MediaType.Audio).Size,
                VideoSize: groups.GetValueOrDefault(MediaType.Video).Size,
                ImageSize: groups.GetValueOrDefault(MediaType.Image).Size
            );
        }

        public void Clear()
        {
            _files.Clear();
            _repository.Clear();
        }

        public void Persist() => _repository.Save(_files);

        public void LoadFromRepository()
        {
            _files = _repository.Load().ToList();
        }
    }

    public sealed record CatalogStats(
        int TotalFiles,
        long TotalSizeBytes,
        int AudioCount,
        int VideoCount,
        int ImageCount,
        long AudioSize,
        long VideoSize,
        long ImageSize);

}
