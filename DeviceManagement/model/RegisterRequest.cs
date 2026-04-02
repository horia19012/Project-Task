namespace DeviceManagement.model
{
    public class RegisterRequest
    {
        public string ?Email { get; set; }
        public string ?Name { get; set; }
        public string ?Location { get; set; }

        public string ?Role { get; set; }
        public string ?Password { get; set; }
    }
}