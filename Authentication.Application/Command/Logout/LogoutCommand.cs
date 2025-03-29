using Authentication.Application.Wrappers;
using Authentication.Domain.Dto;

namespace Authentication.Application.Command.Logout;

public sealed record LogoutCommand(string? RawUserId) : IRequestWrapper<AuthenticationResponseDto>;
