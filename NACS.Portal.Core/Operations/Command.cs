using MediatR;

namespace NACS.Portal.Core.Operations;

public interface ICommand<out TResult> : IRequest<TResult> { }
