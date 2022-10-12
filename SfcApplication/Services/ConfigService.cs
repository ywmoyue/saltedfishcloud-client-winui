using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Downloader;
using Honoo.Configuration;
using SfcApplication.Models.Configs;

namespace SfcApplication.Services
{
    public class ConfigService
    {
        private readonly LocalFileIOService m_localFileIoService;
        private readonly UserConfig m_userConfig;
        private readonly DownloadConfiguration m_downloadConfiguration;

        public ConfigService(LocalFileIOService localFileIoService, UserConfig userConfig, DownloadConfiguration downloadConfiguration)
        {
            m_localFileIoService = localFileIoService;
            m_userConfig = userConfig;
            m_downloadConfiguration = downloadConfiguration;
        }

        public async Task SetIsRemoveDownloadedTaskWithFile(bool isRemoveDownloadedTaskWithFile)
        {
            var paths = new List<string>()
            {
                nameof(UserConfig),
                nameof(m_userConfig.IsRemoveDownloadedTaskWithFile)
            };
            await m_localFileIoService.SetAppSettingsConfig(paths, isRemoveDownloadedTaskWithFile);
        }

        public async Task SetIsRemoveDownloadingTaskWithFile(bool isRemoveDownloadingTaskWithFile)
        {
            var paths = new List<string>()
            {
                nameof(UserConfig),
                nameof(m_userConfig.IsRemoveDownloadingTaskWithFile)
            };
            await m_localFileIoService.SetAppSettingsConfig(paths, isRemoveDownloadingTaskWithFile);
        }

        public async Task SetDefaultDownloadPath(string folderPath)
        {
            var paths = new List<string>()
            {
                nameof(DownloadConfiguration),
                nameof(m_downloadConfiguration.TempDirectory)
            };
            await m_localFileIoService.SetAppSettingsConfig(paths, folderPath);
        }
    }
}
