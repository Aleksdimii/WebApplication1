public class Gate
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int TerminalId { get; set; }
    public virtual Terminal Terminal { get; set; }
}
