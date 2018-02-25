using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.IO;
using System.Xml;
using System.Data.OleDb;
using projectY;

namespace OMR
{
    /// <summary>
    /// Provides various image untilities, such as high quality resizing and the ability to save a JPEG.
    /// </summary>
    /// 
    public static class ImageUtilities
    {
        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        private static Dictionary<string, ImageCodecInfo> encoders = null;
        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        public static Dictionary<string, ImageCodecInfo> Encoders
        {
            //get accessor that creates the dictionary on demand
            get
            {
                //if the quick lookup isn't initialised, initialise it
                if (encoders == null)
                {
                    encoders = new Dictionary<string, ImageCodecInfo>();
                }
                //if there are no codecs, try loading them
                if (encoders.Count == 0)
                {
                    //get all the codecs
                    foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
                    {
                        //add each codec to the quick lookup
                        encoders.Add(codec.MimeType.ToLower(), codec);
                    }
                }
                //return the lookup
                return encoders;
            }
        }
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            //a holder for the result
            Bitmap result = new Bitmap(width, height);
            // set the resolutions the same to avoid cropping due to resolution differences
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }
            //return the resulting bitmap
            return result;
        }
        /// <summary> 
        /// Saves an image as a jpeg image, with the given quality 
        /// </summary> 
        /// <param name="path">Path to which the image would be saved.</param> 
        /// <param name="quality">An integer from 0 to 100, with 100 being the 
        /// highest quality</param> 
        /// <exception cref="ArgumentOutOfRangeException">
        /// An invalid value was entered for image quality.
        /// </exception>
        public static void SaveJpeg(string path, System.Drawing.Image image, int quality)
        {
            //ensure the quality is within the correct range
            if ((quality < 0) || (quality > 100))
            {
                //create the error message
                string error = string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality);
                //throw a helpful exception
                throw new ArgumentOutOfRangeException(error);
            }
            //create an encoder parameter for the image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            //get the jpeg codec
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            //create a collection of all parameters that we will pass to the encoder
            EncoderParameters encoderParams = new EncoderParameters(1);
            //set the quality parameter for the codec
            encoderParams.Param[0] = qualityParam;
            //save the image using the codec and the parameters
            image.Save(path, jpegCodec, encoderParams);
        }
        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            //do a case insensitive search for the mime type
            string lookupKey = mimeType.ToLower();
            //the codec to return, default to null
            ImageCodecInfo foundCodec = null;
            //if we have the encoder, get it to return
            if (Encoders.ContainsKey(lookupKey))
            {
                //pull the codec from the lookup
                foundCodec = Encoders[lookupKey];
            }
            return foundCodec;
        }
    }
    public class OpticalReader
    {
        public enum ExtractResults { FAILED = 0, OK = 1, NOBLOB = 2,INVALIDAR=3 };
        TimeSpan ts = new TimeSpan();
        private Rectangle tSheetSize;
        private string strDataRoot,strXMLSrc;
        public ExtractResults lExtractResult,lRateSliceResult;
        private double minbr, maxbr;
        private System.Drawing.Image iMarkLeft, iMarkRight;
        public Rectangle [] rBlocks;
        private Rectangle rRegBlock;
        public double sheetAR;
        public int defnumcols, defnumrows, defnumblocks,regnumrows,regnumcols;
        OleDbConnection dbcn;
        OleDbCommand dbcmd;
        OleDbDataReader dbdr;
        public List<System.Drawing.Point> answers;
        public int answerkeyid;
        public string dbpath;
        UnmanagedImage cb1,cb2;

        public OpticalReader(string sdr,string sxs,string dbp)
        {
            strDataRoot = sdr;
            strXMLSrc = sxs;
            dbpath = dbp;
            Bitmap b1, b2;
            b1 = new Bitmap(System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(sdr + "lc.jpg"));
            b2 = new Bitmap(System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(sdr + "rc.jpg"));
            iMarkLeft = b1;
            iMarkRight = b2;
            cb1 = UnmanagedImage.FromManagedImage((Bitmap)iMarkLeft);
            cb2 = UnmanagedImage.FromManagedImage((Bitmap)iMarkRight);
            minbr = Convert.ToDouble(GetL1Value(sxs, "MinBlobRatio"));
            maxbr = Convert.ToDouble(GetL1Value(sxs, "MaxBlobRatio"));
            answerkeyid = Convert.ToInt32(GetL1Value(strXMLSrc, "answerkey"));
            tSheetSize = GetBlockRectangle(strXMLSrc, "SheetSize");
            sheetAR = Math.Round((double)tSheetSize.Width / (double)tSheetSize.Height,1);
            defnumblocks=Convert.ToInt32(GetL1Value(strXMLSrc,"numofblocks"));
            defnumcols = Convert.ToInt32(GetL2Value(strXMLSrc,"tensblock1","columns"));
            defnumrows = Convert.ToInt32(GetL2Value(strXMLSrc, "tensblock1", "rows"));
            rRegBlock = GetBlockRectangle(strXMLSrc, "regnumblock");
            regnumrows = Convert.ToInt32(GetL2Value(strXMLSrc, "regnumblock", "rows"));
            regnumcols = Convert.ToInt32(GetL2Value(strXMLSrc, "regnumblock", "columns"));
            rBlocks = new Rectangle[defnumblocks];
            // Read block location from XML file for selected sheet
            for (int i = 1; i <= rBlocks.Length; i++)
            {
                rBlocks[i - 1] = GetBlockRectangle(strXMLSrc, "tensblock" + i.ToString());
            }
            getanswerlist();
        }

        private void getanswerlist()
        {
            dbcn = new OleDbConnection();
            dbcn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;data source=" + dbpath;
            dbcn.Open();
            dbcmd = dbcn.CreateCommand();
            dbcmd.Connection = dbcn;
            dbcmd.CommandText = "select * from answerkeys where answerkeyid = " + answerkeyid + " order by itemnumber";
            dbdr = dbcmd.ExecuteReader();
            answers = new List<System.Drawing.Point>();
            if (dbdr.FieldCount > 0)
                while (dbdr.Read())
                {
                    answers.Add(new System.Drawing.Point(dbdr.GetInt32(1), dbdr.GetInt32(2)));
                }
            else
            {
                MessageBox.Show("No answer keys found.");
            }
            dbdr.Close();
            dbcn.Close();
        }

        public void saveresults(List<List<int>> pa,int srnum)
        {
            if (srnum < 0) return;
            dbcn = new OleDbConnection();
            dbcn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;data source=" + dbpath;
            dbcn.Open();
            dbcmd = dbcn.CreateCommand();
            dbcmd.Connection = dbcn;
            dbcmd.CommandText = "DELETE FROM results where sheetregnumber = " + srnum;
            dbcmd.ExecuteNonQuery();
            int ctx = 1;
            for (int ni = 0; ni < defnumblocks; ni++)
            {
                foreach (int ni2 in pa[ni])
                {
                    dbcmd.CommandText = "insert into results values(" + srnum + "," + answerkeyid + "," + ctx + "," + ni2 + ")";
                    dbcmd.ExecuteNonQuery();
                    ctx++;
                }
            }
            dbcn.Close();
        }
        /// <summary>
        /// Extracts Images from smallSize CameraImage, No feed Back given during process
        /// </summary>
        /// <param name="SmallCameraImage"></param>
        /// <param name="OMRSpecsSheetAddress"></param>
        /// <returns>Formated, Right sized OMR Sheet</returns>
        private Bitmap flatten(Bitmap bmp, int fillint, int contint)
        {
            // step 1 - turn background to black
            ColorFiltering colorFilter = new ColorFiltering();
            colorFilter.Red = new IntRange(0, fillint);
            colorFilter.Green = new IntRange(0, fillint);
            colorFilter.Blue = new IntRange(0, fillint);
            colorFilter.FillOutsideRange = false;
            colorFilter.ApplyInPlace(bmp);
            AForge.Imaging.Filters.ContrastCorrection Contrast = new ContrastCorrection(contint);
            AForge.Imaging.Filters.Invert invert = new Invert();
            AForge.Imaging.Filters.ExtractChannel extract_channel = new ExtractChannel(0);
            AForge.Imaging.Filters.Threshold thresh_hold = new Threshold(44);
            bmp = invert.Apply(thresh_hold.Apply(extract_channel.Apply(Contrast.Apply(bmp))));
            return bmp;
        }
        private bool isSame(UnmanagedImage img1, UnmanagedImage img2)
        {
            int count = 0, tcount = img2.Width * img2.Height;
            int c1a, c2a;
            Color c1, c2;
            for (int y = 0; y < img1.Height; y++)
                for (int x = 0; x < img1.Width; x++)
                {
                    c1 = img1.GetPixel(x, y);
                    c2 = img2.GetPixel(x, y);
                    c1a = (c1.R + c1.G + c1.B) / 3;
                    c2a = (c2.R + c2.G + c2.B) / 3;
                   /* if ((c1.R + c1.G + c1.B) / 3 > (c2.R + c2.G + c2.B) / 3 - 10 &&
                        (c1.R + c1.G + c1.B) / 3 < (c2.R + c2.G + c2.B) / 3 + 10)*/
                    if (c1a > c2a-10 && c1a < c2a +10)
                        count++;
                }
            return (count * 100) / tcount >= 54;
        }
        private void showTimeStamp(string str, ref TextBox textBox1)
        {
            textBox1.AppendText(str + ": " + (new TimeSpan(DateTime.Now.Ticks) - ts).TotalSeconds + "\r\n");
            ts = new TimeSpan(DateTime.Now.Ticks);
        }
        public int getRegNumOfSheet(System.Drawing.Image image, bool readInvalidRegNum)
        {
            List<Bitmap[]> bmps = new List<Bitmap[]>();
            bmps.Add(SliceOMarkBlock(image, rRegBlock, regnumrows));
            
            int regNum = 0, multiplier = (int)Math.Pow(10,regnumrows-1);
            foreach (Bitmap[] blk in bmps)
            {
                foreach (Bitmap line in blk)
                {
                    int num = rateSlice(line, regnumcols)-1;
                    if (num < 0 && !readInvalidRegNum)
                    {
                        return -1;
                    }
                    else
                    {
                        if (num < 0) num = 0;
                        regNum += num * multiplier;
                        multiplier /= 10;
                    }
                }
            }
            return regNum;
        }
        /// <summary>
        /// Reads all the selected options on paper in one call
        /// </summary>
        /// <param name="image">Exctracted Sheet.</param>
        /// <param name="sheet">Type of printed sheet</param>
        /// <param name="OMRSheetFile">XML sheet address</param>
        /// <returns></returns>
        public List<List<int>> getScoreOfSheet(System.Drawing.Image image)
        {
            List<List<int>> scores = new List<List<int>>();
            //number of blocks depend upon type of sheet selected
            // slice the blocks into lines inside them and record as bitmap
            List<Bitmap[]> bmps = new List<Bitmap[]>();
            for (int i = 0; i < defnumblocks ; i++)
            {
                bmps.Add(SliceOMarkBlock(image, rBlocks[i], defnumrows));
                scores.Add(new List<int>());
            }
            int bn = 0;
            // read selected option of sliced line
            foreach (Bitmap[] blk in bmps)
            {
                foreach (Bitmap line in blk)
                {
                    scores[bn].Add(rateSlice(line,defnumcols));
                }
                bn++;
            }
            return scores;
        }
        /// <summary>
        /// returns ant int representation of selected option
        /// </summary>
        /// <param name="slice">Sliced sinlge line in choices block</param>
        /// <param name="OMCount"></param>
        /// <returns></returns>
        /// 

        public bool CheckSheetAR(List<IntPoint> p)
        {
            double r, w, h;
            w = Math.Sqrt(Math.Pow(p[1].X-p[0].X,2));
            h = Math.Sqrt(Math.Pow(p[2].Y - p[1].Y, 2));
            r = Math.Round(w / h,1);
            if (sheetAR.CompareTo(r) == 0) return true;
            return false;
        }

        public int lRateSlice(Bitmap slice, int OMCount)
        {
            try
            {
                lRateSliceResult = ExtractResults.OK;
                return rateSlice(slice, OMCount);
            }
            catch (Exception e)
            {
                lRateSliceResult = ExtractResults.FAILED;
                return -222;
            }
        }
        public int rateSlice(Bitmap slice, int OMCount)
        {
            Rectangle[] cropRects = new Rectangle[OMCount];
            Bitmap[] marks = new Bitmap[OMCount];
            //sub-devide line into option (horizontal only)
            for (int i = 0; i < OMCount; i++)
            {
                cropRects[i] = new Rectangle(i * slice.Width / OMCount, 0, slice.Width / OMCount, slice.Height);
            }
            int crsr = 0;
            foreach (Rectangle cropRect in cropRects)
            {
                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(slice, new Rectangle(0, 0, target.Width, target.Height),
                                     cropRect,
                                     GraphicsUnit.Pixel);
                }
                marks[crsr] = target;
                crsr++;
            }
            List<long> fullInks = new List<long>();
            //get marking level
            foreach (Bitmap mark in marks)
            {
                fullInks.Add(InkDarkness(mark));
            }
            int indofMx = -1;
            long maxD = 0;
            //get maximum ink level
            for (int i = 0; i < OMCount; i++)
            {
                if (fullInks[i] > maxD)
                {
                    maxD = fullInks[i];
                    indofMx = i;
                }
            }
            bool parallelExist = false, spe = false, tpe = false, fpe = false;
            for (int i = 0; i < OMCount; i++)
            {
                if (i != indofMx)
                {
                    if ((double)fullInks[indofMx] / fullInks[i] <= 2) //both ink levels are nearly the same
                    {
                        if (tpe) fpe = true;
                        if (spe) tpe = true;
                        if (parallelExist) spe = true;
                        parallelExist = true;
                    }
                }
            }
            int negScore = parallelExist ? -1 : 0;
            negScore = spe ? -2 : negScore;
            negScore = tpe ? -3 : negScore;
            negScore = fpe ? -4 : negScore;
            if (!parallelExist)
                return indofMx + 1;
            //check if multiple options were selected
            bool atleastOneUnfilled = false;
            for (int i = 0; i < OMCount; i++)
            {
                if (i != indofMx)
                {
                    if ((double)fullInks[indofMx] / fullInks[i] >= 3)
                        atleastOneUnfilled = true;
                }
            }
            if (atleastOneUnfilled)
                return negScore;
            return 0;
        }
        private long InkDarkness(Bitmap OMark)
        {
            int darkestC = 255, lightestC = 0;
            UnmanagedImage mark = UnmanagedImage.FromManagedImage(OMark);
            for (int y = 0; y < OMark.Height; y++)
                for (int x = 0; x < OMark.Width; x++)
                {
                    Color c = mark.GetPixel(x, y);
                    if (((c.R + c.G + c.B) / 3) > lightestC)
                    {
                        lightestC = ((c.R + c.G + c.B) / 3);
                    }
                    if (((c.R + c.G + c.B) / 3) < darkestC)
                    {
                        darkestC = ((c.R + c.G + c.B) / 3);
                    }
                }
            int dc = 0;
            for (int y = 0; y < OMark.Height; y++)
                for (int x = 0; x < OMark.Width; x++)
                {
                    Color c = mark.GetPixel(x, y);

                    if (((c.R + c.G + c.B) / 3) < (lightestC + darkestC) / 2)
                    { dc += 255; }
                }
            return dc;
        }
        public Bitmap[] SliceOMarkBlock(System.Drawing.Image fullSheet, Rectangle slicer, int slices)
        {
            List<Rectangle> cropRects = new List<Rectangle>();
            Bitmap[] bmps = new Bitmap[slices];
            for (int i = 0; i < slices; i++)
            {
                cropRects.Add(new Rectangle(slicer.X, slicer.Y + (slicer.Height / slices) * i, slicer.Width, slicer.Height / slices));
            }
            Bitmap src = (Bitmap)fullSheet;
            int crsr = 0;
            Rectangle tr = new Rectangle(0, 0, slicer.Width, slicer.Height/slices);
            foreach (Rectangle cropRect in cropRects)
            {
                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(src, tr,cropRect,GraphicsUnit.Pixel);
                }
                bmps[crsr] = target;
                crsr++;
            }
            return bmps;
            throw new Exception("Couldn't slice");
        }
        public Bitmap ExtractOMRSheet(Bitmap basicImage, int fillint, int contint)
        {
            System.Drawing.Image flattened = (System.Drawing.Image)flatten(basicImage, fillint, contint);
            return ExtractPaperFromFlattened(new Bitmap(flattened), basicImage, 3, fillint, contint);
        }
        public Bitmap ExtractPaperFromFlattened(Bitmap bitmap, Bitmap basicImage, int minBlobWidHei, int fillint, int contint)
        {
            // lock image, Bitmap itself takes much time to be processed
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, bitmap.PixelFormat);
            // step 2 - locating objects
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = minBlobWidHei;  // both these variables have to be given when calling the
            blobCounter.MinWidth = minBlobWidHei;   // method, the can also be queried from the XML reader using OMREnums
            blobCounter.ProcessImage(bitmapData);
            Blob[] blobs = blobCounter.GetObjectsInformation();
            bitmap.UnlockBits(bitmapData);
            Graphics g = Graphics.FromImage(bitmap);
