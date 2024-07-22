using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileCorrupter
{
    class File_Headers
    {
        public struct headerinfo
        {
            public string extension;
            public int read_length;
            public int check_extn;
            public byte [] hexvalue;

        }

        private List<headerinfo> extensions_list = new List<headerinfo>();


        public List<headerinfo> populate_list()
        {
            //images
            extensions_list.Add(create(".jpg", new byte []{0xFF, 0xD8, 0xFF, 0xE0}, 4));
            extensions_list.Add(create(".bmp", new byte[] { 0x42, 0x4D }, 2));
            extensions_list.Add(create(".gif", new byte[] { 0x47, 0x49, 0x46 }, 3));
            extensions_list.Add(create(".png", new byte[] { 0x08, 0x95, 0x4E, 0x47 }, 4));
            
            //documents
            extensions_list.Add(create(".docx",new byte []{0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00}));
            extensions_list.Add(create(".pptx",new byte []{0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00}));
            extensions_list.Add(create(".xlsx",new byte []{0x50, 0x4B, 0x03, 0x04, 0x0A, 0x00, 0x06, 0x00}));
            extensions_list.Add(create(".doc", new byte []{0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1}));
            extensions_list.Add(create(".ppt", new byte []{0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1}));
            extensions_list.Add(create(".xls", new byte []{0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1}));
            extensions_list.Add(create(".rar", new byte []{0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00, 0xCE}));
            extensions_list.Add(create(".pdf", new byte[] {0x25, 0x50, 0x44, 0x46}, 4));
            
            //videos
            extensions_list.Add(create(".mov", new byte[] { 0x00, 0x01, 0x68, 0x14, 0x6D, 0x6F, 0x6F, 0x76}));
            extensions_list.Add(create(".wmv", new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11}));
            extensions_list.Add(create(".avi", new byte[] { 0x52, 0x49, 0x46, 0x46, 0xE8, 0x8C, 0xAA, 0x01, 0x41, 0x56, 0x49 },11));
            extensions_list.Add(create(".mp3", new byte[] { 0x49, 0x44, 0x33}, 3, 35000));
            extensions_list.Add(create(".mp4", new byte[] { 0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34}, 11));
            return extensions_list;
        }

        private headerinfo create(string a, byte [] arr, int c = 8, int d = 1500)
        {
            headerinfo obj = new headerinfo();
            obj.extension = a;
            obj.read_length = d;
            obj.check_extn = c;
            obj.hexvalue = arr;

            return obj;
        }

    }
}
