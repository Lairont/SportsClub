using FluentValidation;
using SportClub_Bancu.Domain.ModelsDb;

namespace SportClub_Bancu.Domain.Validators
{
    public class CategoryValidator : AbstractValidator<Categories>
    {
        public CategoryValidator()
        {
            RuleFor(category => category.Name)
                .NotEmpty().WithMessage("Название категории обязательно");

            RuleFor(category => category.Description)
                .NotEmpty().WithMessage("Описание категории обязательно");
        }
    }
}