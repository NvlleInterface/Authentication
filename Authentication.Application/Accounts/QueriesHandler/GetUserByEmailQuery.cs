using Authentication.Application.Wrappers;
using Microsoft.AspNetCore.Identity;


namespace Authentication.Application.Accounts.QueriesHandler;


public sealed record GetUserByEmailQuery(string Email) : IRequestWrapper<IdentityUser>
{
}