using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Exceptions
{
    public class NotFoundException : BaseCustomException
    {
        public override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;
        public override string ErrorCode => "NOT_FOUND";
        public NotFoundException() { }
        public NotFoundException(string? message) : base(message) { }
        public NotFoundException(string message, Exception? innerException) : base(message, innerException) { }
    }
}