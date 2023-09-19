using System;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Infra.CrossCutting.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public static class FillBookVMAdditionalInfos
{
	public static List<BookViewModel> FillBookVMModel(List<BookViewModel> List)
	{
        return List;
	}
    public static BookViewModel FillBookVMList(BookViewModel bookVM)
    {
        return bookVM;
    }
}
