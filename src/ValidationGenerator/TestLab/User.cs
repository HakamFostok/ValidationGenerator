using ValidationGenerator.Shared;

namespace TestLab;

[ValidationGenerator]
public partial class User
{
    [NotNull]
    public string Id { get; set; }
}


