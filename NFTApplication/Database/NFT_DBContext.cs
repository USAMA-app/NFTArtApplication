using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Consumer.Model;

namespace RabbitMQ.Producer.Database
{
    public class NFT_DBContext:DbContext
    {
        public NFT_DBContext(DbContextOptions<NFT_DBContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Layer>()
                .HasMany(g => g.LayerImages)
                .WithOne(s => s.LayerImage)
                .HasForeignKey(k =>k.layerCode)
                .OnDelete(DeleteBehavior.NoAction);
                        
        }

        public DbSet<Layer> Layers { get; set; }
        public DbSet<Image>  Images { get; set; }
        

    }
}
