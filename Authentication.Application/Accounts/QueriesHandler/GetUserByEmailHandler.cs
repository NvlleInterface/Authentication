using Authentication.Application.Wrappers;
using Authentication.Domain;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Authentication.Application.Accounts.QueriesHandler;

public class GetUserByEmailHandler : IHandlerWrapper<GetUserByEmailQuery, IdentityUser>
{
    private readonly UserManager<IdentityUser> _userManager;
    public readonly IMapper _mapper;
    public GetUserByEmailHandler(UserManager<IdentityUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    public async Task<Response<IdentityUser>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (users == null)
        {
            return Response.Fail<IdentityUser>($"User not found: {request.Email}", StatusCodes.Status404NotFound, "NotFound");
        }
        return Response.Ok<IdentityUser>(users, "", StatusCodes.Status200OK,"Ok");
    }
}