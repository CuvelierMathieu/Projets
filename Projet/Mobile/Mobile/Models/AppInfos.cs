using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mobile.Models
{
    public class AppInfos
    {
        #region Attributes
        public Guid DeviceId { get; set; }
        public DateTime LastUpload { get; set; }
        public DateTime LastDownload { get; set; }
        #endregion

        #region Contructors
        public AppInfos()
        { }

        public AppInfos(bool GenerateANewOne)
        {
            if (GenerateANewOne)
            {
                DeviceId = Guid.NewGuid();
                LastDownload = DateTime.Now;
                LastUpload = DateTime.Now;
            }
        }
        #endregion

        #region Methods
        public static AppInfos Load()
        {
            return JsonConvert.DeserializeObject<AppInfos>(
                DependencyService.Get<ISaveAndLoad>().LoadText("infos.txt"));
        }
        public void Save()
        {
            DependencyService.Get<ISaveAndLoad>().SaveText("infos.txt",
                    JsonConvert.SerializeObject(this));
        }
        #endregion
    }
}
