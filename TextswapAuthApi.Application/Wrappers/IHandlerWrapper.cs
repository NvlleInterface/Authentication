
using MediatR;
using TextswapAuthApi.Domaine.Models;

namespace TextswapAuthApi.Application.Wrappers;

public interface IHandlerWrapper<TIn, TOut> : IRequestHandler<TIn, Response<TOut>>
    where TIn : IRequestWrapper<TOut>
{ }
