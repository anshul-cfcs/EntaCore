using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class ErrorLogMaster
{
    public long ErrorLogId { get; set; }

    public int? CompanyId { get; set; }

    public long? UserLoginId { get; set; }

    public Guid? EmployeeId { get; set; }

    public string? UserName { get; set; }

    public string? RequestPath { get; set; }

    public string? RequestMethod { get; set; }

    public string? QueryString { get; set; }

    public string? ControllerName { get; set; }

    public string? ActionName { get; set; }

    public string? ServiceName { get; set; }

    public string? MethodName { get; set; }

    public string? ExceptionSource { get; set; }

    public string? ExceptionType { get; set; }

    public string? ExceptionMessage { get; set; }

    public string? InnerExceptionMessage { get; set; }

    public string? StackTrace { get; set; }

    public string? TraceId { get; set; }

    public int StatusCode { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }
}
