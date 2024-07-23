using FluentValidation;

namespace kokshengbi.Application.Charts.Commands.UpdateChart
{
    public class UpdateChartCommandValidator : AbstractValidator<UpdateChartCommand>
    {
        public UpdateChartCommandValidator()
        {
            RuleFor(x => x.id).NotEmpty();

        }
    }
}
