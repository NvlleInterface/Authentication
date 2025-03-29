using Authentication.Application.Wrappers;
using Authentication.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Application.Accounts.QueriesHandler;

public record GetAllUsersQuery : IRequestWrapper<IEnumerable<IdentityUser>>
{
}