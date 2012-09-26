using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace MXiTunesRemote
{
    // TODO: To implement additional album art downloader, instantiate this interface and write
    // code for the appropriate methods. See DefaultAlbumArtRetriever for examples. 
    // Do not forget to register the new downloader at Form1_Load().
    interface AlbumArtRetriever
    {
        string getName();
        IEnumerable<AlbumInfo> retrieve(string Artist, string AlbumName);
    }

    class AlbumArtRetrieverManager
    {
        private static Dictionary<string, AlbumArtRetriever> retrievers = new Dictionary<string, AlbumArtRetriever>();
        private static AlbumArtRetriever curRetriever = null;
        public static void registerRetriever(AlbumArtRetriever retriever)
        {
            retrievers[retriever.getName()] = retriever;
            if (curRetriever == null)
                curRetriever = retriever;
        }
        public static IEnumerable<string> getRetrieverList()
        {
            return retrievers.Keys;
        }

        // TODO: Maybe we should support searching from more than one retriever at the same time, which 
        // would require a few changes in this class, plus minor tweaks in UI and AlbumArtServer
        public static void SelectRetriever(string Name)
        {
            curRetriever = retrievers[Name];
        }

        public static AlbumArtRetriever getSelectedRetriever()
        {
            return curRetriever;
        }
    }

    class AlbumInfo
    {
        public string Artist;
        public string AlbumName;
        // Something to keep in mind: MXiTunes internally does a string substitution of "s100_"->"s300_"
        // on image url, so the url you feed to it may not be the one it really downloads.
        public string AlbumArtURL;
    }

    // Reimplementation of the default album art downloader in MXiTunes with latest regex fixes.
    class DefaultAlbumArtRetriever : AlbumArtRetriever
    {
        private const string PicRegex = "http://img.1ting.com/images/special(?<url>.+?)\"([\\w\\W]*?)class=\"albumName\">([\\w\\W]*?)(?<Album>.+?)</span>([\\w\\W]*?)class=\"singerName\">([\\w\\W]*?)>(?<Artist>.+?)</a>";
        public string getName()
        {
            return "MXiTunes默认搜索引擎";
        }

        public IEnumerable<AlbumInfo> retrieve(string Artist, string AlbumName)
        {
            try
            {
                WebClient client = new WebClient ();
                client.Headers.Add ("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                Stream data = client.OpenRead (String.Format("http://so.1ting.com/album.do?q={0}+{1}", HttpUtility.UrlEncode(AlbumName), HttpUtility.UrlEncode(Artist)));
                StreamReader reader = new StreamReader (data);
                string content = reader.ReadToEnd ();
                data.Close ();
                reader.Close ();

                List<AlbumInfo> result = new List<AlbumInfo>();
                foreach (Match match in Regex.Matches(content, PicRegex))
                {
                    // We do not need to do much filtering here because MXiTunes will handle it anyway.
                    // MXiTunes will look for an exact match (up to lower characters) of artist name and album name,
                    // if that is the case then the album  art is picked up automatically. Otherwise a dialog will pop
                    // up to allow the user to pick manually.

                    AlbumInfo album = new AlbumInfo();
                    album.AlbumArtURL = "http://img.1ting.com/images/special" + match.Groups["url"].ToString();
                    album.AlbumName = match.Groups["Album"].ToString();
                    album.Artist = match.Groups["Artist"].ToString();
                    result.Add(album);
                }
                return result;
            } catch(Exception e)
            {
                Form1.LogInfo(String.Format("{0}: Exception while retrieving - {1}", getName(), e.ToString()));
            }
            return new List<AlbumInfo>();
        }
    }
}
