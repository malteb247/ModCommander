using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModCommander.Data
{
    public class ModExtractorServiceStatusUpdateEventArgs
    {
        public int Count { get; set; }
        public int CurrentIndex { get; set; }
        public string FileName { get; set; }

        public ModExtractorServiceStatusUpdateEventArgs(int count, int currentIndex, string fileName)
        {
            this.Count = count;
            this.CurrentIndex = currentIndex;
            this.FileName = fileName;
        }
    }
}
