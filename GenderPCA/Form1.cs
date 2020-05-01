using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Diagnostics;
using Emgu.CV.Util;
using System.IO;

namespace GenderPCA
{

    public partial class Form1 : Form
    {
        //Image image;
        public Form1()
        {
            InitializeComponent();
        }

        public void LoadData( String gambar)
        {
            string imgFileName = gambar;

            Image<Bgr, byte> imgInit = new Image<Bgr, byte>(imgFileName);
            Image<Bgr, byte> imgDraw = imgInit.Clone();

            // Deklarasi Class ClassDetecion
            ClassDetection CD = new ClassDetection(imgInit);


            /* Memberikan value pada variable global FileCascade
             * Ketepatan File Cascade(HAARCASCADE)
             * index 0 = face
             * index 1 = eyes
             * index 2 = nose
             * index 3 = mouth
            */

            CD.FileCascade = new String[4] { "haarcascade_frontalface_default.xml", "haarcascade_eye.xml", "haarcascade_mcs_nose.xml", "haarcascade_mcs_mouth.xml" };

            /* Return value dari pemanggilan function dari ClassDetection
             * DetectFace()     = deteksi wajah 
             * DetectFeature(1) = deteksi mata
             * DetectFeature(2) = deteksi  Hidung
             * DetectFeature(3) = deteksi Mulut 
            */

            List<Rectangle> roiFace = CD.DetectFace();
            List<Rectangle> roiEye = CD.DetectFeature(1);
            List<Rectangle> roiNose = CD.DetectFeature(2);
            List<Rectangle> roiMouth = CD.DetectFeature(3);


            /* Pemecahan ROI untuk draw rectangle pada image */

            foreach (Rectangle fc in roiFace)
            {
                imgDraw.Draw(fc, new Bgr(Color.Magenta), 2);

            }

            foreach (Rectangle fe in roiEye)
            {
                //imgDraw.Draw(fe, new Bgr(Color.Green), 2);
                Rectangle mata1 = roiEye.ElementAt<Rectangle>(0);
                var crop1 = this.cropImage(imgDraw, mata1);
                imageBox1.Image = crop1;
                crop1.Save("D:\\tes\\" + imgFileName + "_mata1.jpg");

                Rectangle mata2 = roiEye.ElementAt<Rectangle>(1);
                var crop2 = this.cropImage(imgDraw, mata2);
                imageBox2.Image = crop2;
                crop2.Save("D:\\tes\\" + imgFileName + "_mata2.jpg");

                Matrix<byte> mtrx = new Matrix<byte>(crop2.Width, crop2.Height);

                for (int i = 0; i < mtrx.Height; i++)
                {
                    for (int j = 0; j < mtrx.Width; j++)
                    {
                        float pixel = mtrx.Data[i, j] = crop2.Data[i, j, 1];
                        Console.Write(pixel + "|");
                    }
                    Console.WriteLine("");

                }

            }

            foreach (Rectangle fn in roiNose)
            {
                //imgDraw.Draw(fn, new Bgr(Color.Brown), 2);
                var crophidung = this.cropImage(imgDraw, fn);
                imageBox4.Image = crophidung;
                crophidung.Save("D:\\tes\\" + imgFileName + "_hidung.jpg");
            }

            foreach (Rectangle fm in roiMouth)
            {
                //imgDraw.Draw(fm, new Bgr(Color.Yellow), 2);
                var cropmulut = this.cropImage(imgDraw, fm);
                imageBox3.Image = cropmulut;
                cropmulut.Save("D:\\tes\\" + imgFileName + "_mulut.jpg");
            }


            //foreach (Rectangle fe in roiEye)
            //{
            //imgDraw.Draw(fe, new Bgr(Color.Green), 2);

            //}

            imageBox5.Image = imgDraw;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            String gambarawal = "FE016.JPG";
            LoadData(gambarawal);
        }

        private Image<Bgr, byte> cropImage(Image<Bgr, byte> input, Rectangle rect)
        {
            var croppedImage = input.Clone();

            croppedImage.ROI = rect;

            return croppedImage;
        }

        private void preProcessingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {


            //Image<Gray, Byte> imgeOrigenal = new Image<Gray, Byte>(imageBox5.Image.Bitmap);
      
            //imageBox5.Image = imgeOrigenal;

            //Image<Gray, Byte> imgeOrigenal1 = new Image<Gray, Byte>(imageBox5.Image.Bitmap);

            //imageBox5.Image = imgeOrigenal1;

            Image<Gray, Byte> imgeOrigenal2 = new Image<Gray, Byte>(imageBox1.Image.Bitmap);

            imageBox1.Image = imgeOrigenal2;

            Image<Gray, Byte> imgeOrigenal3 = new Image<Gray, Byte>(imageBox2.Image.Bitmap);

            imageBox2.Image = imgeOrigenal3;
            
            Image<Gray, Byte> imgeOrigenal5 = new Image<Gray, Byte>(imageBox3.Image.Bitmap);

            imageBox3.Image = imgeOrigenal5;
            
            Image<Gray, Byte> imgeOrigenal4 = new Image<Gray, Byte>(imageBox4.Image.Bitmap);

            imageBox4.Image = imgeOrigenal4;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var FD = new System.Windows.Forms.OpenFileDialog();
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileToOpen = FD.FileName;

                System.IO.FileInfo File = new System.IO.FileInfo(FD.FileName);

                //OR

                System.IO.StreamReader reader = new System.IO.StreamReader(fileToOpen);
                Uri uri = new Uri(FD.FileName);
                if (uri.IsFile)
                {
                    string filename = System.IO.Path.GetFileName(uri.LocalPath);
                    LoadData(filename);
                }
            }
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox11_Enter(object sender, EventArgs e)
        {

        }





    }
}