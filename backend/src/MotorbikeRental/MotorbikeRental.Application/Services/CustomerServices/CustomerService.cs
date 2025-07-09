using AutoMapper;
using MotorbikeRental.Application.DTOs.Customers;
using MotorbikeRental.Application.Interface.IServices.ICustomerServices;
using MotorbikeRental.Application.Interface.IValidators.ICustomerValidators;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories;
using System.Threading.Tasks;
using MotorbikeRental.Application.DTOs.Pagination;

namespace MotorbikeRental.Application.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ICustomerValidator customerValidator;
        private readonly IMapper mapper;
        public CustomerService(ICustomerRepository customerRepository, ICustomerValidator customerValidator, IMapper mapper)
        {
            this.customerRepository = customerRepository;
            this.customerValidator = customerValidator;
            this.mapper = mapper;
        }
        public async Task<CustomerDto> CreateCustomer(CustomerCreateDto customerCreateDto, CancellationToken cancellationToken = default)
        {
            await customerValidator.ValidateForCreate(customerCreateDto);
            return mapper.Map<CustomerDto>(await customerRepository.Create(mapper.Map<Customer>(customerCreateDto), cancellationToken));
        }
        public async Task<CustomerDto> GetCustomerById(int id, CancellationToken cancellationToken = default)
        {
            Customer? customer = await customerRepository.GetByIdWithIncludes(id, cancellationToken);
            if (customer == null)
                throw new NotFoundException($"Customer with id {id} not found");
            customer.CreateAt = DateTime.UtcNow;
            CustomerDto customerDto = mapper.Map<CustomerDto>(customer);
            return customerDto;
        }
        public async Task<PaginatedDataDto<CustomerListDto>> GetCustomerByFilter(CustomerFilterDto filterDto, CancellationToken cancellationToken = default)
        {
            (IEnumerable<Customer> customer, int totalCount) = await customerRepository.GetFilterData(filterDto.Search, filterDto.PageNumber, filterDto.PageSize, cancellationToken);
            return new PaginatedDataDto<CustomerListDto>(mapper.Map<IEnumerable<CustomerListDto>>(customer), totalCount);
        }
        public async Task<CustomerDto> UpdateCustomer(CustomerUpdateDto customerUpdateDto, CancellationToken cancellationToken = default)
        {
            await customerValidator.ValidateForUpdate(customerUpdateDto, cancellationToken);
            Customer customer = await customerRepository.GetByIdWithIncludes(customerUpdateDto.CustomerId, cancellationToken);
            mapper.Map(customerUpdateDto, customer);
            await customerRepository.Update(customer, cancellationToken);
            return mapper.Map<CustomerDto>(customer);
        }
        public async Task<bool> DeleteCustomer(int id, CancellationToken cancellationToken = default)
        {
            Customer customer = await customerRepository.GetById(id, cancellationToken);
            if (customer == null)
                throw new NotFoundException("Customer not found");
            await customerRepository.Delete(customer, cancellationToken);
            return true;
        }
    }
}