//            Pen yellowPen = new Pen(Color.Yellow, 2);   // create pen in case image extraction failes and we need to preview the
            //blobs that were detected
            Rectangle[] rects = blobCounter.GetObjectsRectangles();
            Blob[] blobs2 = blobCounter.GetObjects(bitmap, false);
            //Detection of paper lies within the presence of crossmark printed on the corneres of printed sheet.
            // First, detect left edge.
            // lc.jpg = Mirrored image sample as located on the corner of printed sheet
            // this helps filtering out much smaller and much larger blobs depending upon the size of image.
            // can be queried from XML Reader
            List<IntPoint> quad = new List<IntPoint>(); // Store sheet corner locations (if anyone is detected )
            if (blobs2.GetLength(0) < 4 && contint == 0)
            {
                lExtractResult = ExtractResults.FAILED;
                return basicImage;
            }
            try
            {
                foreach (Blob blob in blobs2)
                {
                    if (
                        ((double)blob.Area) / ((double)bitmap.Width * bitmap.Height) > minbr &&
                        ((double)blob.Area) / ((double)bitmap.Width * bitmap.Height) < maxbr &&
                            blob.Rectangle.X < (bitmap.Width) / 4) // filters oout very small or very larg blobs
                    {
                        if ((double)blob.Rectangle.Width / blob.Rectangle.Height < 1.4 &&
                            (double)blob.Rectangle.Width / blob.Rectangle.Height > .6) // filters out blobs having insanely wrong aspect ratio
                        {
                            cb1= UnmanagedImage.FromManagedImage(ImageUtilities.ResizeImage(iMarkLeft, blob.Rectangle.Width, blob.Rectangle.Height));
                            if (isSame(blob.Image, cb1))
                            {
                                quad.Add(new IntPoint((int)blob.CenterOfGravity.X, (int)blob.CenterOfGravity.Y));
                            }
                        }
                    }
                }
            }
            catch (ArgumentException) { lExtractResult = ExtractResults.NOBLOB; }
            try
            { // Sort out the list in right sequence, UpperLeft,LowerLeft,LowerRight,upperRight
                if (quad[0].Y > quad[1].Y)
                {
                    IntPoint tp = quad[0];
                    quad[0] = quad[1];
                    quad[1] = tp;
                }
            }
            catch
            {
            }
            try
            {
                foreach (Blob blob in blobs2)
                {
                    if (
                        ((double)blob.Area) / ((double)bitmap.Width * bitmap.Height) > minbr &&
                        ((double)blob.Area) / ((double)bitmap.Width * bitmap.Height) < maxbr &&
                        blob.Rectangle.X > (bitmap.Width * 3) / 4)
                    {
                        if ((double)blob.Rectangle.Width / blob.Rectangle.Height < 1.4 &&
                        (double)blob.Rectangle.Width / blob.Rectangle.Height > .6)
                        {
                            cb2 = UnmanagedImage.FromManagedImage(ImageUtilities.ResizeImage(iMarkRight, blob.Rectangle.Width, blob.Rectangle.Height));
                            if (isSame(blob.Image, cb2))
                            {
                                quad.Add(new IntPoint((int)blob.CenterOfGravity.X, (int)blob.CenterOfGravity.Y));
                            }
                        }
                    }
                }
            }
            catch (ArgumentException) { lExtractResult = ExtractResults.NOBLOB; }
            try
            {
                if (quad[2].Y < quad[3].Y)
                {
                    IntPoint tp = quad[2];
                    quad[2] = quad[3];
                    quad[3] = tp;
                }
            }
            catch
            {
            }
            g.Dispose();
            //Again, filter out if wrong blobs pretended to our blobs.
            if (quad.Count == 4)
            {
                if (((double)quad[1].Y - (double)quad[0].Y) / ((double)quad[2].Y - (double)quad[3].Y) < .75 ||
                    ((double)quad[1].Y - (double)quad[0].Y) / ((double)quad[2].Y - (double)quad[3].Y) > 1.25)
                    quad.Clear(); // clear if, both edges have insanely wrong lengths
                else if (quad[0].X > bitmap.Width / 2 || quad[1].X > bitmap.Width / 2 || quad[2].X < bitmap.Width / 2 || quad[3].X < bitmap.Width / 2)
                    quad.Clear(); // clear if, sides appear to be "wrong sided"
            }
            if (quad.Count != 4)// sheet not detected, reccurrsive call.
            {
                if (contint <= 60)//try altering the contrast correction on both sides of numberline
                {
                    if (contint >= 0)
                    {
                        contint += 5;
                        contint *= -1;
                        return ExtractOMRSheet(basicImage, fillint, contint);
                    }
                    else
                    {
                        contint *= -1;
                        contint += 10;
                        return ExtractOMRSheet(basicImage, fillint, contint);
                    }
                }
                else // contrast correction yeilded no result
                {
                    lExtractResult = ExtractResults.FAILED;
                    return basicImage;
                }
            }
            else // sheet found
            {
                IntPoint tp2 = quad[3];
                quad[3] = quad[1];
                quad[1] = tp2;

                if (!CheckSheetAR(quad))
                {
                    lExtractResult = ExtractResults.INVALIDAR;
                    return basicImage;
                }

                //sort the edges for wrap operation
                QuadrilateralTransformation wrap = new QuadrilateralTransformation(quad);
                wrap.UseInterpolation = false; //perspective wrap only, no binary.
                wrap.AutomaticSizeCalculaton = false;
                wrap.NewWidth = tSheetSize.Width;
                wrap.NewHeight = tSheetSize.Height;
                lExtractResult = ExtractResults.OK;
                //wrap.Apply(basicImage);//.Save("LastImg.jpg", ImageFormat.Jpeg); // creat file backup for future use.
                return wrap.Apply(basicImage); // wrap
            }
        }
        public string GetL1Value(string xmldoc, string ename)
        {
            XmlDocument xd = new XmlDocument();
            string s = "";
            xd.Load(xmldoc);
            foreach (XmlNode xn in xd.ChildNodes[1].ChildNodes)
            {
                if (String.Compare(xn.Name.ToLower(), ename.ToLower()) == 0)
                    s = xn.InnerText;
            }
            return (s);
        }
        public string GetL2Value(string xmldoc, string l1s, string l2s)
        {
            XmlDocument xd = new XmlDocument();
            string s = "";
            xd.Load(xmldoc);
            foreach (XmlNode xn in xd.ChildNodes[1].ChildNodes)
            {
                if (String.Compare(xn.Name.ToLower(), l1s.ToLower()) == 0)
                {
                    foreach (XmlNode xn2 in xn.ChildNodes)
                    {
                        if (String.Compare(xn2.Name.ToLower(), l2s.ToLower()) == 0)
                            s = xn2.InnerText;
                    }
                }
            }
            return (s);
        }
        public Rectangle GetBlockRectangle(string f, string bn)
        {
            int x, y, w, h;
            x = Convert.ToInt32(GetL2Value(f, bn, "x"));
            y = Convert.ToInt32(GetL2Value(f, bn, "y"));
            w = Convert.ToInt32(GetL2Value(f, bn, "width"));
            h = Convert.ToInt32(GetL2Value(f, bn, "height"));
            return (new Rectangle(x, y, w, h));
            
        }

    }
    
}
