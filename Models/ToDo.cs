using DotLiquid;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class ToDo : ILiquidizable
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }

        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(this);
        }
    }
}
