using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common.Exceptions;
using System.Collections;

namespace AzureIotDeviceOperations.Services
{
    public static class ManageIoTDevicesService
    {
        private static string? deviceKey;
        private static DeviceClient? deviceClient;
        private static RegistryManager? registryManager;
        private static string iotHubUri = "MyLTIIoTHub1.azure-devices.net";
        private static string iotHubConnectionString = "HostName=MyLTIIoTHub1.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=VaRNerMRVgnoAnwkegz6BOGJbQudi3btDIWLW5aaBJc=";

        public static DeviceClient AddDevice(string deviceName, bool isIoTEdge)
        {
            RegisterDeviceAsync(deviceName, isIoTEdge).Wait();

            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceName, deviceKey));

            return deviceClient;
        }

        public async static Task<IEnumerable<Device>> GetDevicesAsync(int deviceCount)
        {
            var jsons = new List<string>();

            var registryManager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);

            var query = await registryManager.GetDevicesAsync(10);
            // CreateQuery("SELECT * FROM devices;");
            //while (query.HasMoreResults)
            //{
            //    var page = await query.GetNextAsJsonAsync();
            //    foreach (var json in page)
            //    {
            //        jsons.Add(json);
            //    }
            //}

            return query;

        }

        public async static Task<Device> GetDevice(string deviceId)
        {

            var registryManager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);


           var res = await registryManager.GetDeviceAsync(deviceId);

            return res;
        }

        public async static Task RemoveDevice(string deviceId)
        {

            var registryManager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);

            await registryManager.RemoveDeviceAsync(deviceId);

        }

        private static async Task RegisterDeviceAsync(string deviceName, bool isIoTEdge)
        {
            Device? device;
            registryManager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);
            try
            {
                if (isIoTEdge)
                {
                    device = new Device(deviceName);
                    device.Capabilities = new Microsoft.Azure.Devices.Shared.DeviceCapabilities()
                    {
                        IotEdge = true
                    };
                    device = await registryManager.AddDeviceAsync(device);
                }
                else
                {
                    device = await registryManager.AddDeviceAsync(new Device(deviceName));
                }
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceName);
            }
            deviceKey = device.Authentication.SymmetricKey.PrimaryKey;
        }
    }
}
