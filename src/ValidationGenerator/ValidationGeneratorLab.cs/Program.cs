




using ValidationGenerator.Shared;

User user = new()
{
    Id = null
};
user.Validate();


Console.WriteLine("Test");


public class User : ValidationMarker
{
    [NotNull]
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class ValidationMarker
{
    public void Validate()
    {

    }

    public Task ValidateAsync()
    {
        return Task.CompletedTask;
    }
}

