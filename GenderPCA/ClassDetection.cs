using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.UI;


namespace GenderPCA
{
    public class ClassDetection
    {
        private Image<Bgr, byte> _imgInit;
        private UMat _imgGray;
        private Rectangle[] _facesDetected;

        public String[] FileCascade;

        public ClassDetection(Image<Bgr, byte> imgInit)
        {
            // TODO: Complete member initialization
            this._imgInit = imgInit;
        }

        public List<Rectangle> DetectFace()
        {
            List<Rectangle> roiFace = new List<Rectangle>();
            try
            {
                CascadeClassifier FileFace = new CascadeClassifier(FileCascade[0]);
                UMat imgGray = new UMat();
                
                using (FileFace) {
                    using (this._imgGray)
                    {
                        // Convert to gray color
                        CvInvoke.CvtColor(this._imgInit, imgGray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                        CvInvoke.EqualizeHist(imgGray, imgGray);
                        this._imgGray = imgGray;

                        // ROI Of Faces
                        this._facesDetected = FileFace.DetectMultiScale(this._imgGray, 1.1, 10, new Size(20, 20));
                        roiFace.AddRange(this._facesDetected);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error : "+e.Message);
            }

            return roiFace;
        }

        public List<Rectangle> DetectFeature(int FileCascadeIndex)
        {

            List<Rectangle> featureFace = new List<Rectangle>();
            try
            {
                List<Rectangle> faceImg = this.DetectFace();
                CascadeClassifier FileFeature = new CascadeClassifier(this.FileCascade[FileCascadeIndex]);

                foreach(Rectangle fc in faceImg){
                    UMat imgRegion = new UMat(this._imgGray, fc);

                    using(imgRegion){
                        Rectangle[] featureDetected = FileFeature.DetectMultiScale(imgRegion,1.1,10,new Size(20, 20));

                        foreach (Rectangle ft in featureDetected)
                        {
                            Rectangle ftRect = ft;
                            ftRect.Offset(fc.X, fc.Y);
                            featureFace.Add(ftRect);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Error : " + e.Message);
            }

            return featureFace;
        }
    }
}
