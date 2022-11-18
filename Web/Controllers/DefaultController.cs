using AutoMapper;
using Core;
using Data;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Services;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User,Administrator")]
public abstract class DefaultController : ControllerBase
{
    public DefaultController()
    {
    }
    
    protected async Task<string> ValidateEntity<T>(T ToValidate) //string empty on success
    {
        var validator = HttpContext.RequestServices.GetService<IValidator<T>>();
        if(validator == null)
        {
            return "Could not validate the data";
        }
        var res = await validator.ValidateAsync(ToValidate);
        var message = string.Empty;

        if(!res.IsValid)
        {
            message = $"{res.Errors.First().ErrorMessage}";
        }
        return message;
    }
}