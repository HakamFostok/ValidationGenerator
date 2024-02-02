using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ValidationGenerator.Tests;

public class ValidationGeneratorUnitTest
{
    [Fact(DisplayName = "Validation Success")]
    public void ValidationSuccess()
    {
    }

    [Fact(DisplayName = "Validation Fail")]
    public void ValidationFail()
    {
        User user = new();

        Assert.False(user.IsValid);
        Assert.Equal(15, user.GetValidationResult().ValidationResults.Count);
        Assert.Throws<ValidationException>(() => user.ThrowIfNotValid());
    }
}
