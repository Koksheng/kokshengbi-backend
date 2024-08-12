using FluentValidation;

namespace kokshengbi.Application.Charts.Commands.GenChartByAiAsyncMq
{
    public class GenChartByAiAsyncMqCommandValidator : AbstractValidator<GenChartByAiAsyncMqCommand>
    {
        public GenChartByAiAsyncMqCommandValidator()
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
