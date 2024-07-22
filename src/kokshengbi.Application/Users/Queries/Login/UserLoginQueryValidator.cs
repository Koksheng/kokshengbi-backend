using FluentValidation;

namespace kokshengbi.Application.Users.Queries.Login
{
    public class UserLoginQueryValidator : AbstractValidator<UserLoginQuery>
    {
        public UserLoginQueryValidator()
        {
            RuleFor(x => x.userAccount)
                .NotEmpty()
                .MinimumLength(4).WithMessage("User account must be at least 4 characters long.")
                .Matches("^[a-zA-Z0-9_]*$").WithMessage("User account can only contain letters, numbers, and underscores.");
            RuleFor(x => x.userPassword).NotEmpty().MinimumLength(8).WithMessage("User password must be at least 8 characters long.");
        }
    }
}
