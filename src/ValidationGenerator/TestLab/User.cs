
using ValidationGenerator.Shared;

namespace TestLab;

[ValidationGenerator(GenerateThrowIfNotValid = false, GenerateIsValidProperty = true, GenerateValidationResult = true)]
public partial class User
{
    [NotNullGenerator(ErrorMessage = "User Id cannot be null")]
    public string? Id { get; set; }

    [NotNullGenerator(ErrorMessage = "User Name cannot be null")]
    public string? Name { get; set; }

    [EmailGenerator]
    public string Email { get; set; } = "textX";

    [EmailGenerator]
    public string Email2 { get; set; } = "emirhan.aksoy@everi.com";

    [NotEmptyGenerator(ErrorMessage = "THIS IS EMPTY !!")]
    [EmailGenerator(ErrorMessage = "THIS IS EMPTY AND NOT A VALID EMAIL")]
    public string AdditionalEmail { get; set; } = "";

    [Base64Generator]
    public string HelloWorldBase64 { get; set; } = "SGVsbG8gd29ybGQgIQ==";

    [Base64Generator]
    public string InvalidBase64 { get; set; } = "123!23rqsdcasdc asf asdfas 3!'''''+!32 adf.,asdf ";

    [MinimumLengthGenerator(MinimumLength =20)]
    public string RockBandName { get; set; } = "The Metalica";

    [MaximumLengthGenerator(MaximumLength = 10)]
    public string CarModel { get; set; } = "BEST_CAR_MODEL_EVER";

    [RegexMatchGenerator(Regex = "\b[A-Z][a-zA-Z]*\b", ErrorMessage = "Should starts with upper case !!")]
    public string xTwitter { get; set; } = "xTwitter";

    [AlphaNumericGenerator]
    public string NonAlphaNumeric { get; set; } = "K7eR3vP9_$%";

    [AlphaNumericGenerator]
    public string AlphaNumeric { get; set; } = "K7eR3vP9";

    [SpecialCharacterGenerator]
    public string SpecialCharacter { get; set; } = "@#$%^&*()";

    [SpecialCharacterGenerator]
    public string NonSpecialCharacter { get; set; } = "LOVE";



}


