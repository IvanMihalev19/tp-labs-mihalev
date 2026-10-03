using Media.Core;

namespace Media.Tests;

public class MediaTypeResolverTests
{
    [Theory]
    [InlineData(".mp3", MediaType.Audio)]
    [InlineData(".mp4", MediaType.Video)]
    [InlineData(".jpg", MediaType.Image)]
    [InlineData(".txt", MediaType.Unknown)]
    public void Resolve_ReturnsCorrectType(string extension, MediaType expected)
    {
        Assert.Equal(expected, MediaTypeResolver.Resolve(extension));
    }
}