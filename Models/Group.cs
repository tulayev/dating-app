using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Group
    {
        [Key]
        public string Name { get; set; }

        public ICollection<Connection> Connections { get; set; } = new List<Connection>();

        public Group()
        {
        }

        public Group(string name)
        {
            Name = name;
        }
    }
}
