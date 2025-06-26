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
            List<string> errors = new List<string>();
            if (await customerRepository.IsExists(nameof(Customer.IdNumber), customerDto.IdNumber, cancellationToken))
                errors.Add("Id number already exists");
            if (await customerRepository.IsExists(nameof(Customer.PhoneNumber), customerDto.PhoneNumber, cancellationToken))
                errors.Add("Phone number already exists");
            if (await customerRepository.IsExists(nameof(Customer.Email), customerDto.Email, cancellationToken))
                errors.Add("Email already exists");
            if (errors.Any())
                throw new ValidatorException(string.Join("; ", errors));
            return true;
        }

        public async Task<bool> ValidateForUpdate(CustomerDto customerDto, CancellationToken cancellationToken = default)
        {
            List<string> errors = new List<string>();
            if (!await customerRepository.IsExists(nameof(Customer.CustomerId), customerDto.CustomerId, cancellationToken))
                throw new NotFoundException("Customer not found");
            if (await customerRepository.IsExistsForUpdate(customerDto.CustomerId, nameof(Customer.IdNumber), customerDto.IdNumber, nameof(Customer.CustomerId), cancellationToken))
                errors.Add("Id number already exists");
            if (await customerRepository.IsExistsForUpdate(customerDto.CustomerId, nameof(Customer.PhoneNumber), customerDto.PhoneNumber, nameof(Customer.CustomerId), cancellationToken))
                errors.Add("Phone number already exists");
            if (await customerRepository.IsExistsForUpdate(customerDto.CustomerId, nameof(Customer.Email), customerDto.Email, nameof(Customer.CustomerId), cancellationToken))
                errors.Add("Email already exists");
            if (errors.Any())
                throw new ValidatorException(string.Join("; ", errors));
            return true;
        }
    }
}