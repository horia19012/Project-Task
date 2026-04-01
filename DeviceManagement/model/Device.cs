using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceManagement.model
{
    public class Device
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public  string ?Name { get; set; }

        [Column("manufacturer")]
        public  string ?Manufacturer { get; set; }

        [Column("type")]
        public string ?Type { get; set; }
        [Column("operating_system")]
        public string ?OperatingSystem { get; set; }
        [Column("os_version")]
        public string ?OsVersion { get; set; }
        [Column("processor")]
        public string ?Processor { get; set; }
        [Column("ram")]
        public string ?Ram { get; set; }
        [Column("description")] 
        public string ?Description { get; set; }
        [Column("userId")]
        public int? UserId { get; set; }

    }
}