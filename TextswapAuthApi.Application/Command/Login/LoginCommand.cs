using TextswapAuthApi.Application.Wrappers;
using TextswapAuthApi.Domaine.Models.Dto;

namespace TextswapAuthApi.Application.Command.Login;

public sealed record LoginCommand(string Email, string Password) : IRequestWrapper<TextswapAuthApiUserResponseDto>;

