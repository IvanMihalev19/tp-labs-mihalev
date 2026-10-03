using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Media.Core;

namespace Media.Tests
{
    public class MediaCatalogServiceTests
    {
        private static MediaCatalogService CreateService(params MediaFile[] initial)
        {
            var repo = new InMemoryMediaRepository();
            if (initial.Length > 0)
                repo.Save(initial);
            return new MediaCatalogService(repo);
        }

        [Fact]
        public void GetByType_FiltersCorrectly()
        {
            var service = CreateService(
                new MediaFile("/s.mp3", 10),
                new MediaFile("/v.mp4", 20),
                new MediaFile("/i.jpg", 30),
                new MediaFile("/s2.wav", 40));

            Assert.Equal(2, service.GetByType(MediaType.Audio).Count);
            Assert.Single(service.GetByType(MediaType.Video));
            Assert.Single(service.GetByType(MediaType.Image));
        }

        [Fact]
        public void Search_FindsByFileName()
        {
            var service = CreateService(
                new MediaFile("/music/rock_anthem.mp3", 1),
                new MediaFile("/video/movie.mp4", 2),
                new MediaFile("/pics/rock_photo.jpg", 3));

            var found = service.Search("rock");
            Assert.Equal(2, found.Count);
        }

        [Fact]
        public void GetStats_ComputesCorrectly()
        {
            var service = CreateService(
                new MediaFile("/a.mp3", 100),
                new MediaFile("/b.mp3", 200),
                new MediaFile("/c.mp4", 1000),
                new MediaFile("/d.png", 50));

            var stats = service.GetStats();
            Assert.Equal(4, stats.TotalFiles);
            Assert.Equal(1350, stats.TotalSizeBytes);
            Assert.Equal(2, stats.AudioCount);
            Assert.Equal(1, stats.VideoCount);
            Assert.Equal(1, stats.ImageCount);
        }

        [Fact]
        public void Clear_RemovesAll()
        {
            var service = CreateService(new MediaFile("/x.mp3", 1));
            service.Clear();
            Assert.Empty(service.AllFiles);
        }
    }
}
