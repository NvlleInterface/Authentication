
using Authentication.Domain;
using MediatR;

namespace Authentication.Application.Wrappers;

public interface IRequestWrapper<T> : IRequest<Response<T>> { }