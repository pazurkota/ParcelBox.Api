using Microsoft.AspNetCore.Mvc;
using ParcelBox.Api.Abstraction;
using ParcelBox.Api.Repositories;

namespace ParcelBox.Api.Controllers;

public class LockerBoxController(IRepository<LockerRepository> repository, ILogger<LockerBoxController> logger) : 
    BaseController
{
    
}