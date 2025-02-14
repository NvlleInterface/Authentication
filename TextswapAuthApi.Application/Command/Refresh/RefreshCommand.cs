using TextswapAuthApi.Application.Wrappers;
using TextswapAuthApi.Domaine.Models.Dto;

namespace TextswapAuthApi.Application.Command.Refresh;

public sealed record RefreshCommand(string RefreshToken) : IRequestWrapper<TextswapAuthApiUserResponseDto>;
