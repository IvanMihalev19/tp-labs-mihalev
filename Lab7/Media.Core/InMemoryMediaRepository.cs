using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Core
{
    public sealed class InMemoryMediaRepository : IMediaRepository
    {
        private readonly List<MediaFile> _files = [];

        public void Save(IEnumerable<MediaFile> files)
        {
            ArgumentNullException.ThrowIfNull(files);
            _files.Clear();
            _files.AddRange(files);
        }

        public IReadOnlyList<MediaFile> Load() => _files.AsReadOnly();

        public void Clear() => _files.Clear();
    }
}
