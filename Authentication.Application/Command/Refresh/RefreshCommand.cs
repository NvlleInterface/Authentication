using Authentication.Application.Wrappers;
using Authentication.Domain.Dto;

namespace Authentication.Application.Command.Refresh;

public sealed record RefreshCommand(string RefreshToken) : IRequestWrapper<AuthenticationResponseDto>;
