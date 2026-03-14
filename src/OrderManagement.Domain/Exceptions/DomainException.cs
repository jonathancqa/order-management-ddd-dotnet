using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)  { }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message) { }
}