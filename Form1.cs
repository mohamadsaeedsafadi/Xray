using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using NAudio.FileFormats;
using System.Media;
using NAudio.Wave;
using System.IO;
using NAudio.Gui;
using NAudio.Wave.SampleProviders;
using NAudio.CoreAudioApi;
using System.Diagnostics;
using MathNet;
using MathNet.Numerics.IntegralTransforms;
using System.Numerics;
using AForge.Imaging;
using AForge.Imaging.Filters;
using Accord;
using Accord.Math;
using Image = AForge.Imaging.Image;
using AForge.Imaging.ComplexFilters;

using System.Drawing.Drawing2D;
using Accord.IO;

using System.Drawing.Text;
using System.Runtime.InteropServices;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Telegram.Bot;







namespace Csharp
{
    public partial class Form1 : Form
    {


        int xDown = 0;
        int yDown = 0;
        int xUp = 0;
        int yUp = 0;
        System.Drawing.Rectangle rectCropArea = new System.Drawing.Rectangle();
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        Task timeout;
        string fn = "";
        static string token = "";
        static Telegram.Bot.TelegramBotClient bot = new TelegramBotClient(token);
        public Form1()
        {

            InitializeComponent();



        }

        Graphics g;
        Graphics image;
        Bitmap btm;
        Color c;
        bool drawing = false;

        private int count2;
        private bool flag = true;
        private int count1;
        private int rounded;
        private object numericUpDown2;
        private object radioButton1;
        private float _snAngle;
        private object numericUpDown1;
        private object canvas;
        private object rotateCursor;
        private Graphics s;
        private object bmp;
        Label[] L = new Label[100];
        Panel[] P = new Panel[100];
        int c2 = 0;
        Bitmap b1;
        bool add = false;
        private System.Drawing.Rectangle selectedRegion;
        int wi;

        public int GridGap { get; private set; }
        public object CreateCursor { get; private set; }

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        string record = "";
        private bool drawShape;


        private void Form1_Load(object sender, EventArgs e)
        {
            g = panel1.CreateGraphics();
            btm = new Bitmap(panel1.Width, panel1.Height);
            image = Graphics.FromImage(btm);
            image.Clear(Color.White);



        }



        private Bitmap ColorSelectedRegion(Bitmap image, System.Drawing.Rectangle selectedRegion)
        {
            Bitmap coloredImage = (Bitmap)image.Clone();
            int totalRed = 0, totalGreen = 0, totalBlue = 0;
            int pixelCount = 0;


            for (int y = selectedRegion.Y; y < selectedRegion.Y + selectedRegion.Height; y++)
            {
                for (int x = selectedRegion.X; x < selectedRegion.X + selectedRegion.Width; x++)
                {


                    Color pixelColor = image.GetPixel(x, y);
                    totalRed = pixelColor.R;
                    totalGreen = pixelColor.G;
                    totalBlue = pixelColor.B;
                    pixelCount++;
                    int avgRed = totalRed / pixelCount;
                    int avgGreen = totalGreen / pixelCount;
                    int avgBlue = totalBlue / pixelCount;


                    if (avgGreen == 0 && avgRed == 0 && avgBlue == 0)
                    {
                        coloredImage.SetPixel(x, y, Color.Red);


                    }

                    else if (avgBlue == avgGreen && avgRed == avgBlue && avgBlue != 0)
                    {
                        coloredImage.SetPixel(x, y, Color.Yellow);
                    }
                    else
                        coloredImage.SetPixel(x, y, Color.Blue);


                }

            }

            return coloredImage;

        }
        private Bitmap DrawShapeOnImage(Bitmap image, System.Drawing.Rectangle shapeRegion, Color color)
        {

            Bitmap modifiedImage = (Bitmap)image.Clone();


            using (Graphics graphics = Graphics.FromImage(modifiedImage))
            {

                using (Pen pen = new Pen(color))
                {

                    graphics.DrawRectangle(pen, shapeRegion);

                }
            }

            return modifiedImage;
        }

