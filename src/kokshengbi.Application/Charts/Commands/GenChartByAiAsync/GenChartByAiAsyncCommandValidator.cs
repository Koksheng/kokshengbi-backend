using FluentValidation;

namespace kokshengbi.Application.Charts.Commands.GenChartByAiAsync
{
    public class GenChartByAiAsyncCommandValidator : AbstractValidator<GenChartByAiAsyncCommand>
    {
        public GenChartByAiAsyncCommandValidator()
        {
            RuleFor(x => x.goal)
                .NotEmpty();
            RuleFor(x => x.chartName)
                .NotEmpty()
                .MaximumLength(200).WithMessage("Chart Name too long.");
            RuleFor(x => x.chartType)
                .NotEmpty();

        }
    }
}
