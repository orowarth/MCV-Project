using MCBADataLibrary.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MCBAWebApp.Models;

public class ImageViewModel
{
    [DisplayName("New Image")]
    [Required]
    public IFormFile? Image { get; set; }

    public CustomerImage? CurrentImage { get; set; } = null;
}
