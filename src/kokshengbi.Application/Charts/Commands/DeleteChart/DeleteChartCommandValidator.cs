using FluentValidation;

namespace kokshengbi.Application.Charts.Commands.DeleteChart
{
    public class DeleteChartCommandValidator : AbstractValidator<DeleteChartCommand>
    {
        public DeleteChartCommandValidator()
        {
            RuleFor(x => x.id).NotEmpty();

        }
    }
}
