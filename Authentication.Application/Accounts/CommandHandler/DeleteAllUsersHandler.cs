using Authentication.Application.Wrappers;
using Authentication.Domain;
using Authentication.Domain.Dto;

namespace Authentication.Application.Accounts.CommandHandler;

public class DeleteAllUsersHandler : IHandlerWrapper<DeleteAllUsersCommand, ErrorResponsesDto>
{
    public Task<Response<ErrorResponsesDto>> Handle(DeleteAllUsersCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
