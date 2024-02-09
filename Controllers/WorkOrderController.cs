using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using BiancasBikes.Data;

namespace BiancasBikes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkOrderController : ControllerBase
{
  private BiancasBikesDbContext _dbContext;

  public WorkOrderController(BiancasBikesDbContext context)
  {
    _dbContext = context;
  }

  [HttpGet("incomplete")]
  [Authorize]
  public IActionResult GetIncompleteWorkOrders()
  {
    return Ok(_dbContext.WorkOrders
    .Include(wo => wo.Bike)
    .ThenInclude(b => b.Owner)
    .Include(wo => wo.Bike)
    .ThenInclude(b => b.BikeType)
    .Include(wo => wo.UserProfile)
    .Where(wo => wo.DateCompleted == null)
    .OrderBy(wo => wo.DateInitiated)
    .ThenByDescending(wo => wo.UserProfileId == null).ToList());
  }
}