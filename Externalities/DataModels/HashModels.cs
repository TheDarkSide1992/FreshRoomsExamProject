namespace Infastructure;

public class HashModel
{
    public int id { get; set; }
    public required string Hash { get; set; }
    public required string Salt { get; set; }
    public required string Algorithm { get; set; }
}