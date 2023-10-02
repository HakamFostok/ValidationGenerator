using ValidationGenerator.Shared;

namespace TestLab;

[ValidationGenerator]
public partial class User
{
    [NotNull(ErrorMessage ="User Id cannot be null")]
    public string Id { get; set; }

    [NotNull(ErrorMessage = "User Name cannot be null")]
    public string Name { get; set; }    
}


