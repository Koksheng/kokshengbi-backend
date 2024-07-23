using FluentValidation;

namespace kokshengbi.Application.Charts.Commands.CreateChart
{
    public class CreateChartCommandValidator : AbstractValidator<CreateChartCommand>
    {
        public CreateChartCommandValidator()
        {
            RuleFor(x => x.chartName)
                .NotEmpty();
        }
    }
}
