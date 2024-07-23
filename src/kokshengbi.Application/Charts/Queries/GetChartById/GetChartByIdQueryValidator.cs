using FluentValidation;

namespace kokshengbi.Application.Charts.Queries.GetChartById
{
    public class GetChartByIdQueryValidator : AbstractValidator<GetChartByIdQuery>
    {
        public GetChartByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

        }
    }
}
