using ECommerce.API.DTOs.Cart;
using FluentValidation;

namespace ECommerce.API.Validators;

public class CartItemRequestValidator : AbstractValidator<CartItemRequest>
{
    public CartItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be at least 1.");
    }
}
