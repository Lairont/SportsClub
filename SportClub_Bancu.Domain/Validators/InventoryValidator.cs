using FluentValidation;
using SportClub_Bancu.Domain.ModelsDb;

namespace SportClub_Bancu.Domain.Validators
{
    public class InventoryValidator : AbstractValidator<Inventory>
    {
        public InventoryValidator()
        {
            RuleFor(inventory => inventory.Name)
                .NotEmpty().WithMessage("Название инвентаря обязательно");

            RuleFor(inventory => inventory.Notes)
                .NotEmpty().WithMessage("Примечание обязательно");

            RuleFor(inventory => inventory.Count)
                .GreaterThan(0).WithMessage("Количество должно быть больше нуля");

            RuleFor(inventory => inventory.Condition)
                .IsInEnum()
                .WithMessage("Состояние должно быть допустимым значением");

            RuleFor(inventory => inventory.InventoryNumber)
                .NotEmpty().WithMessage("Номер инвентаря обязателен");

            RuleFor(inventory => inventory.Price)
                .GreaterThan(0).WithMessage("Цена должна быть больше нуля");

            RuleFor(inventory => inventory.PurchaseDate)
                .NotEmpty().WithMessage("Дата покупки обязательна");

            RuleFor(inventory => inventory.WarrantyUntil)
                .NotEmpty().WithMessage("Дата окончания гарантии обязательна")
                .GreaterThanOrEqualTo(inventory => inventory.PurchaseDate)
                .WithMessage("Дата окончания гарантии не может быть раньше даты покупки");

            RuleFor(inventory => inventory.CategoryId)
                .NotEmpty().WithMessage("Категория обязательна");
        }
    }
}