using FluentValidation;

namespace kokshengbi.Application.Users.Queries.ListUserByPage
{
    public class ListUserByPageQueryValidator : AbstractValidator<ListUserByPageQuery>
    {
        public ListUserByPageQueryValidator()
        {
            //RuleFor(x => x.id).NotEmpty();
        }
    }
}
