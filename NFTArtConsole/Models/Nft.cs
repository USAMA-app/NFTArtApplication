using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFTArtConsole.Models
{
    public class Nft
    {
        public string LayerFolderPath { get; set; }
        public string OutputFolderPath { get; set; }

        public int MetaDataType { get; set; } 
        public string MetaDataDescription { get; set; }
        public string MetadataImageBaseUri { get; set; }
        public string MetadataExternalUrl { get; set; }
        public bool MetadataUseFileExtension { get; set; }


        public string collectionSize { get; set; }
        public string InitialNumber { get; set; }
        public string ImagePrefix { get; set; }

        public string SelectionStart { get; set; }
        public int SelectionLength { get; set; }
        public string SelectionColor { get; set; }
        public string Text { get; set; }



        public Nft()
        {

        }
    }

    public enum MetaType
    {
        None = 1,
        Merged = 2,
        Individual = 3,
        Both = 4 
    }
}
