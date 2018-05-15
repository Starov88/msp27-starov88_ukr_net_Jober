using Jober.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Areas.API.Models
{
    public class PutWorkerSettings
    {
        public int WorkerId { get; set; }

        public string Guid { get; set; }

        public int CityId { get; set; }

        public List<int> DistrictIds { get; set; }

        public List<int> CategoryIds { get; set; }

        public string GetWorkerSettingsJSON()
        {
            return GetWorkerSettings()?.ToJSON();
        }

        public WorkerSetting GetWorkerSettings()
        {
            if (DistrictIds != null && CategoryIds != null)
            {
                WorkerSetting settings = new WorkerSetting
                {
                    CategoryIds = this.CategoryIds,
                    CityId = this.CityId,
                    DistrictIds = this.DistrictIds
                };
                return settings;
            }
            return null;
        }
    }
}
