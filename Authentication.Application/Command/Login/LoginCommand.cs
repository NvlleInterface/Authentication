using Authentication.Application.Wrappers;
using Authentication.Domain.Dto;

namespace Authentication.Application.Command.Login;

public sealed record LoginCommand(string Email, string Password) : IRequestWrapper<AuthenticationResponseDto>;

