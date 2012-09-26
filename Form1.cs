using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;  
using System.Runtime.Remoting.Channels;  
using System.Runtime.Remoting.Channels.Tcp;

namespace MXiTunesRemote
{
    public partial class Form1 : Form
    {
        private static Form1 instance = null;
        public Form1()
        {
            InitializeComponent();
            instance = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Start the proxy http server
            AlbumArtServer.getInstance().Start();

            // Register retrievers and fill UI.
            AlbumArtRetrieverManager.registerRetriever(new DefaultAlbumArtRetriever());
            foreach (string r in AlbumArtRetrieverManager.getRetrieverList())
                chkRetriever.Items.Add(r);

            chkRetriever.SetItemChecked(0, true);
            chkRetriever_SelectedIndexChanged(null, null);

            // Register Remoting object which MXiTunes will access when it starts up.
            TcpServerChannel servChannel = new TcpServerChannel(7898);
            ChannelServices.RegisterChannel(servChannel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                                    typeof(MXiTunesRemote), "MXiTunes",
                                    WellKnownObjectMode.SingleCall);
        }

        private void Form1_Click(object sender, EventArgs e)
        {
        }

        public static void LogInfo(String s)
        {
            MethodInvoker action = delegate { instance.txtLogMsg.Text += s + "\r\n"; }; 
            instance.txtLogMsg.Invoke(action);
        }

        private void chkRetriever_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkRetriever.CheckedItems.Count > 0)
                AlbumArtRetrieverManager.SelectRetriever(chkRetriever.CheckedItems[0].ToString());
            lblSelectedRetriever.Text = AlbumArtRetrieverManager.getSelectedRetriever().getName();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            AlbumArtServer.getInstance().Stop();
        }

        private void chkRetriever_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.chkRetriever.CheckedItems.Count > 0)
            {
                for (int i = 0; i < chkRetriever.Items.Count; i++)
                {
                    if (i != e.Index)
                    {
                        chkRetriever.SetItemCheckState(i, CheckState.Unchecked);
                    }
                }
            }
        }
    }

    [Serializable]
    class MXiTunesRemote : MarshalByRefObject, IMXiTunes
    {
        private List<PicModel> picmethod;
        public MXiTunesRemote()
        {
            Form1.LogInfo("MXiTunes connected.");
            PicModel picmodel = new PicModel();
            picmodel.PicRegex = AlbumArtServer.PicRegex;
            picmodel.PicServiceUrl = AlbumArtServer.PicServiceUrl;
            picmodel.SearchUrl = AlbumArtServer.SearchUrl;
            picmethod = new List<PicModel>();
            picmethod.Add(picmodel);
        }

        public List<PicModel> PicMethod
        {
            get { return picmethod; }
        }

        public int Version
        {
            get { return 1; }
        }

    }
}
