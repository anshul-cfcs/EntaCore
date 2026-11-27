using Application.DTOs.SuperAdmin;
using Application.Interfaces;
using Application.Interfaces.Database;
using Domain.CommonModels;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;

namespace Application.Services
{
    public class SuperAdminCompanyService : ISuperAdminCompanyService
    {
        private readonly ISuperAdminCompanyDbService _dbService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuthenticateDbService _authenticateDbService;
        private readonly IAccountMailService _accountMailService;
        public SuperAdminCompanyService(ISuperAdminCompanyDbService dbService, ICurrentUserService currentUserService, IAuthenticateDbService authenticateDbService, IAccountMailService accountMailService)
        {
            _dbService = dbService;
            _currentUserService = currentUserService;
            _authenticateDbService = authenticateDbService;
            _accountMailService = accountMailService;
        }
        public async Task<PagingMeta> GetCompanyListAsync(CompanyFilters filter)
        {
            filter.PageNo = Math.Max(filter.PageNo, 1);
            filter.PageSize = filter.PageSize > 0 ? filter.PageSize : 10;

            if (filter.Active is not (0 or 1 or 2)) throw new AppException("Invalid active filter.");

            var result = await _dbService.GetCompanyListAsync(filter);

            return new PagingMeta(new Meta(1, result.Items.Count > 0 ? "Success" : "Data Not Found"), new data(filter.PageNo, result.TotalCount, result.NextPageAvailable, result.Items));
        }

        public async Task<ResponseJson> CreateCompanyAsync(CompanyInsUpdtRequest request)
        {
            ValidateCommon(request);
            ValidateCreate(request);

            var createdByUserLoginId = _currentUserService.GetUserLoginId();
            if (createdByUserLoginId <= 0) throw new AppException("Invalid login user.");

            var companyId = await _dbService.CreateCompanyAsync(request, createdByUserLoginId);

            return new ResponseJson(new Meta(1, "Company created successfully"), new { CompanyID = companyId });
        }
        private static void ValidateCommon(CompanyInsUpdtRequest request)
        {
            if (request == null) throw new AppException("Request cannot be null.");

            if (request.Company == null) throw new AppException("Company information is required.");

            if (string.IsNullOrWhiteSpace(request.Company.CompanyName)) throw new AppException("Company name is required.");

            if (!string.IsNullOrWhiteSpace(request.Company.MailId) && !request.Company.MailId.Contains("@")) throw new AppException("Invalid company email.");

            if (request.Company.FinancialYearStartMonth is < 1 or > 12)
                throw new AppException("Financial year start month must be between 1 and 12.");

            if (request.Company.CompanyValidFrom.HasValue &&
                request.Company.CompanyValidUpto.HasValue &&
                request.Company.CompanyValidUpto.Value < request.Company.CompanyValidFrom.Value)
                throw new AppException("Company valid upto date must be greater than or equal to valid from date.");

            if (request.Subscription != null)
            {
                if (request.Subscription.SubscriptionPlanId <= 0) throw new AppException("Subscription plan is required.");

                if (request.Subscription.SubscriptionStartDate == default) throw new AppException("Subscription start date is required.");
            }
        }

        private static void ValidateCreate(CompanyInsUpdtRequest request)
        {
            if (request.Subscription == null) throw new AppException("Subscription information is required.");
        }
    }
}
