

using Authentication.Application.Wrappers;
using Authentication.Domain.Dto;
using Authentication.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Application.Accounts.CommandHandler;

public class DeleteUserByEmailHandler : IHandlerWrapper<DeleteUserByEmailCommand, ErrorResponsesDto>
{
    private readonly UserManager<IdentityUser> _userRepository;
    public DeleteUserByEmailHandler(UserManager<IdentityUser> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Response<ErrorResponsesDto>> Handle(DeleteUserByEmailCommand request, CancellationToken cancellationToken)
    {
        var usersToDelete = await _userRepository.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (usersToDelete == null)
        {
            return Response.Fail<ErrorResponsesDto>($"Users not found: ", StatusCodes.Status404NotFound," NotFound");
        }
        var result = await _userRepository.DeleteAsync(usersToDelete!);
        if (!result.Succeeded)
        {
            return Response.Fail<ErrorResponsesDto>($"fail to delete role: {request.Email}", StatusCodes.Status401Unauthorized, "Unauthorized");
        }

        return Response.Ok<ErrorResponsesDto>(null, "deleted", StatusCodes.Status201Created,"NotContent");
    }
}
