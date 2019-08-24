using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace m4rcus.TuyaCore
{
    public struct TuyaStatus
    {
        public bool Powered;
        public int Delay;
        public double Current_mA;
        public double Power_W;
        public double Voltage_V;
    }
    

    public class TuyaPlug : TuyaDevice
    {
        public async Task<TuyaStatus> GetStatus()
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
                TuyaStatus tuyaStatus = new TuyaStatus();

                JObject result = JObject.Parse(json);
                if (result["dps"] != null)
                {
                    if (result["dps"]["1"] != null)
                        tuyaStatus.Powered = (bool)result["dps"]["1"].ToObject(typeof(bool));
                    if (result["dps"]["2"] != null)
                        tuyaStatus.Delay = (int)result["dps"]["2"].ToObject(typeof(int));
                    if (result["dps"]["4"] != null)
                        tuyaStatus.Current_mA = (double)result["dps"]["4"].ToObject(typeof(double));                    
                    if (result["dps"]["5"] != null)
                        tuyaStatus.Power_W = (double)result["dps"]["5"].ToObject(typeof(double));
                    if (result["dps"]["6"] != null)
                        tuyaStatus.Voltage_V = (double)result["dps"]["6"].ToObject(typeof(double))/10.0;
                    if (result["dps"]["18"] != null)
                        tuyaStatus.Current_mA = (double)result["dps"]["18"].ToObject(typeof(double));                    
                    if (result["dps"]["19"] != null)
                        tuyaStatus.Power_W = (double)result["dps"]["19"].ToObject(typeof(double)) / 10.0;
                    if (result["dps"]["20"] != null)
                        tuyaStatus.Voltage_V = (double)result["dps"]["20"].ToObject(typeof(double)) / 10.0;
                }
                return (tuyaStatus);
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
