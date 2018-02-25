using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using OMR;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace projectY
{
    public partial class Form1 : Form
    {
        TimeSpan ts;
        Bitmap imgsrc;
        OpticalReader opr;
        string sxml,dbpath;
        string[] sFiles;
        List<string> sFileFails;
        string sMsg;
        Font f = new Font("Impact", 20);
        System.Drawing.Point pt = new System.Drawing.Point(0,0);
        int nFiles, nCount,nSuccess;
        bool running = false,
             userended=false,
             hassource=false,
             hassheet=false,
             hasdb=false;
        int srn,idc,crc,srctype;
        Pen pn = new Pen(Brushes.Red, 5);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            statusStrip1.Items[0].Text = "Status: IDLE";
            statusStrip1.Items[1].Text = "Source:";
            statusStrip1.Items[2].Text = "Sheet:";
            statusStrip1.Items[3].Text = "Database:";

            srctype = Program.GetRegValue("SOURCETYPE", 1);
            if (srctype == 1)
            {
                sFiles = new string[1];
                sFiles[0] = Program.GetRegValue("SOURCE", "");
                if (sFiles[0].Length > 0)
                {
                    statusStrip1.Items[1].Text = "Source: " + sFiles[0];
                    hassource = true;
                    nFiles = 1;
                }
            }
            else
            {
                if (Program.GetRegValue("SOURCE","").Length > 0)
                {
                    string sxt = Program.GetRegValue("SOURCE", "");
                    nFiles = System.IO.Directory.GetFiles(sxt, "*.jpg").Length;
                    if (nFiles > 0)
                    {
                        statusStrip1.Items[1].Text = "Source:" + sxt;
                        sFiles = System.IO.Directory.GetFiles(sxt,"*.jpg");
                        hassource = true;
                    }

                }
            }
            sxml = Program.GetRegValue("SHEET", "");
            if (sxml.Length > 0)
            {
                statusStrip1.Items[2].Text = "Sheet: " + sxml;
                hassheet = true;
            }
            dbpath = Program.GetRegValue("DBPATH", "");
            if (dbpath.Length > 0)
            {
                statusStrip1.Items[3].Text = "Database: " + dbpath;
                hasdb = true;
            }
        }


        //**************************************
        /*IMPORTED ALL*/

        private bool ProcessImage()
        {
            ts = new TimeSpan(DateTime.Now.Ticks);
            sMsg = "Extracting OMR image...";
            panel.Invalidate();
            Application.DoEvents();
            imgsrc = ImageUtilities.ResizeImage(imgsrc, 2100, 2100 * imgsrc.Height / imgsrc.Width);
            showTimeStamp("Resized",true);
            Bitmap unf = new Bitmap(imgsrc);
            Application.DoEvents();
            panel.Image =(System.Drawing.Image) opr.ExtractOMRSheet(unf, 0, 0);
            Graphics gx = Graphics.FromImage(panel.Image);
            if (opr.lExtractResult == OpticalReader.ExtractResults.FAILED)
            {
                sMsg = "Failed acquiring OMR source.";
                panel.Invalidate();
                showTimeStamp("Extraction Failed.", true);
                return false;
            }

            if (opr.lExtractResult == OpticalReader.ExtractResults.INVALIDAR)
            {
                sMsg = "Invalid OMR source.";
                panel.Invalidate();
                showTimeStamp("OMR source and sheet size do not match.", true);
                return false;
            }
            foreach (Rectangle ri in opr.rBlocks)
            {
                gx.DrawRectangle(pn, ri);
            }

            sMsg = "OMR extraction complete.";
            panel.Invalidate();
            showTimeStamp("Successful OMR Extraction.", true);
            return true;

        }
        private void showTimeStamp(string str,bool showtime)
        {
            if (showtime)
            {
                listResults.Items.Add(str + ": " + (new TimeSpan(DateTime.Now.Ticks) - ts).TotalSeconds+"s");
                ts = new TimeSpan(DateTime.Now.Ticks);
            }
            else
                listResults.Items.Add(str);
            listResults.SetSelected(listResults.Items.Count - 1, true);

        }
        private void fILEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPEG|*.jpg|BITMAP|*.bmp";
            openFileDialog1.FileName = "";
            hassource = false;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                sFiles = openFileDialog1.FileNames;
                nFiles = sFiles.Length;
                hassource= true;
                statusStrip1.Items[1].Text = "Source: " + sFiles[0] + " (File)";
                Program.SetRegValue("SOURCE", sFiles[0]);
                Program.SetRegValue("SOURCETYPE", 1);
            }
        }

        private void sHEETFORMATToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "XML|*.xml";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                sxml = openFileDialog1.FileName;
                statusStrip1.Items[2].Text = "Sheet: " + sxml;
                hassheet = true;
                Program.SetRegValue("SHEET", sxml);
            }
            else hassheet = false;
        }

        private void cLEARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listResults.Items.Count > 0) listResults.Items.Clear();
        }


        private void fOLDERToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Application.StartupPath;
            if (folderBrowserDialog1.ShowDialog()== System.Windows.Forms.DialogResult.OK)
            {
                sFiles = System.IO.Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.jpg");
                nFiles = sFiles.Length;
                if (nFiles > 0)
                {
                    hassource= true;
                    statusStrip1.Items[1].Text = "Source: " + folderBrowserDialog1.SelectedPath+ " FOLDER";
                    Program.SetRegValue("SOURCE", folderBrowserDialog1.SelectedPath);
                    Program.SetRegValue("SOURCETYPE", 2);
                }
                else
                {
                    hassource = false;
                    MessageBox.Show("No jpg images found.");
                }
            }
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(sMsg, f,Brushes.Red, pt);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!hassource)
            {
                if (MessageBox.Show("Click YES to open a file, NO to open a folder", "OPEN SOURCE", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    fILEToolStripMenuItem.PerformClick();
                else
                    fOLDERToolStripMenuItem2.PerformClick();
                return;
            }
            if (!hassheet)
            {
                sHEETFORMATToolStripMenuItem.PerformClick();
                return;
            }
            if (!hasdb)
            {
                dATABASEToolStripMenuItem.PerformClick();
                return;
            }
            if (!running)
            {
                statusStrip1.Items[0].Text = "Status: RUNNING...";
                running = !running;
                userended = false;
                cmdexec();
            }
            else
            {
                if (MessageBox.Show(this, "Terminate?", "END PROCESSING", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                {
                    showTimeStamp("Ending process...", false);
                    running = !running;
                    userended = true;
                }
            }
        }

        void cmdexec()
        {
            opr = new OpticalReader("projectY.Resources.", sxml,dbpath);
            sFileFails = new List<string>();
            List<List<int>> answers;
            nCount = 1;
            nSuccess = 0;
            button1.Text = "STOP";
            mnuClearLog.Enabled = false;
            foreach (string s in sFiles)
            {
                Application.DoEvents();
                if (!running)
                    break;
                imgsrc = new Bitmap(s);
                panel.Image = imgsrc;
                sMsg = "";
                showTimeStamp("Processing " + s + " (" + nCount + " of "  + nFiles + ")", false);
                crc = 0;
                if (ProcessImage())
                {
                        try
                        {
                            Application.DoEvents();
                            srn = opr.getRegNumOfSheet(panel.Image, false);
                            answers = opr.getScoreOfSheet(panel.Image);
                            nSuccess++;
                            if (srn > 0)
                                showTimeStamp("Sheet No. " + srn, false);
                            else                           
                                showTimeStamp("Invalid sheet number", false);
                            if (opr.answers.Count == 0)
                                showTimeStamp("No valid answer keys loaded.", false);
                            showTimeStamp("Answers:", false);
                            idc = 1;
                            for (int ni = 0; ni < opr.defnumblocks; ni++)
                            {
                                foreach (int ni2 in answers[ni])
                                {
                                    if (opr.answers.Count > 0)
                                    {
                                        if (ni2 == opr.answers[idc - 1].Y)
                                        {
                                            showTimeStamp(idc + ". " + ni2 + " : Correct", false);
                                            crc++;
                                        }
                                        else
                                        {
                                            showTimeStamp(idc + ". " + ni2 + " : WRONG", false);
                                        }
                                    }
                                    else
                                        showTimeStamp(idc + ". " + ni2, false);
                                    idc++;
                                }
                            }
                            if (srn >= 0 && opr.answers.Count > 0)
                            {
                                opr.saveresults(answers, srn);
                                showTimeStamp("Score: " + crc + "/" + --idc + " (SAVED)", false);
                            }
                            else
                                showTimeStamp("Score: " + crc + "/" + --idc + " (NOT SAVED. Invalid sheet number or no answer keys found.)", false);
                        }
                        catch (Exception ei)
                        {
                            sFileFails.Add(s);
                            showTimeStamp("Error processing scores." + ei.Message, false);
                        }
                }
                else
                {
                    sFileFails.Add(s);
                }
                nCount++;
            }
            if (!userended)
            {
                showTimeStamp("Done: " + nSuccess + "/" + nFiles + " success", false);
                foreach (string sss in sFileFails)
                {
                    showTimeStamp("Failed: " + sss, false);
                }
                MessageBox.Show("Done.");
            }
            else
            {
                showTimeStamp("Processing ended, user-signaled.", false);
                MessageBox.Show("User terminated processing.");
            }
            statusStrip1.Items[0].Text = "Status: DONE";
            button1.Text = "START";
            running = false;
            mnuClearLog.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (running)
                if (MessageBox.Show(this, "EXIT?", "EXIT PROGRAM", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                    e.Cancel = true;
        }

        private void cLEARLOGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listResults.Items.Clear();
        }

        private void dATABASEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "ACCESS DATABASE|*.accdb";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dbpath = openFileDialog1.FileName;
                statusStrip1.Items[3].Text = "Database: " + dbpath;
                Program.SetRegValue("DBPATH", dbpath);
                hasdb = true;
            }
            else hasdb = false;
        }
    }
}
