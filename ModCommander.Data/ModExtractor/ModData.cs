using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ModCommander.Data
{
    public class ModData
    {
        public string Author { get; set; }
        public string Version { get; set; }

        public bool IsMultiplayerReady { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string Hash { get; set; }
        public long FileSize { get; set; }
        public string FullPath { get; set; }

        public string FileName 
        {
            get { return Path.GetFileName(FullPath); }
        }

        public Bitmap Image { get; set; }
        public string ImagePath { get; set; }

        public List<StoreItemData> StoreItems { get; set; }

        public ModData()
        {
            StoreItems = new List<StoreItemData>();
        }
    }
}
