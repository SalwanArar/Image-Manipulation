using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Image
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap b = null;
        Bitmap backup = null;
        Bitmap mer = null;
        List<double> Saturation = new List<double>();
        List<double> Hue = new List<double>();
        List<double> Lightness = new List<double>();
        
        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Choose image";
            dialog.Filter = "JPG iamge|*.jpg|JPEG image|*.jpeg|PNG Image|*.png";
            var res = dialog.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.Cancel)
                return;
            String path = dialog.FileName;
            b = new Bitmap(path);
            picBoxMain.Image = b;
            //picBoxMain.ImageLocation = path;

        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (b == null)
                return;
            String info = "Width: " + b.Width + Environment.NewLine
                + "Height: " + b.Height + Environment.NewLine
                + "ResX: " + b.HorizontalResolution + Environment.NewLine
                + "ResY: " + b.VerticalResolution + Environment.NewLine
                + "Pixel format: " + b.PixelFormat;
            MessageBox.Show(this, info, "Image information", MessageBoxButtons.OK
                , MessageBoxIcon.Information);
        }

        void backUpImage()
        {
            if (backup != null)
                backup.Dispose();
            backup = new Bitmap(b);
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backUpImage();
            if (b == null)
                return;
            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    Color c = b.GetPixel(i, j);
                    Color newC = Color.FromArgb(c.R, c.G, 0);
                    b.SetPixel(i, j, newC);
                }
            }
            picBoxMain.Image = b;
        }


        private void grayToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            backUpImage();
            if (b == null)
                return;
            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    Color c = b.GetPixel(i, j);
                    byte avg = (byte)((c.R + c.G + c.B) / 3);
                    Color newC = Color.FromArgb(avg, avg, avg);
                    b.SetPixel(i, j, newC);
                }
            }
            picBoxMain.Image = b;
        }

        private void setBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            var res = dialog.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.Cancel)
                return;
            picBoxMain.BackColor = dialog.Color;
        }

        private void addColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backUpImage();
            if (b == null)
                return;
            ColorDialog dialog = new ColorDialog();
            var res = dialog.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.Cancel)
                return;
            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    Color c = b.GetPixel(i, j);
                    byte newR = (byte)((c.R + dialog.Color.R) / 2);
                    byte newG = (byte)((c.G + dialog.Color.G) / 2);
                    byte newB = (byte)((c.B + dialog.Color.B) / 2);
                    Color newC = Color.FromArgb(newR, newG, newB);
                    b.SetPixel(i, j, newC);
                }
            }
            picBoxMain.Image = b;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (backup == null)
                return;
            b.Dispose();
            b = backup;
            backup = null;
            picBoxMain.Image = b;
        }
        void array2img(Bitmap output, byte[] data)
        {
            var lockdata = output.LockBits(
            new Rectangle(0, 0, output.Width, output.Height),
            ImageLockMode.WriteOnly, output.PixelFormat);
            Marshal.Copy(data, 0, lockdata.Scan0, data.Length);
            output.UnlockBits(lockdata);
        } 
        private void mergeImagesPixelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Choose image";
            dialog.Filter = "JPG iamge|*.jpg|JPEG image|*.jpeg|PNG Image|*.png";
            var res = dialog.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.Cancel)
                return;
            String path = dialog.FileName;
            mer = new Bitmap(path);
            pictureBox2.Image = mer;
        }



        private void pictureBox2_Click_1(object sender, EventArgs e)
        {

        }

        private void picBoxMain_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (b == null || mer == null)
                return;
            backup = new Bitmap(b);
            for (int i = 0; i < Math.Min(b.Width, mer.Width); i++)
            {
                for (int j = 0; j < Math.Min(b.Height, mer.Height); j++)
                {
                    Color c1 = b.GetPixel(i, j);
                    Color c2 = mer.GetPixel(i, j);
                    byte newR = (byte)((c1.R + c2.R) / 2);
                    byte newG = (byte)((c1.G + c2.G) / 2);
                    byte newB = (byte)((c1.B + c2.B) / 2);
                    Color newC = Color.FromArgb(newR, newG, newB);
                    backup.SetPixel(i, j, newC);
                }
            }
            pictureBox3.Image = backup;
            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (b == null)
                return;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JPG iamge|*.jpg|JPEG image|*.jpeg|PNG Image|*.png";
            if (dialog.ShowDialog() == DialogResult.Cancel) return;
            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegcodec = info.Single(i => i.MimeType == "image/jpeg");
            EncoderParameter para = new EncoderParameter(
                System.Drawing.Imaging.Encoder.Quality, 10L);
            EncoderParameters param = new EncoderParameters(1);
            param.Param[0] = para;
            b.Save(dialog.FileName, jpegcodec, param);
        }

        private void convertToHLSGrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color c = Color.White;
            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    c = b.GetPixel(i, j);
                    int R, G, B;
                    double H, S, L;
                    HLS.RgbToHls(c.R, c.G, c.B, out H, out L, out S);
                    Saturation.Add(S);
                    Hue.Add(H);
                    Lightness.Add(L);
                    S = 0.0;

                    HLS.HlsToRgb(H, L, S, out R, out G, out B);
                    Color newC = Color.FromArgb(R, G, B);

                    b.SetPixel(i, j, newC);
                }
            }

            picBoxMain.Image = b;
        }
        private void picBoxMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (b == null)
                MessageBox.Show("image not loaded");
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                int x = e.X;
                int y = e.Y;
                double hr = (double)b.Height / picBoxMain.Height;
                double wr = (double)b.Width / picBoxMain.Width;
                if (hr > wr)
                {
                    y = (int)(y * hr);
                    x = (int)((x - picBoxMain.Width / 2) * hr + b.Width / 2);
                }
                else
                {
                    x = (int)(x * hr);
                    y = (int)((y - picBoxMain.Width / 2) * hr + b.Width / 2);
                }
                if (x >= 0 && x < b.Width && y >= 0 && y < b.Height)
                {
                    Color c = b.GetPixel(x, y);
                    pictureBox4.BackColor = c;
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
    
        
    }
}
