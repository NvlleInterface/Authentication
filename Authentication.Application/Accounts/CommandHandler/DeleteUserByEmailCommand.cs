using Authentication.Application.Wrappers;
using Authentication.Domain.Dto;


namespace Authentication.Application.Accounts.CommandHandler;

public record DeleteUserByEmailCommand(string Email) : IRequestWrapper<ErrorResponsesDto>;

