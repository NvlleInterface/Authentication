using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Authentication.Application.Wrappers;
using Authentication.Domain.Dto;
using Authentication.Domain;

namespace Authentication.Application.Command.Register;

public sealed class RegisterHandle : IHandlerWrapper<RegisterCommand, ErrorResponsesDto>
{
    public readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userRepository;

    public RegisterHandle(UserManager<IdentityUser> userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Response<ErrorResponsesDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsConfirmPassword())
        {
            return Response.Fail<ErrorResponsesDto>("Password doesn't match with confirm password", StatusCodes.Status400BadRequest, "Bad request");
        }

        var registrationUser = _mapper.Map<IdentityUser>(request);

        var result = await _userRepository.CreateAsync(registrationUser, request.Password).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            var errorDescriber = new IdentityErrorDescriber();
            var primaryError = result.Errors.FirstOrDefault();

            if (primaryError?.Code == nameof(errorDescriber.DuplicateEmail))
            {
                return Response.Fail<ErrorResponsesDto>("Email already exists.", StatusCodes.Status409Conflict, "Conflict");
            }
            else if (primaryError?.Code == nameof(errorDescriber.DuplicateUserName))
            {
                return Response.Fail<ErrorResponsesDto>("username already exists.", StatusCodes.Status409Conflict, "Conflict");
            }
        }
        var user = await _userRepository.FindByEmailAsync(request.Email);
        await _userRepository.AddToRoleAsync(user!, "USER");

        return Response.Ok<ErrorResponsesDto>(null!, "User registed successful", StatusCodes.Status201Created, "Created");
    }

    
}
