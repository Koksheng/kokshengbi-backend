using FluentValidation;

namespace kokshengbi.Application.Charts.Commands.GenChartByAi
{
    public class GenChartByAiCommandValidator : AbstractValidator<GenChartByAiCommand>
    {
        public GenChartByAiCommandValidator()
        {
            RuleFor(x => x.goal)
                .NotEmpty();
            RuleFor(x => x.chartName)
                .NotEmpty()
                .MaximumLength(4).WithMessage("Chart Name too long.");
            RuleFor(x => x.chartType)
                .NotEmpty();

        }
    }
}
