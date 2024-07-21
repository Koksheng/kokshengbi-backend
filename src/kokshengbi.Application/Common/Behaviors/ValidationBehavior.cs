using FluentValidation;
using kokshengbi.Application.Common.Constants;
using kokshengbi.Application.Common.Exceptions;
using MediatR;

namespace kokshengbi.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
        //where TResponse : BaseResponse<object>
    {
        private readonly IValidator<TRequest>? _validator;
        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }
        /*
         * when go to here return await _mediator.Send(abc);
         * the code execute Validation first before go to CommandHandler or QueryHandler
         * 
         * ***** before the handler *****
         * var result = await next(); ***** here will go CommandHandler or QueryHandler
         * ***** after the handler *****
         */

        // When 
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validator is null)
            {
                return await next();
            }

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid)
            {
                return await next();
            }

            var errorMessages = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));

            throw new BusinessException(ErrorCode.PARAMS_ERROR, errorMessages);
        }
    }
}
