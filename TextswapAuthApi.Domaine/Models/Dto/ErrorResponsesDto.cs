
namespace TextswapAuthApi.Domaine.Models.Dto;

public class ErrorResponsesDto
{
    public IEnumerable<string> ErrorMessages { get; }

    public ErrorResponsesDto(string errorMessage) : this(new List<string>() { errorMessage }) { }

    public ErrorResponsesDto(IEnumerable<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }
}

