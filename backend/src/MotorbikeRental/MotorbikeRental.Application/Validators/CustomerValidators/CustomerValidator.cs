using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Application.DTOs.Customers;
using MotorbikeRental.Application.Interface.IValidators.ICustomerValidators;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories;
using MotorbikeRental.Application.Exceptions;

namespace MotorbikeRental.Application.Validators.CustomerValidators
{
    public class CustomerValidator : ICustomerValidator
    {
        private readonly ICustomerRepository customerRepository;
        public CustomerValidator(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task<bool> ValidateForCreate(CustomerDto customerDto, CancellationToken cancellationToken = default)
        {
            if (await customerRepository.IsExists(nameof(Customer.IdNumber), customerDto.IdNumber, cancellationToken))
                throw new ValidatorException("Id number already exists.");
            if (await customerRepository.IsExists(nameof(Customer.PhoneNumber), customerDto.PhoneNumber, cancellationToken))
                throw new ValidatorException("Phone number already exists.");
            if (await customerRepository.IsExists(nameof(Customer.Email), customerDto.Email, cancellationToken))
                throw new ValidatorException("Email already exists.");
            return true;
        }

        public async Task<bool> ValidateForUpdate(CustomerDto customerDto, CancellationToken cancellationToken = default)
        {
            if (!await customerRepository.IsExists(nameof(Customer.CustomerId), customerDto.CustomerId, cancellationToken))
                throw new ValidatorException("Customer not found");
            if (await customerRepository.IsExistsForUpdate(customerDto.CustomerId, nameof(Customer.IdNumber), customerDto.IdNumber, nameof(Customer.CustomerId), cancellationToken))
                throw new ValidatorException("Id number already exists.");
            if (await customerRepository.IsExistsForUpdate(customerDto.CustomerId, nameof(Customer.PhoneNumber), customerDto.PhoneNumber, nameof(Customer.CustomerId), cancellationToken))
                throw new ValidatorException("Phone number already exists.");
            if (await customerRepository.IsExistsForUpdate(customerDto.CustomerId, nameof(Customer.Email), customerDto.Email, nameof(Customer.CustomerId), cancellationToken))
                throw new ValidatorException("Email already exists.");
            return true;
        }
    }
}