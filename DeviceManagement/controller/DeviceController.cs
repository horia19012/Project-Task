
using DeviceManagement.Config;
using DeviceManagement.model;
using DeviceManagement.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        /// <summary>
        /// Retrieves a device by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetDevice(int id)
        {
            var device = await _service.GetDeviceByIdAsync(id);
            if (device == null) return NotFound();
            return Ok(device);

        }

        /// <summary>
        /// Retrieves all devices.
        /// </summary>
        /// <returns>A list of all devices</returns>
        [HttpGet()]
        public async Task<ActionResult> GetAllDevices()
        {
            var devices = await _service.GetAllDevicesAsync();
            return Ok(devices);
        }

        /// <summary>
        /// Retrieves all devices assigned to the currently authenticated user.
        /// </summary>
        /// <returns>A list of devices assigned to the authenticated user</returns>
        [HttpGet("mine")]
        public async Task<ActionResult> GetMyDevices()
        {
            var userIdValue = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            var devices = await _service.GetDevicesByUserIdAsync(userId);
            return Ok(devices);
        }

        /// <summary>
        /// Adds a new device.
        /// </summary>
        /// <param name="device">The device to add</param>
        /// <returns>The added device</returns>
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

        /// <summary>
        /// Updates an existing device.
        /// </summary>
        /// <param name="id">The ID of the device to update</param>
        /// <param name="device">The updated device information</param>
        /// <returns>The updated device</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDevice(int id, [FromBody] Device device)
        {
            if (id != device.Id) return BadRequest();

            var updatedDevice = await _service.UpdateDeviceAsync(device);

            if (!updatedDevice) return NotFound();

            return Ok(updatedDevice);
        }

        /// <summary>
        /// Deletes a device by its ID.
        /// </summary>
        /// <param name="id">The ID of the device to delete</param>
        /// <returns>True if the device was deleted, otherwise false</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteDevice(int id)
        {
            var deleted = await _service.DeleteDeviceAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Assigns a device to a user.
        /// </summary>
        /// <param name="id">The ID of the device to assign</param>
        /// <param name="userId">The ID of the user to assign the device to</param>
        /// <returns>The updated device</returns>

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

        /// <summary>
        /// Unassigns a device from a user by setting the UserId property of the device to null.
        /// </summary>
        /// <param name="id">The ID of the device to unassign</param>
        /// <returns>The updated device</returns>
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

        /// <summary>
        /// Creates a description for a device using the Groq service
        /// </summary>
        /// <param name="device">The device for which to create a description</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The generated description</returns>

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