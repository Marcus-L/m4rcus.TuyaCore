using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace m4rcus.TuyaCore
{
    public class TuyaPlug : TuyaDevice
    {
        public async Task<(bool Powered, int Delay)> GetStatus()
        {
            var cmd = new Dictionary<string, object>
            {
                ["gwId"] = Id,
                ["devId"] = Id
            };
            string json = JsonConvert.SerializeObject(cmd);
            var payload = CreatePayload(json, false);
            var buffer = ConstructBuffer(payload, 10);
            json = ReadBuffer(await Send(buffer));
            try
            {
                dynamic result = JObject.Parse(json);
                return (result.dps["1"], result.dps["2"]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading buffer: {json}", ex);
            }
        }

        public async Task SetStatus(bool status, int delay = 0)
        {
            var cmd = new Dictionary<string, object>
            {
                ["t"] = (DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds.ToString("0"),
                ["devId"] = Id,
                ["dps"] = new Dictionary<string, object>
                {
                    ["1"] = status,
                    ["2"] = delay
                },
                ["uid"] = "" // this key is required but the value doesn't appear to be used
            };
            string json = JsonConvert.SerializeObject(cmd);
            var payload = CreatePayload(json);
            var buffer = ConstructBuffer(payload, 7);
            var result = ReadBuffer(await Send(buffer)); // returns empty string on success
            if (result != "")
            {
                throw new Exception($"Unexpected result: {result}");
            }
        }
    }
}
