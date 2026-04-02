
using DeviceManagement.Config;
using DeviceManagement.model;
using DeviceManagement.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DeviceManagement.controller
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {

        private readonly IDeviceService _service;

        public DeviceController(IDeviceService service)
        {
            _service = service;
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

            if(!updatedDevice) return NotFound();

            return Ok(updatedDevice);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>>  DeleteDevice(int id)
        {
            var deleted = await _service.DeleteDeviceAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        


    }
}