
namespace Domain.Options;

public class PasswordHashOptions
{
     public static string Section = "PasswordHash";
    public int SaltByteSize { get; set; }
    public int HashByteSize { get; set; }
    public string? HashingAlgorithm { get; set; }
    public int MinIteration { get; set; }
    public int MaxIteration { get; set; }
}
