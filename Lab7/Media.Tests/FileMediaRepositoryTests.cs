using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Media.Core;

namespace Media.Tests
{
    public class FileMediaRepositoryTests : IDisposable
    {
        private readonly string _tempFile;

        public FileMediaRepositoryTests()
        {
            _tempFile = Path.Combine(Path.GetTempPath(), $"media_test_{Guid.NewGuid():N}.txt");
        }

        public void Dispose()
        {
            if (File.Exists(_tempFile))
                File.Delete(_tempFile);
        }

        [Fact]
        public void SaveAndLoad_RoundTrip()
        {
            var repo = new FileMediaRepository(_tempFile);
            var files = new[]
            {
            new MediaFile("/data/track.flac", 5000),
            new MediaFile("/data/photo.webp", 800)
        };

            repo.Save(files);
            var loaded = repo.Load();

            Assert.Equal(2, loaded.Count);
            Assert.Equal(MediaType.Audio, loaded[0].Type);
            Assert.Equal(MediaType.Image, loaded[1].Type);
        }
    }
}
