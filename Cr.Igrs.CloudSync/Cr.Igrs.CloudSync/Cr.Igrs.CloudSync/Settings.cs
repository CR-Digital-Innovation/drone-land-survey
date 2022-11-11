using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cr.Igrs.CloudSync
{
    public static class Settings
    {
        public static string VendorKefFile { get; set; }
        public static string Sourcefolder { get; set; }
        public static string EofExtension {get;set;}
        public static double ElapsedTime {get;set;}

        public static string ContainerName { get; set; }    

        public static string BlobName { get; set; } 
    }
}
