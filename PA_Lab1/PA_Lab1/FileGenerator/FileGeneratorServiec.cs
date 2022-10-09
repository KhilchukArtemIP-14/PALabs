using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
namespace PA_Lab1
{
    [Flags]
    public enum FileSize:long
    {
        OneMB= 1048576,
        TenMB = 10 * 1048576,
        HundredMB = 100 * 1048576,
        Gigabyte = 1024 * 1048576,
        TwoGig= 2147483648,
        FourGig= 4294967296,
        FifteenGig = 16106127360,
        SixteenGig= 17179869184
    }

    class FileGeneratorService
    {
        private string _path;

        public FileGeneratorService(string path)
        {
            _path = path;
        }

        public void GenerateFile(FileSize fileSize)
        {
            FileStream fs = new FileStream(_path, FileMode.Create);
            var sw = new StreamWriter(fs,Encoding.UTF32, 131072);
                Random rand = new Random();
                while(fs.Length<(long)fileSize)
                {
                     sw.WriteLine(rand.Next(1_000_000));
                }
            fs.Close();
        }
    }
}