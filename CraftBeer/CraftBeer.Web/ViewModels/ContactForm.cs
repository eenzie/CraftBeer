using System.ComponentModel.DataAnnotations;

namespace CraftBeer.Web.ViewModels;

public class ContactForm
{
    [Required(ErrorMessage = "Name is a required field")]
    public string Alias { get; set; } = "";

    [Required]
    [EmailAddress(ErrorMessage = "not a valid email")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Subject is a required field")]
    public string Subject { get; set; } = "";

    [Required(ErrorMessage = "Message is a required field")]
    public string Message { get; set; } = "";
}
