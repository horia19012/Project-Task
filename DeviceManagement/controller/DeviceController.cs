
using DeviceManagement.Config;
using DeviceManagement.model;
using DeviceManagement.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata.Ecma335;

namespace DeviceManagement.controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {

        private readonly IDeviceService _service;
        private readonly IGroqService _groqService;

        public DeviceController(IDeviceService service, IGroqService groqService)
        {
            _service = service;
            _groqService = groqService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetDevice(int id)
        {
            var device = await _service.GetDeviceByIdAsync(id);
            if (device == null) return NotFound();
            return Ok(device);

        }

        [HttpGet()]
        public async Task<ActionResult> GetAllDevices()
        {
            var devices = await _service.GetAllDevicesAsync();
            return Ok(devices);
        }

        [HttpPost]
        public async Task<ActionResult> AddDevice([FromBody] Device device)
        {
            try
            {
                var addedDevice = await _service.AddDeviceAsync(device);
                return Ok(addedDevice);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDevice(int id, [FromBody] Device device)
        {
            if (id != device.Id) return BadRequest();

            var updatedDevice = await _service.UpdateDeviceAsync(device);

            if (!updatedDevice) return NotFound();

            return Ok(updatedDevice);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteDevice(int id)
        {
            var deleted = await _service.DeleteDeviceAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/assign/{userId}")]
        public async Task<ActionResult> AssignToUser(int id, int userId)
        {
            var updatedDevice = await _service.AssignToUser(id, userId);
            if (updatedDevice == null)
            {
                return NotFound();
            }
            return Ok(updatedDevice);
        }

        [HttpPut("{id}/unassign")]
        public async Task<ActionResult> UnassignFromUser(int id)
        {
            var updatedDevice = await _service.UnassignFromUser(id);
            if (updatedDevice == null)
            {
                return NotFound();
            }
            return Ok(updatedDevice);
        }

        [HttpPost("create_description")]
        public async Task<ActionResult> CreateDescription([FromBody] Device device, CancellationToken cancellationToken)
        {
            if (device is null)
            {
                return BadRequest(new { message = "Device is required." });
            }

            var prompt = $"Create a concise and informative description for the following device:\n\n" +
                        $"Name: {device.Name}\n" +
                        $"Type: {device.Type}\n" +
                        $"Manufacturer: {device.Manufacturer}\n" +
                        $"Operating System: {device.OperatingSystem}\n" +
                        $"RAM: {device.Ram} GB\n" +
                        $"Processor: {device.Processor}\n" +
                        $"The description should highlight the key features and specifications of the device.";
            try
            {
                var answer = await _groqService.SendPromptAsync(prompt, cancellationToken);
                return Ok(new { content = answer });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway, new { message = "Groq call failed.", detail = ex.Message });
            }
        }
    }
}