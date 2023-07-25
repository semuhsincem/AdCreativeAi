using System;
using System.Collections.Generic;
using System.Text;

namespace AdCr.BackEnd
{
    internal class JsonRequest
    {
        public int Count { get; set; }
        public int Parallelism { get; set; }
        public string SavePath { get; set; }
        public JsonRequest(int count, int parallelism, string savePath)
        {
            Count = count;
            Parallelism = parallelism;
            SavePath = savePath;
        }

    }
}
