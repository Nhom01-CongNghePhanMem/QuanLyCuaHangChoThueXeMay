using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Exceptions
{
    public class BusinessRuleException : BaseCustomException
    {
        public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
        public override string ErrorCode => "BUSINESS_RULE_VIOLATION";
        public BusinessRuleException() { }
        public BusinessRuleException(string message) : base(message) { }
    }
}