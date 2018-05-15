using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jober.Models
{
    public class WorkerSetting
    {
        public int CityId { get; set; }

        public List<int> DistrictIds { get; set; }

        public List<int> CategoryIds { get; set; }
    }

    public static class WorkerSettingExtantion
    {
        public static string ToJSON(this WorkerSetting obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static WorkerSetting ParseToWorkerSetting(this string workerSettingJSON)
        {
            return JsonConvert.DeserializeObject<WorkerSetting>(workerSettingJSON);
        }
    }
}
