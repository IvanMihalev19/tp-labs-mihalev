using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Core
{
    public interface IMediaRepository
    {
        void Save(IEnumerable<MediaFile> files);
        IReadOnlyList<MediaFile> Load();
        void Clear();
    }
}
