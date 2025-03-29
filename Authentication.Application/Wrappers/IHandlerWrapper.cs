
using Authentication.Domain;
using MediatR;

namespace Authentication.Application.Wrappers;

public interface IHandlerWrapper<TIn, TOut> : IRequestHandler<TIn, Response<TOut>>
    where TIn : IRequestWrapper<TOut>
{ }
