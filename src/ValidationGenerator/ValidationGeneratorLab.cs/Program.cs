using ValidationGenerator.Shared;

User user = new()
{
    Id = null
};
user.Validate();


Console.WriteLine("Test");


[ValidationGenerator]
public partial class User 
{
    [NotNull(ErrorMessage = "The message that will be used when throw exception and validation error")]
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
