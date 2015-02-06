namespace ModCommander.Data
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;

    public class StoreItemData
    {
        public double Price { get; set; }
        public double Upkeep { get; set; }

        public string Brand { get; set; }
        public string MachineType { get; set; }

        public string XmlFileName { get; set; }

        public Bitmap Image { get; set; }
        public string ImagePath { get; set; }

        public Bitmap BrandImage { get; set; }
        public string BrandImagePath { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Specs { get; set; }

        public StoreItemData()
        {
           
        }
    }


}
