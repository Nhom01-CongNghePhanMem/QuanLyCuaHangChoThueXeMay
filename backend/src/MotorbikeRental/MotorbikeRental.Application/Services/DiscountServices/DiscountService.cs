using AutoMapper;
using MotorbikeRental.Application.DTOs.Discount;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IServices.IDiscountServices;
using MotorbikeRental.Application.Interface.IValidators.IDiscountValidators;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Interfaces.IRepositories.IPricingRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Services.DiscountServices
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository discountRepository;
        private readonly IDiscountValidator discountValidator;
        private readonly IMapper mapper;
        public DiscountService(IDiscountRepository discountRepository, IDiscountValidator discountValidator, IMapper mapper)
        {
            this.discountRepository = discountRepository;
            this.discountValidator = discountValidator;
            this.mapper = mapper;
        }
        public async Task<DiscountDto> CreateDiscount(DiscountCreateDto discountCreateDto)
        {
            (bool isValid, List<string> categoryName) = await discountValidator.ValidateForCreate(discountCreateDto);
            Discount discount = mapper.Map<Discount>(discountCreateDto);
            List<Discount_Category> discountCategories = new List<Discount_Category>();
            for (int i = 0; i < discountCreateDto.CategoryId.Count; i++)
            {
                discountCategories.Add(new Discount_Category
                {
                    CategoryId = discountCreateDto.CategoryId[i],
                });
            }
            discount.Categories = discountCategories;
            discount.CreatedAt = DateTime.UtcNow;
            await discountRepository.Create(discount);
            DiscountDto discountDto = mapper.Map<DiscountDto>(discount);
            discountDto.CategoryNames = categoryName;
            return discountDto;
        }
        public async Task<PaginatedDataDto<DiscountDto>> GetDiscountsByFilter(DiscountFilterDto filter, CancellationToken cancellationToken = default)
        {
            (IEnumerable<Discount> discount, int totalCount) = await discountRepository.GetFilterData(filter.Search, filter.PageNumber, filter.PageSize, filter.FilterStartDate, filter.FilterEndDate,filter.IsActive, cancellationToken);
            return new PaginatedDataDto<DiscountDto>(mapper.Map<IEnumerable<DiscountDto>>(discount), totalCount);
        }
        public async Task<DiscountDto> GetDiscountById(int id, CancellationToken cancellationToken = default)
        {
            return mapper.Map<DiscountDto>(await discountRepository.GetDiscountById(id,false, cancellationToken));
        }
        public async Task<DiscountDto> UpdateDiscount(DiscountUpdateDto discountUpdateDto, CancellationToken cancellationToken = default)
        {
            (bool isValid, List<string> categoryNames) = await discountValidator.ValidateForUpdate(discountUpdateDto, cancellationToken);
            Discount discount = await discountRepository.GetDiscountById(discountUpdateDto.DiscountId,true, cancellationToken) ?? throw new NotFoundException($"Discount with id {discountUpdateDto.DiscountId} not found");
            mapper.Map(discountUpdateDto, discount);
            discount.Categories.Clear();
            for (int i = 0; i < discountUpdateDto.CategoryId.Count; i++)
            {
                discount.Categories.Add(new Discount_Category
                {
                    CategoryId = discountUpdateDto.CategoryId[i],
                });
            }
            await discountRepository.Update(discount, cancellationToken);
            DiscountDto discountDto = mapper.Map<DiscountDto>(discount);
            discountDto.CategoryNames = categoryNames;
            return discountDto;
        }
        public async Task<bool> DeleteDiscount(int id, CancellationToken cancellationToken = default)
        {
            Discount? discount = await discountRepository.GetById(id, cancellationToken) ?? throw new NotFoundException($"Discount with id {id} not found");
            await discountRepository.Delete(discount, cancellationToken);
            return true;
        }
    }
}