        private void panel1_Mouseup(object sender, MouseEventArgs e)
        {
            selectedRegion = System.Drawing.Rectangle.Empty;


        }
        private void panel1_Mousedown(object sender, MouseEventArgs e)
        {



            if (e.Button == MouseButtons.Left)
            {
                int x = e.X;
                int y = e.Y;
                selectedRegion = new System.Drawing.Rectangle(x, y, 0, 0);
                drawShape = true;


            }

        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.ShowDialog();
            c = cd.Color;
        }
        private void panel1_Mousemove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && drawShape)
            {
                int x = selectedRegion.X;
                int y = selectedRegion.Y;
                int width = e.X - selectedRegion.X;
                int height = e.Y - selectedRegion.Y;
                selectedRegion = new System.Drawing.Rectangle(x, y, width, height);
                Bitmap coloredImage = ColorSelectedRegion(btm, selectedRegion);
                Bitmap modifiedImage = DrawShapeOnImage(btm, selectedRegion, Color.Red);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = modifiedImage;
                pictureBox5.Image = coloredImage;
                if (selectedRegion.Width >= 101 &&selectedRegion.Height>=100)
                {

                    wi = 1;
                }

                if (selectedRegion.Width < 100 && selectedRegion.Width > 65 &&selectedRegion.Height <100 &&selectedRegion.Height>50)
                {

                    wi = 2;
                }
                if (selectedRegion.Width <= 60 && selectedRegion.Width >= 5 &&selectedRegion.Height>=5 &&selectedRegion.Height<51 )
                {

                    wi = 3;
                }
            }





        }
        private void button2_Click(object sender, EventArgs e)
        {

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            pictureBox5.Image.Save(sfd.FileName);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            Bitmap b = new Bitmap(ofd.FileName);
            image.DrawImage(b, new System.Drawing.Rectangle(0, 0, panel1.Width, panel1.Height));
            g.DrawImage(btm, PointF.Empty);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            Bitmap b = new Bitmap(ofd.FileName);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox1.Image = b;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

            OpenFileDialog ofd2 = new OpenFileDialog();
            ofd2.ShowDialog();
            Bitmap b = new Bitmap(ofd2.FileName);
            pictureBox2.Image = b;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;

            OpenFileDialog ofd3 = new OpenFileDialog();
            ofd3.ShowDialog();
            Bitmap b = new Bitmap(ofd3.FileName);
            pictureBox3.Image = b;
        }

        private void button5_Click(object sender, EventArgs e)
        {


            Bitmap img1 = new Bitmap(pictureBox2.Image);
            Bitmap img2 = new Bitmap(pictureBox3.Image);

            if (img1.Width == img2.Width && img1.Height == img2.Height)
            {
                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        string v = img1.GetPixel(i, j).ToString();

                        string v1 = img2.GetPixel(i, j).ToString();

                        if (v != v1)
                        {
                            count2++;
                            flag = false;
                            continue;
                        }
                        count1++;
                    }

                }
                if (flag == false)
                    MessageBox.Show("التقدم بالعلاج أو التطور بالمرض , " + count2 + " wrong pixels found");
                else
                    MessageBox.Show(" لا يوجد تقدم بالعلاج أوتطور بالمرض, " + count1 + " same pixels found and " + count2 + " wrong pixels found");
            }
            else
                MessageBox.Show("التقدم بالعالج أو التطور بالمرض");
            this.Dispose();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd4 = new OpenFileDialog();
            ofd4.ShowDialog();
            Bitmap b = new Bitmap(ofd4.FileName);



            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            Bitmap grayImage = filter.Apply(b);



            ComplexImage complexImage = ComplexImage.FromBitmap(grayImage);
            complexImage.ForwardFourierTransform();

            int cutoffFrequency = 30;
            FrequencyFilter FrequencyFilter = new FrequencyFilter(new AForge.IntRange(0, cutoffFrequency));
            FrequencyFilter.Apply(complexImage);


            complexImage.BackwardFourierTransform();

            Bitmap filteredImage = complexImage.ToBitmap();


            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.Image = filteredImage;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            filteredImage.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);

        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            if (opf.ShowDialog() == DialogResult.OK)
            {
                pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;

                pictureBox5.Image = Image.FromFile(opf.FileName);
                fn = opf.FileName;
            }
            pictureBox5.Cursor = Cursors.Cross;
        }

        private void pictureBox5_MouseUp(object sender, MouseEventArgs e)
        {

            xUp = e.X;
            yUp = e.Y;
            System.Drawing.Rectangle rec = new System.Drawing.Rectangle(xDown, yDown, Math.Abs(xUp - xDown), Math.Abs(yUp - yDown));
            using (Pen pen = new Pen(Color.YellowGreen, 3))
            {

                pictureBox5.CreateGraphics().DrawRectangle(pen, rec);
            }
            rectCropArea = rec;
            button10.Enabled = true;
        }
        private void pictureBox5_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox5.Invalidate();

            xDown = e.X;
            yDown = e.Y;
            button10.Enabled = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {

                pictureBox6.Refresh();
                pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;

                Bitmap sourceBitmap = new Bitmap(pictureBox5.Image, pictureBox5.Width, pictureBox5.Height);
                Graphics g = pictureBox6.CreateGraphics();

                g.DrawImage(sourceBitmap, new System.Drawing.Rectangle(0, 0, pictureBox6.Width, pictureBox6.Height), rectCropArea, GraphicsUnit.Pixel);
                sourceBitmap.Dispose();
                button10.Enabled = false;
                var path = Environment.CurrentDirectory.ToString();
                ms = new System.IO.MemoryStream();


                byte[] ar = new byte[ms.Length];
                var timeout = ms.WriteAsync(ar, 0, ar.Length);

            }
            catch (Exception ex)
            {

            }

        }

        private void btn_Record_Click(object sender, EventArgs e)
        {

            mciSendString("open new Type waveaudio Alias recsound", null, 0, 0);
            mciSendString("set recsound time format ms bitspersample 16 channels 2 samplespersec 48000 bytespersec 192000 alignment 4", null, 0, 0);
            mciSendString("record recsound", null, 0, 0);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {


            var filePath = "C:\\test\\result.wav";
            mciSendString("save recsound " + filePath, null, 0, 0);
            mciSendString("close recsound ", null, 0, 0);
        }

        private void save_report_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\test\\" +
            firstName.Text + " " + LastName.Text + ".txt");
            sw.WriteLine(label1.Text + " " + firstName.Text);
            sw.WriteLine(label2.Text + " " + LastName.Text);
            sw.WriteLine(label4.Text + " " + gender.Text);
            sw.WriteLine(label3.Text + " " + address.Text);
            sw.Close();

        }

        private void savepdf_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.ShowDialog();
            string pdf = (opf.FileName).Replace(".txt", ".pdf");
            StreamReader rdr = new StreamReader(Application.StartupPath + "\\test\\" +
            firstName.Text + " " + LastName.Text + ".txt");
            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(pdf, FileMode.Create));
            doc.Open();
            doc.Add(new Paragraph(rdr.ReadToEnd()));
            doc.Close();
            MessageBox.Show("done");
            System.Diagnostics.Process.Start(pdf);

        }

        private void button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();



            if (opf.ShowDialog() == DialogResult.OK)
            {
                var bytes = System.IO.File.ReadAllBytes(opf.FileName);
                int L1 = bytes.Length;
                bytes = Csharp.GZipCompressor.CompressBytes(bytes);
                int L2 = bytes.Length;
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "GZip File|*.gzip";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllBytes(sfd.FileName, bytes);
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "GZip File|*.gzip";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                var bytes = System.IO.File.ReadAllBytes(opf.FileName);
                int L1 = bytes.Length;
                bytes = Csharp.GZipCompressor.DecompressBytes(bytes);
                int L2 = bytes.Length;

                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllBytes(sfd.FileName, bytes);
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int offset = 0;
            while (true)
            {
                Telegram.Bot.Types.Update[] updates = bot.GetUpdatesAsync(offset).Result;
                try
                {
                    foreach (var update in updates)
                    {
                        offset = update.Id + 1;
                        if (update.Message != null)
                        {
                            long id = update.Message.From.Id;
                            string txt = update.Message.Text;
                            string username = update.Message.From.Username;
                            string firstname = update.Message.From.FirstName;
                            string lastname = update.Message.From.LastName;
                            if (txt == "/start")
                            {
                                bot.SendTextMessageAsync(id, "hello");

                            }
                            else if (txt == "/image")
                            {
                                OpenFileDialog ofd4 = new OpenFileDialog();
                                if (ofd4.ShowDialog() == DialogResult.OK)
                                {
                                    try
                                    {
                                        using (FileStream fileStream = new FileStream(ofd4.FileName, FileMode.Open, FileAccess.Read))
                                        {
                                            bot.SendPhotoAsync(id, new Telegram.Bot.Types.InputFileStream(fileStream));

                                            bot.SendTextMessageAsync(id, "done");

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error updating chat photo: " + ex.Message);
                                    }
                                }


                            }
                        }
                    }
                }
                catch
                {

                }

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (wi == 1) { MessageBox.Show("حالة مرضية شديدة "); }
            if (wi == 2) { MessageBox.Show("حالة مرضية متوسطة "); }
            if (wi == 3) { MessageBox.Show("حالة مرضية خفيفة "); }

        }

        private void button14_Click(object sender, EventArgs e)
        {
            DateTime desiredDateTime = dateTimePicker1.Value;

            string directoryPath = "D:\\تعليم\\كلية\\سنة رابعة\\فصل ثاني\\ملتي\\عملي";

            string[] imageFiles = Directory.GetFiles(directoryPath);

            foreach (string imageFile in imageFiles)
            {
                DateTime imageDateTime = File.GetLastWriteTime(imageFile);

                if (imageDateTime.Date == desiredDateTime.Date)
                {

                    Bitmap image = Image.FromFile(imageFile);
                    pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = image;
                  

                    break; 
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            long desiredSize = Convert.ToInt64(numericUpDown3.Value);
            string directoryPath = "D:\\تعليم\\كلية\\سنة رابعة\\فصل ثاني\\ملتي\\عملي";

            string[] imageFiles = Directory.GetFiles(directoryPath);

            bool foundImage = false;

            foreach (string imageFile in imageFiles)
            {
                FileInfo fileInfo = new FileInfo(imageFile);
                long imageSize = fileInfo.Length;
              
                if (imageSize == desiredSize)
                {
                    Bitmap image = Image.FromFile(imageFile);
                    pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = image;

                    foundImage = true;
                    break; 
                }
            }

            if (!foundImage)
            {
                MessageBox.Show("لم يتم العثور على صورة بالحجم المطلوب.");
            }
        }
    }
}   
    
    


