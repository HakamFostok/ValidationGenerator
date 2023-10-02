using ValidationGenerator.Shared;

namespace TestLab;

[ValidationGenerator]
public partial class User
{
    [NotNull(ErrorMessage ="Id cannot be null")]
    public string Id { get; set; }

    [NotNull(ErrorMessage = "Name cannot be null")]
    public string Name { get; set; }    
}


