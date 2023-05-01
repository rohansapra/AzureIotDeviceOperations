using Microsoft.AspNetCore.Mvc;

using AzureIotDeviceOperations.Services;

namespace AzureIotDeviceOperations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManageIOTDeviceController : ControllerBase
    {

        private readonly ILogger<ManageIOTDeviceController> _logger;
        public ManageIOTDeviceController(ILogger<ManageIOTDeviceController> logger)
        {
            _logger = logger;

        }
        [HttpGet]
        [Route("~/[controller]/GetDevices")]
        public async Task<ActionResult> Get(int deviceCount = 10)
        {
            try
            {
                var devices = await ManageIoTDevicesService.GetDevicesAsync(deviceCount);
                return Ok(devices);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("~/[controller]/DeviceDetails")]
        public async Task<ActionResult> Details(string deviceId)
        {
            var device = await ManageIoTDevicesService.GetDevice(deviceId);
            return Ok(device);
        }

        [HttpPost]
        [Route("~/[controller]/CreateDevice")]
        public ActionResult Create(string deviceName, bool isIoTEdge = false)
        {
            // Register Device with Azure
            var device = ManageIoTDevicesService.AddDevice(deviceName, isIoTEdge);

            return Ok(device);
        }

        [HttpDelete]
        [Route("~/[controller]/DeleteDevice")]
        public ActionResult Delete(string deviceId)
        {
            ManageIoTDevicesService.RemoveDevice(deviceId).Wait();
            return Ok();
        }

//        1. ManageIOTDeviceController -> 
//	Add device/Edge device to the Iot Hub

//    Add device/Edge device to the Iot Hub with Device-Twin
//    Remove device/Edge device from the Iot Hub

//2. DeviceCommunicationController -> 
//	Get the device-twin for device/Iot edge device
//    Update the desired properties from iot hub and read it in the app Device-Twin(console-app) -> preffered
//    Update the reported properties/tags from Iot Device/Edge device

//    Send telemetry messages from device to Iot Hub


//Problem statement – 301

//1.       Create IoT Hub, IoT edge devices in portal

//2.       Create two.Net Core API apps

//2.1. Send message to IoT device, telemetry msgs too

//2.2. Upload to Blob storage, table storage, queues

//2.3. Do CRUD operations in both cases, telemetry and storage

//Build pipeline and Deploy one of the above apps to App service, Container registry (Docker), AKS (Kubernetes)

//DEVICE:  

//https://learn.microsoft.com/en-us/dotnet/api/microsoft.azure.devices.registrymanager.updatedeviceasync?view=azure-dotnet  

//https://learn.microsoft.com/en-us/azure/iot-hub/iot-hub-csharp-csharp-twin-getstarted

//https://www.c-sharpcorner.com/article/how-to-send-telemetry-from-an-iot-device-to-the-azure-iot-hub-using-c-sharp/

//RegistryManager.UpdateDeviceAsync Method (Microsoft.Azure.Devices) - Azure for .NET Developers

//Update the mutable fields of the device registration

//Create IoT Hub

//Use the Azure portal to create an IoT Hub | Microsoft Learn

//Use the Azure portal to create an IoT Hub

//How to create, manage, and delete Azure IoT hubs through the Azure portal.Includes information about pricing tiers, scaling, security, and messaging configuration.

//STORAGE:  

//https://learn.microsoft.com/en-us/azure/storage/queues/storage-dotnet-how-to-use-queues?tabs=dotnet  

//https://code-maze.com/azure-table-storage-aspnetcore/

//https://www.c-sharpcorner.com/article/azure-storage-tables/  

//https://www.c-sharpcorner.com/article/azure-storage-crud-operations-in-mvc-using-c-sharp-part-two/  

//https://learn.microsoft.com/en-us/python/api/azure-storage-file-share/azure.storage.fileshare.shareclient?view=azure-python

//Get started with Azure Queue Storage using .NET - Azure Storage

//Azure Queue Storage provide reliable, asynchronous messaging between application components.Cloud messaging enables your application components to scale independently.

//RegistryManager.UpdateDeviceAsync Method(Microsoft.Azure.Devices) - Azure for .NET Developers

//Update the mutable fields of the device registration


    }
}
