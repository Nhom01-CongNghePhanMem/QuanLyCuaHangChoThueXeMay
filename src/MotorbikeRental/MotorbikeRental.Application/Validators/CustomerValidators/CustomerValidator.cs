using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Application.DTOs.Customers;
using MotorbikeRental.Application.Interface.IValidators.ICustomerValidators;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories;

namespace MotorbikeRental.Application.Validators.CustomerValidators
{
    public class CustomerValidator : ICustomerValidator
    {
        private readonly ICustomerRepository customerRepository;
        public CustomerValidator(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task<bool> ValidateForCreate(CustomerDto customerDto)
        {
            if (await customerRepository.IsExists(nameof(Customer.IdNumber), customerDto.IdNumber))
                throw new Exception("Id number already exists.");
            if (await customerRepository.IsExists(nameof(Customer.PhoneNumber), customerDto.PhoneNumber))
                throw new Exception("Phone number already exists.");
            if (await customerRepository.IsExists(nameof(Customer.Email), customerDto.Email))
                throw new Exception("Email already exists.");
            return true;
        }

        public async Task<bool> ValidateForUpdate(CustomerDto customerDto)
        {
            if (!await customerRepository.IsExists(nameof(Customer.CustomerId), customerDto.CustomerId))
                throw new Exception("Customer not found");
            if (await customerRepository.IsExistsForUpdate(customerDto.CustomerId, nameof(Customer.IdNumber), customerDto.IdNumber, nameof(Customer.CustomerId)))
                throw new Exception("Id number already exists.");
            if (await customerRepository.IsExistsForUpdate(customerDto.CustomerId, nameof(Customer.PhoneNumber), customerDto.PhoneNumber, nameof(Customer.CustomerId)))
                throw new Exception("Phone number already exists.");
            if (await customerRepository.IsExistsForUpdate(customerDto.CustomerId, nameof(Customer.Email), customerDto.Email, nameof(Customer.CustomerId)))
                throw new Exception("Email already exists.");
            return true;
        }
    }
}