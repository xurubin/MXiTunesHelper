namespace MXiTunesRemote
{
    using System;
    using System.Collections.Generic;

    public interface IMXiTunes
    {
        List<PicModel> PicMethod { get; }

        int Version { get; }
    }
}

