using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace PA_Lab1
{
    class FileDataSequence : IDataContainer
    {
        private string _path;
        FileStream fs=null;
        private StreamWriter sw = null;
        private StreamReader sr = null;
        public FileDataSequence(string path)
        {
            _path = path;
            fs = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            sw = new StreamWriter(fs, Encoding.UTF32, 131072);
            sr = new StreamReader(fs, Encoding.UTF32);
        }

        public void Clear()
        {
            if (fs != null)
            {
                fs.Close();
            }
            fs = new FileStream(_path, FileMode.Create);
            sw = new StreamWriter(fs, Encoding.UTF32, 131072);
            sr = new StreamReader(fs, Encoding.UTF32);
        }

        public long GetSize()
        {if (fs != null)
            {
                return fs.Length;
            }
            return 0;
        }

        public void InputInt(int a)
        {
             sw.WriteLine(a);
             sw.Flush();
        }

        public int? SpitOutInt()
        {
            if (!sr.EndOfStream)
            {
                var temp = sr.ReadLine();
                return Convert.ToInt32(temp);
            }
            else return null;
        }

        public void ResetPointer()
        {
            if (fs != null)
            {
                fs.Close();
            }
            fs = new FileStream(_path, FileMode.Open, FileAccess.ReadWrite);
            sw = new StreamWriter(fs, Encoding.UTF32, 131072);
            sr = new StreamReader(fs, Encoding.UTF32);
        }

        public long GetIndex()
        {
            return fs.Position;
        }
        public void Close()
        {
            Clear();
            sw.Close();
            fs.Close();
            sr.Close();
        }
        }
    }
