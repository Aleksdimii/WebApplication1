public class Terminal
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<Gate> Gates { get; set; }
}
