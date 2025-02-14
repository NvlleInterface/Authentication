using TextswapAuthApi.Application.Wrappers;
using TextswapAuthApi.Domaine.Models.Dto;

namespace TextswapAuthApi.Application.Command.Logout;

public sealed record LogoutCommand(string? RawUserId) : IRequestWrapper<TextswapAuthApiUserResponseDto>;
