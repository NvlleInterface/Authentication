using Authentication.Application.Wrappers;
using Authentication.Domain;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Authentication.Application.Accounts.QueriesHandler;

public class GetAllUsersHandler : IHandlerWrapper<GetAllUsersQuery, IEnumerable<IdentityUser>>
{
    private readonly UserManager<IdentityUser> _userManager;
    public readonly IMapper _mapper;
    public GetAllUsersHandler(UserManager<IdentityUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    public async Task<Response<IEnumerable<IdentityUser>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.ToListAsync(cancellationToken: cancellationToken);
        if (users == null)
        {
            return Response.Fail<IEnumerable<IdentityUser>>($"Users not found: ", StatusCodes.Status404NotFound, " NotFound");
        }
        return Response.Ok<IEnumerable<IdentityUser>>(users, "", StatusCodes.Status200OK, "Ok");
    }
}
