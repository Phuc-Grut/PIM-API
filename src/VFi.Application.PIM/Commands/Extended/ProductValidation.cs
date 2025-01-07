using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using static VFi.Application.PIM.Commands.ProductSizeEditCommand;

namespace VFi.Application.PIM.Commands.Validations
{
     
    public class ProductAddFromLinkCommandValidation : AbstractValidator<ProductAddFromLinkCommand>
    {

        protected readonly IProductRepository _context;
        private Guid Id;
        public ProductAddFromLinkCommandValidation(IProductRepository context, Guid id)
        {
            _context = context;
            Id = id;
        }
         
    }

}
