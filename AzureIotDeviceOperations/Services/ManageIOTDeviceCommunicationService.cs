using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Text;
using TransportType = Microsoft.Azure.Devices.Client.TransportType;

namespace AzureIotDeviceOperations.Services
{
    public class ManageIOTDeviceCommunicationService
    {
        static string DeviceConnectionString = "HostName=MyLTIIoTHub1.azure-devices.net;DeviceId=iotdevice1;SharedAccessKey=DcQvN4O21EkfiNrOLxBbR91lHWYo2bCJCQqyDaykSXM=";
        static DeviceClient Client = null;
        static RegistryManager registryManager;
        private static string iotHubConnectionString = "HostName=MyLTIIoTHub1.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=rwZf4xmWKjCEx667CYNRcv2adnWfN0oVrE4bZW3CMEY=";

        public ManageIOTDeviceCommunicationService(IConfiguration configuration)
        {

        }

        public static async Task<Twin> GetDeviceTwin(string DeviceId)
        {
            try
            {
                registryManager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);
                var twin = await registryManager.GetTwinAsync(DeviceId);
                return twin;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async void UpdateReportedProperties()
        {
            try
            {
                Client = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Mqtt);
                TwinCollection reportedProperties, connectivity;
                reportedProperties = new TwinCollection();
                connectivity = new TwinCollection();
                connectivity["type"] = "demo";
                reportedProperties["connectivity"] = connectivity;
                await Client.UpdateReportedPropertiesAsync(reportedProperties);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<IEnumerable<Twin>> AddTagsAndQueryDevice(string DeviceId)
        {
            registryManager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);
            var twin = await registryManager.GetTwinAsync(DeviceId);
            var patch =
                @"{
                    tags: {
                        location: {
                            region: 'IN',
                            plant: 'LTIM'
                        }
                    }
                }";
            await registryManager.UpdateTwinAsync(twin.DeviceId, patch, twin.ETag);

            var query = registryManager.CreateQuery("SELECT * FROM devices WHERE tags.location.plant = 'LTIM' AND properties.reported.connectivity.type = 'demo'", 100);
            var updatedTwin = await query.GetNextAsTwinAsync();
            return updatedTwin;
        }

        public static async void SendDeviceToCloudMessagesAsync()
        {
            try
            {
                DeviceClient s_deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString,
                  TransportType.Mqtt);
                double minTemperature = 20;
                double minHumidity = 60;
                Random rand = new Random();

                int messageCount = 0;

                while (messageCount <= 10)
                {
                    double currentTemperature = minTemperature + rand.NextDouble() * 15;
                    double currentHumidity = minHumidity + rand.NextDouble() * 20;

                    // Create JSON message  

                    var telemetryDataPoint = new
                    {

                        temperature = currentTemperature,
                        humidity = currentHumidity
                    };

                    string messageString = "Hello";

                    messageString = JsonConvert.SerializeObject(telemetryDataPoint);

                    var message = new Microsoft.Azure.Devices.Client.Message(Encoding.ASCII.GetBytes(messageString));

                    // Add a custom application property to the message.  
                    // An IoT hub can filter on these properties without access to the message body.  
                    //message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");  

                    // Send the telemetry message  
                    await s_deviceClient.SendEventAsync(message);
                    
                    await Task.Delay(1000 * 10);
                    messageCount++;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
