using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Media.Core;

namespace Media.Tests
{
    public class InMemoryMediaRepositoryTests
    {
        [Fact]
        public void SaveAndLoad_RoundTrip()
        {
            var repo = new InMemoryMediaRepository();
            var files = new[]
            {
            new MediaFile("/a/song.mp3", 100),
            new MediaFile("/b/clip.mp4", 200)
        };

            repo.Save(files);
            var loaded = repo.Load();

            Assert.Equal(2, loaded.Count);
            Assert.Equal("song.mp3", loaded[0].FileName);
            Assert.Equal(MediaType.Video, loaded[1].Type);
        }

        [Fact]
        public void Clear_EmptiesStorage()
        {
            var repo = new InMemoryMediaRepository();
            repo.Save([new MediaFile("/x.jpg", 10)]);
            repo.Clear();
            Assert.Empty(repo.Load());
        }
    }
}
