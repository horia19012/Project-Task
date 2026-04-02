using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DeviceManagement.model
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        [Column("id")]
        public int Id { get; set;}
        [Column("name")]
        public String ?Name {get; set;}
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Column("role")]
        public Role ?Role{get; set;} 
        [Column("location")]
        public String ?Location {get; set;}
        [Column("email")]
        public String ?Email {get; set;}
        [Column("password_hash")]
        public String ?PasswordHash {get; set;}

    }
}