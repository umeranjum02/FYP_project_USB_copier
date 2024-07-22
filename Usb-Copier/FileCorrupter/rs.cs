using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace E_Library
{
    internal class rs
    {

        public List<string> DirSearch(string sDir, ref List<string> path, string ext = "*.*")
        {
            try
            {
                if (path.Count == 0)
                {
                    foreach (string f in Directory.GetFiles(sDir, ext))
                    {
                        path.Add(f);

                    }

                }

                foreach (string d in Directory.GetDirectories(sDir))
                {

                    foreach (string f in Directory.GetFiles(d, ext))
                    {
                        path.Add(f);
                        
                    }
                    DirSearch(d, ref  path, ext);
                }
            }
            catch
            {
               
            }

            return path;


        }

    }
}
