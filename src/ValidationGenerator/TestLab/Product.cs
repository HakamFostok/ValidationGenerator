
using ValidationGenerator.Shared;

namespace TestLab;

[ValidationGenerator]
public partial class Product
{
    [NotNull]
    public string Id { get; set; }

    [NotNull]
    public string Name { get; set; }

    [NotNull(ErrorMessage = "Product code cannot be null")]
    public string Code { get; set; }
}
