using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class ToDo
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
    }
}
