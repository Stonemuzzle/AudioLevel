using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio;
using NAudio.CoreAudioApi;

namespace AudioLevel
{
    public partial class Form1 : Form
    {
        private static readonly MMDeviceEnumerator enumer = new MMDeviceEnumerator();
        private readonly MMDevice dev = enumer.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        public Form1()
        {
            InitializeComponent();
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            dev.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
            CreateTextIcon(((int)(dev.AudioEndpointVolume.MasterVolumeLevel * -100f)).ToString());
        }

        void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            CreateTextIcon(((int)(data.MasterVolume * 100f)).ToString());
        }

        public void CreateTextIcon(string str)
        {
            Font fontToUse = new Font("Tahoma", 14, FontStyle.Regular, GraphicsUnit.Pixel);
            Brush brushToUse = new SolidBrush(Color.White);
            Bitmap bitmapText = new Bitmap(16, 16);
            Graphics g = System.Drawing.Graphics.FromImage(bitmapText);

            IntPtr hIcon;

            g.Clear(Color.Transparent);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            g.DrawString(str, fontToUse, brushToUse, -2, -2);
            hIcon = (bitmapText.GetHicon());
            notifyIcon1.Icon = System.Drawing.Icon.FromHandle(hIcon);
            notifyIcon1.Icon.Dispose();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            this.Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.Visible = false;
        }
    }
}
