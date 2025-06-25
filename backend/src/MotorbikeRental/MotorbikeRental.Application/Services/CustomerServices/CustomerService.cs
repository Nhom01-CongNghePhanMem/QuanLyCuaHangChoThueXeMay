using AutoMapper;
using MotorbikeRental.Application.DTOs.Customers;
using MotorbikeRental.Application.Interface.IServices.ICustomerServices;
using MotorbikeRental.Application.Interface.IValidators.ICustomerValidators;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories;

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
        public async Task<CustomerDto> CreateCustomer(CustomerDto customerDto, CancellationToken cancellationToken = default)
        {
            await customerValidator.ValidateForCreate(customerDto);
            return mapper.Map<CustomerDto>(await customerRepository.GetByIdWithIncludes((await customerRepository.Create(mapper.Map<Customer>(customerDto), cancellationToken)).CustomerId, cancellationToken));
        }

        public async Task<bool> DeleteCustomer(int id, CancellationToken cancellationToken = default)
        {
            Customer customer = await customerRepository.GetById(id, cancellationToken);
            if (customer == null)
                throw new NotFoundException("Customer not found");
            await customerRepository.Delete(customer, cancellationToken);
            return true;
        }

        public async Task<CustomerDto> GetCustomerById(int id, CancellationToken cancellationToken = default)
        {
            return mapper.Map<CustomerDto>(await customerRepository.GetByIdWithIncludes(id, cancellationToken));
        }
    }
}

