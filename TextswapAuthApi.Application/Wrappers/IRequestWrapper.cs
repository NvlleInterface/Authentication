
using MediatR;
using TextswapAuthApi.Domaine.Models;

namespace TextswapAuthApi.Application.Wrappers;

public interface IRequestWrapper<T> : IRequest<Response<T>> { }