using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQ.Consumer.Model
{
    public class Image
    {
        [Key]
        public int ID { get; set; }
        public string ImageCode { get; set; }
        public string ImageName { get; set; }
        public float rarity { get; set; }
        public string status { get; set; }
        public string layerCode { get; set; }

        //[NotMapped]
        //public IFormFile File { get; set; }

        public Layer LayerImage { get; set; }

    }
}
