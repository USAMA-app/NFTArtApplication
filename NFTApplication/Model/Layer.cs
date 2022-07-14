using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQ.Consumer.Model
{
    public class Layer
    {
        [Key]
        public int ID { get; set; }

        public string layerCode { get; set; }
        public string layerName { get; set; }
        public float rarity { get; set; }
        public int imagecount { get; set; }
        public string status { get; set; }

        [InverseProperty("LayerImage")]
        public ICollection<Image> LayerImages { get; set; }

      
    }
}
