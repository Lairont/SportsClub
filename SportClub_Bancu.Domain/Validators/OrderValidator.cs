using FluentValidation;
using SportClub_Bancu.Domain.ModelsDb;

namespace SportClub_Bancu.Domain.Validators
{
    public class OrderValidator : AbstractValidator<Orders>
    {
        public OrderValidator()
        {
            RuleFor(order => order.UserId)
                .NotEmpty().WithMessage("ID пользователя обязателен");

            RuleFor(order => order.InventoryId)
                .NotEmpty().WithMessage("ID инвентаря обязателен");

            RuleFor(order => order.Quantity)
                .GreaterThan(0).WithMessage("Количество должно быть больше нуля");

            RuleFor(order => order.Status)
                .IsInEnum().WithMessage("Статус должен быть допустимым значением");

            RuleFor(order => order.IssuedAt)
                .NotEmpty().WithMessage("Дата выдачи обязательна");

            RuleFor(order => order.DueDate)
                .NotEmpty().WithMessage("Дата возврата обязательна")
                .GreaterThanOrEqualTo(order => order.IssuedAt)
                .WithMessage("Дата возврата не может быть раньше даты выдачи");

            RuleFor(order => order.ReturnedAt)
                .GreaterThanOrEqualTo(order => order.IssuedAt)
                .When(order => order.ReturnedAt.HasValue)
                .WithMessage("Дата возврата не может быть раньше даты выдачи");
        }
    }
}