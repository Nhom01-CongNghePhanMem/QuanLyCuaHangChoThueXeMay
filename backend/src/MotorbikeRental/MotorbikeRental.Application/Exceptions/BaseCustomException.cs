using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Exceptions
{
    public abstract class BaseCustomException : Exception
    {
        public abstract HttpStatusCode HttpStatusCode { get; }
        public abstract string ErrorCode { get; }
        public virtual string ErrorType => GetType().Name;
        protected BaseCustomException() { }
        protected BaseCustomException(string? message) : base(message) { }
        protected BaseCustomException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}