
using ValidationGenerator.Shared;

namespace TestLab;

[ValidationGenerator]
public partial class Product
{
    [NotNullGenerator]
    public string Id { get; set; }

    [NotNullGenerator]
    public string Name { get; set; }

    [NotNullGenerator(ErrorMessage = "Product code cannot be null")]
    public string Code { get; set; }
}
