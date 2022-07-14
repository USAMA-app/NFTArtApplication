using NFTArtConsole.Models;
using Tedeschi.NFT.Event;
using Tedeschi.NFT.Exception;
using Tedeschi.NFT.Services.Collection;
using Tedeschi.NFT.Services.Layer;
using Tedeschi.NFT.Services.Metadata;
using Tedeschi.NFT.Util;
using System;
using System.ComponentModel;
using System.IO;
using Autofac;
using Tedeschi.NFT.Services.Image;
using Tedeschi.NFT.Services.Generator;

namespace NFTArtConsole
{
    public class Program
    {
        private static  ILayerService layerService;
        private static  ICollectionService collectionService;
        private static  IMetadataService metadataService;
        //private static IContainer container;
        private static Nft model = new Nft();




        static void Main(string[] args)
        {
          

            // Get instance of Autofac Container
            var container = Configure();

            layerService = container.Resolve<ILayerService>();
            collectionService = container.Resolve<ICollectionService>();
            metadataService = container.Resolve<IMetadataService>();    
                

            model.LayerFolderPath = @"E:\NFTApplication\sample\layers\";
            model.OutputFolderPath = @"E:\NFTApplication\";

            model.MetaDataType = 1;
            model.MetaDataDescription = "Made by NFT.net";
            model.MetadataImageBaseUri = @"https://someurl.com/nft";
            model.MetadataExternalUrl = "";
            model.MetadataUseFileExtension = true;

            model.collectionSize = "10";
            model.InitialNumber = "1";
            model.ImagePrefix = "nft#1 ";


            //Console.WriteLine("{0} {1}", model.LayerFolderPath, model.OutputFolderPath);
            //Console.WriteLine("{0} {1}", model.MetaDataType, model.MetaDataDescription);
            //Console.WriteLine("{0}", model.MetaDataType);
            //Console.WriteLine("{0} {1}", model.MetadataImageBaseUri, model.MetadataExternalUrl);
            //Console.WriteLine("{0}", model.MetadataUseFileExtension);
            //Console.WriteLine("{0}", model.collectionSize);
            //Console.WriteLine("{0}", model.InitialNumber);
            //Console.WriteLine("{0}", model.ImagePrefix);

            //Console.ReadLine();


            ValidateForGeneration(model.LayerFolderPath, model.OutputFolderPath, model.MetadataImageBaseUri, model.collectionSize, model.InitialNumber);
            collectionService.CollectionItemStatus += new EventHandler<ImageEventArgs>(OnCollectionItemProcessed);
            collectionService.Create(model.LayerFolderPath, model.OutputFolderPath, model.MetaDataType, model.MetaDataDescription, model.MetadataImageBaseUri, model.MetadataExternalUrl="test", model.MetadataUseFileExtension, int.Parse(model.collectionSize), int.Parse(model.InitialNumber), model.ImagePrefix);

           


        }

        private static void OnCollectionItemProcessed(object sender, ImageEventArgs e)
        {
            var status = string.Format("Testing", e.CollectionItemId, model.collectionSize);

           
                var toolStripStatus = status;

                var Maximum = int.Parse(model.collectionSize);
                var Value = e.CollectionItemId;
          
        }
        private static void ValidateForGeneration(string layersFolder, string outputFolder, string imageBaseUri, string collectionSize, string collectionInitialNumber)
        {
            if (!Directory.Exists(layersFolder))
            {
                throw new InvalidSettingException("Layer folder");
            }

            if (!Directory.Exists(outputFolder))
            {
                throw new InvalidSettingException("Output folder");
            }

            if (!ValidationUtil.IsUri(imageBaseUri))
            {
                throw new InvalidSettingException("Image Base URI");
            }

            if (!ValidationUtil.IsNumeric(collectionSize) || int.Parse(collectionSize) <= 0)
            {
                throw new InvalidSettingException("Collection size");
            }

            if (!ValidationUtil.IsNumeric(collectionInitialNumber) || int.Parse(collectionInitialNumber) < 0)
            {
                throw new InvalidSettingException("Collection initial number");
            }
        }

        public static Autofac.IContainer Configure()
        {
            var builder = new ContainerBuilder();

            
            builder.RegisterType<LayerService>().As<ILayerService>();
            builder.RegisterType<ImageService>().As<IImageService>();
            builder.RegisterType<GeneratorService>().As<IGeneratorService>();
            builder.RegisterType<MetadataService>().As<IMetadataService>();
            builder.RegisterType<RarityService>().As<IRarityService>();
            builder.RegisterType<CollectionService>().As<ICollectionService>();


            return builder.Build();
        }



    }
}