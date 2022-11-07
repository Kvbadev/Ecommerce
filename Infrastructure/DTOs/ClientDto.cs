namespace Infrastructure.DTOs;

public class ClientDto
{
    public string Username { get; set; } = default!;
    public IEnumerable<string> Privileges { get; set; } = default!;
    public decimal MoneySpent { get; set; }
    public DateTime CreatedAt { get; set; }
}