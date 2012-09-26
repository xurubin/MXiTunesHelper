using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace MXiTunesRemote
{
    class AlbumArtServer
    {
        private static int ServerPort = 7899;
        private static string ServerUrlRoot = String.Format("http://localhost:{0}/", ServerPort);

        // MXiTunes submits query to our server using parameterized <SearchUrl>, {0} for artist, {1} for album
        public static string SearchUrl = ServerUrlRoot + "ar={0}&ab={1}";
        // Internally we use <SearchUrlMatcher> to extract artist/album from MXiTunes's query, as generated using the above string.
        private static string SearchUrlMatcher = "/ar=(?<Artist>.*)&ab=(?<Album>.*)";

        // MXiTunes use <PicRegex> to extract all possible triples of [album image, album, artist] from the response
        // it gets after submits an query
        public static string PicRegex = "image=(?<url>.+?)&artist=(?<Artist>.+?)&album=(?<Album>.+?)---\n";
        // Internally we use <PicRegexMatcher> to produce a list of possible album information, which is then fed 
        // to and finally get parsed by MXiTunes using the above regex.
        public static string PicRegexGenerator = "image={0}&artist={2}&album={1}---\n";

        // Interpreted by MXiTunes as prefix of album art image url, not necessary here.
        public static string PicServiceUrl = "";

        // Singleton
        public static AlbumArtServer instance = null;
        private AlbumArtServer() { }
        public static AlbumArtServer getInstance()
        {
            if (instance == null)
                instance = new AlbumArtServer();
            return instance;
        }
        private Thread thread = null;
        HttpListener listener = null;
        public void Start()
        {
            listener = new HttpListener();
            listener.Prefixes.Add(ServerUrlRoot);
            listener.Start();

            thread = new Thread(ListenerThread);
            thread.Start();
            Form1.LogInfo("Server started at " + ServerUrlRoot);
        }

        public void Stop()
        {
            if (listener != null)
                listener.Stop();
            if (thread != null)
                thread.Interrupt();
        }
        private void ListenerThread()
        {
            try
            {
                while (true)
                {
                    HttpListenerContext context = listener.GetContext();
                    Thread worker = new Thread(Process);
                    worker.Start(context);
                }
            }
            catch (HttpListenerException)  // Time to exit.
            {}
        }
        private static void Process(Object param)
        {
            HttpListenerContext context = (HttpListenerContext)param;
            Encoding encoder = new UTF8Encoding();

            string url = context.Request.Url.AbsolutePath;

            Match url_match = Regex.Match(url, SearchUrlMatcher);
            if (!url_match.Success)
            {
                Form1.LogInfo("Unknown URL: " + url);
                return;
            }
            String AlbumName = HttpUtility.UrlDecode(url_match.Groups["Album"].ToString());
            String ArtistName = HttpUtility.UrlDecode(url_match.Groups["Artist"].ToString());

            AlbumArtRetriever retriever = AlbumArtRetrieverManager.getSelectedRetriever();

            int album_count = 0;
            String result_line = "";
            foreach (AlbumInfo album in retriever.retrieve(ArtistName, AlbumName))
            {
                album_count++;
                result_line += String.Format(PicRegexGenerator, album.AlbumArtURL, album.AlbumName, album.Artist);
            }
            Match m = Regex.Match(result_line, PicRegex);
            String s = m.Groups["Artist"].Value;
            byte[] result_bytes = encoder.GetBytes(result_line);
            context.Response.OutputStream.Write(result_bytes, 0, result_bytes.Length);
            Form1.LogInfo(String.Format("Search of {0} {1} via {2} returned {3} albums.", 
                new object[] {ArtistName, AlbumName, retriever.getName(), album_count}));

            context.Response.OutputStream.Close();
        }
    }
}
