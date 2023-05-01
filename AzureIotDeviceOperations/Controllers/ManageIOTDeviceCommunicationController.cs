using Microsoft.AspNetCore.Mvc;

using AzureIotDeviceOperations.Services;
using Microsoft.Azure.Devices;

namespace AzureIotDeviceOperations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManageIOTDeviceCommunicationController : ControllerBase
    {

        private readonly ILogger<ManageIOTDeviceCommunicationController> _logger;
        public ManageIOTDeviceCommunicationController(ILogger<ManageIOTDeviceCommunicationController> logger)
        {
            _logger = logger;

        }
        [HttpGet]
        [Route("~/[controller]/GetDeviceTwin")]
        public async Task<ActionResult> Get(string deviceName)
        {
            try
            {
                var devices = await ManageIOTDeviceCommunicationService.GetDeviceTwin(deviceName);
                return Ok(devices);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("~/[controller]/UpdateReportedProperties")]
        public ActionResult UpdateReportedProperties(string deviceId)
        {
            ManageIOTDeviceCommunicationService.UpdateReportedProperties();
            return Ok(true);
        }

        [HttpPost]
        [Route("~/[controller]/AddTagsAndQueryDevice")]
        public async Task<ActionResult> AddTagsAndQueryDevice(string deviceName)
        {
            var device = await ManageIOTDeviceCommunicationService.AddTagsAndQueryDevice(deviceName);

            return Ok(device);
        }

        [HttpPost]
        [Route("~/[controller]/SendTelemetryMessages")]
        public ActionResult SendTelemetryMessagesFromDevice(string deviceId)
        {
            ManageIOTDeviceCommunicationService.SendDeviceToCloudMessagesAsync();
            return Ok();
        }
    }
}
