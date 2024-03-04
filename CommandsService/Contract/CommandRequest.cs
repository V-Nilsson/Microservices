using System.ComponentModel.DataAnnotations;

namespace CommandsService.Contract;

public class CommandRequest
{
    [Required]
    public string? HowTo { get; set; }
    [Required]
    public string? CommandLine { get; set; }
}
