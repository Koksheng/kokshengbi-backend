using FluentValidation;

namespace kokshengbi.Application.Charts.Queries.ListChartByPage
{
    public class ListChartByPageQueryValidator : AbstractValidator<ListChartByPageQuery>
    {
        public ListChartByPageQueryValidator()
        {
            //RuleFor(x => x.id).NotEmpty();

        }
    }
}
