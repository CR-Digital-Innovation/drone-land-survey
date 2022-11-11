using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using Azure.Storage.Blobs;
using log4net;
using Cr.Igrs.CloudSync.Log;

namespace Cr.Igrs.CloudSync
{
    public partial class ImageSync : ServiceBase
    {
        private Timer timer;
        private List<string> foldersToSync;
        private bool isSyncing = false;
        private Logger log = new Logger();

        public ImageSync()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            LogEvents("Starting the synchronization...", 1, null);
            log = new Logger();
            LogEvents("Reading the configuration...", 1, null);
            ReadConfig();
            
            timer = new Timer(Settings.ElapsedTime);
            timer.Enabled = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //throw new NotImplementedException();

            if (Environment.UserInteractive) timer.Stop();
            LogEvents("Starting the synchronization...", 1, null);
            if (!isSyncing)
            {
                LogEvents("Reading the folders for sync...", 1, null);
                GetFoldersforSync();
                LogEvents("Starting the configuration...", 1, null);
                LoadImagesToCloud();
            }
        }

        private void LoadImagesToCloud()
        {
            string connectionString = File.ReadAllText(Settings.VendorKefFile);
            if (!isSyncing)
            {
                isSyncing = true;

            }
            try
            {
                LogEvents("Creating the Blob client...", 1, null);

                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                var blobContainer = blobServiceClient.GetBlobContainerClient("graphs-data");

                foreach (string path in foldersToSync)
                {
                    string eofFile = "";
                    var files = Directory.GetFiles(Path.GetDirectoryName(path), "*.*", SearchOption.AllDirectories);
                    var folder = "";
                    

                    foreach (string file in files)
                    {
                        if (Path.GetExtension(file).ToLower() == ".eof")
                        {
                            eofFile = file;
                            continue;
                        }
                        folder = Path.GetFileName(Path.GetDirectoryName(path)) + "/" + Path.GetFileName(file);
                        BlobClient bc = blobContainer.GetBlobClient(folder);
                        LogEvents(String.Format("loading the file {0} to cloud...",file), 1, null);

                        using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                            bc.Upload(fs, overwrite: true);
                        LogEvents(String.Format("completed the file {0} to cloud...", file), 1, null);

                    }
                    folder = Path.GetFileName(Path.GetDirectoryName(path)) + "/" + Path.GetFileName(eofFile);
                    var blobClient = blobContainer.GetBlobClient(folder);
                    LogEvents("completed the EOF file loading to cloud...", 1, null);

                    using (FileStream fs = new FileStream(eofFile, FileMode.Open, FileAccess.Read))
                        blobClient.Upload(fs, overwrite: true);
                }
                isSyncing = false;
                LogEvents("Completed the Sync",1,null);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogEvents("", 3, ex);
            }        
        }
        private void GetFoldersforSync()
        {
            foldersToSync = new List<string>();
            LogEvents("Getting the folders for the Sync", 1, null);

            var syncFile = Directory.GetFiles(Settings.Sourcefolder, Settings.EofExtension, SearchOption.AllDirectories);
            foreach (string file in syncFile)
            {
                if (!foldersToSync.Contains(Path.GetFullPath(file)))
                {
                    LogEvents(String.Format("Adding the folder {0} the Sync",Path.GetDirectoryName(file)), 1, null);
                    foldersToSync.Add(Path.GetFullPath(file));
                }
            }
        }
        protected override void OnStop()
        {
            LogEvents("Stopping the Sync", 1, null);
            timer.Stop();
        }

        private void ReadConfig()
        {
            LogEvents("Setting the source folder the Sync", 1, null);

            Settings.Sourcefolder = ConfigurationManager.AppSettings["SourceFolder"].ToString();
            if (!Directory.Exists(Settings.Sourcefolder)) 
                throw new ArgumentException(String.Format("Source folder {0} missing", Settings.Sourcefolder));
            LogEvents("Setting the vendor key", 1, null);
            string vendorKeyFile = ConfigurationManager.AppSettings["VendorKeyFile"].ToString();
            Settings.VendorKefFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, vendorKeyFile);
            if (!File.Exists(Settings.VendorKefFile))
                throw new ArgumentException(String.Format("Source folder {0} missing", Settings.VendorKefFile));
            LogEvents("Setting the elapsed time", 1, null);
            Settings.ElapsedTime = 60000* double.Parse(ConfigurationManager.AppSettings["TimerInMins"].ToString());
            LogEvents("Setting the EOF extension", 1, null);
            Settings.EofExtension= ConfigurationManager.AppSettings["EofFileExtension"].ToString();
            Settings.ContainerName= ConfigurationManager.AppSettings["ContainerName"].ToString();
            Settings.BlobName= ConfigurationManager.AppSettings["BlobName"].ToString();
        }
        private void LogEvents(string message, int logtype, Exception ex )
        {
   
            if(Environment.UserInteractive)
            {
                Console.WriteLine(message);
            }
            
            switch (logtype)
            {
                case 1:
                    log.LogInformation(message);
                    break;
                    case 2:
                    log.LogError(message);
                    break;
                case 3:
                    log.LogException(ex);
                    break;
            }   
            
        }
    }
}
