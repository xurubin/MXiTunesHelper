using System;
using System.Collections.Generic;

namespace MXiTunesRemote
{

    [Serializable]
    public class PicModel
    {
        public string PicRegex {get; set;}
        public string PicServiceUrl { get; set; }
        public string SearchUrl { get; set; }
    }

}

