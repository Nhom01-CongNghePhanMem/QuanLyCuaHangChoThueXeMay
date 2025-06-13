using AutoMapper;
using MotorbikeRental.Application.DTOs.Customers;
using MotorbikeRental.Application.Interface.IServices.ICustomerServices;
using MotorbikeRental.Application.Interface.IValidators.ICustomerValidators;
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
        public async Task<CustomerDto> CreateCustomer(CustomerDto customerDto)
        {
            await customerValidator.ValidateForCreate(customerDto);
            return mapper.Map<CustomerDto>(await customerRepository.GetByIdWithIncludes((await customerRepository.Create(mapper.Map<Customer>(customerDto))).CustomerId));
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            Customer customer = await customerRepository.GetById(id);
            if (customer == null)
                throw new Exception("Customer not found");
            await customerRepository.Delete(customer);
            return true;
        }

        public async Task<CustomerDto> GetCustomerById(int id)
        {
            return mapper.Map<CustomerDto>(await customerRepository.GetByIdWithIncludes(id));
        }
    }
}

