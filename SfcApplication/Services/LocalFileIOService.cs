using Newtonsoft.Json;
using SfcApplication.Models.Common;
using SfcApplication.Models.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using SfcApplication.Converters;

namespace SfcApplication.Services
{
    public class LocalFileIOService
    {
        private readonly ClientConfig m_clientConfig;
        private readonly StorageFolder m_localFolder;

        public LocalFileIOService(ClientConfig clientConfig)
        {
            m_clientConfig = clientConfig;
            m_localFolder = ApplicationData.Current.LocalFolder;
        }

        public async Task SetUserToken(string token)
        {
            await Write(new List<string>() { "user" }, "token", token);
        }

        public async Task<string> GetUserToken()
        {
            var token = await Read<string>(new List<string>() { "user" }, "token");
            return token;
        }

        public async Task SetDownloadTaskList(List<DownloadTask> downloadTasks)
        {
            await Write(new List<string>() { "download" }, "list", downloadTasks);
        }

        public async Task<List<DownloadTask>> GetDownloadTaskList()
        {
            var downloadTaskList = await Read<List<DownloadTask>>(new List<string>() { "download" }, "list");
            return downloadTaskList;
        }

        //private async Task Write(List<string> folderNames, string fileName, object data)
        //{
        //    StorageFolder currentFolder = m_localFolder;
        //    foreach (var fName in folderNames)
        //    {
        //        currentFolder = await currentFolder.CreateFolderAsync(fName, CreationCollisionOption.OpenIfExists);
        //    }

        //    var file = await currentFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
        //    string dataStr = JsonConvert.SerializeObject(data);
        //    await FileIO.WriteTextAsync(file, dataStr);
        //}

        private async Task Write(List<string> folderNames, string fileName, object data)
        { 
            var currentFolder = new DirectoryInfo(m_localFolder.Path);
            foreach (var fName in folderNames)
            {
                var tempFolder = new DirectoryInfo($"{currentFolder.FullName}/{fName}");
                if(!tempFolder.Exists)
                    tempFolder.Create();
                currentFolder = tempFolder;
            }

            var file = new FileInfo($"{currentFolder.FullName}/{fileName}");
            string dataStr = JsonConvert.SerializeObject(data);
            var stream=file.CreateText();
            await stream.WriteAsync(dataStr);
            stream.Close();
        }

        //private async Task<T> Read<T>(List<string> folderNames, string fileName)
        //{
        //    StorageFolder currentFolder = m_localFolder;
        //    foreach (var fName in folderNames)
        //    {
        //        var fI = (await currentFolder.TryGetItemAsync(fName));
        //        if (fI == null) return default;
        //        currentFolder = (StorageFolder)fI;
        //    }
        //    var fileI = await currentFolder.TryGetItemAsync(fileName);
        //    if (fileI == null) return default;
        //    var file = (StorageFile)fileI;
        //    var dataStr = await FileIO.ReadTextAsync(file);
        //    var data = JsonConvert.DeserializeObject<T>(dataStr);
        //    return data;
        //}

        private async Task<T> Read<T>(List<string> folderNames, string fileName)
        {
            var path = m_localFolder.Path;
            foreach (var folderName in folderNames)
            {
                path += $"/{folderName}";
            }

            var file = new FileInfo($"{path}/{fileName}");
            if (!file.Exists) return default;
            var stream = file.OpenText();
            var dataStr = await stream.ReadToEndAsync();
            stream.Close();
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new DownloadStorageConverter());
            var data = JsonConvert.DeserializeObject<T>(dataStr, settings);
            return data;
        }
    }
}
