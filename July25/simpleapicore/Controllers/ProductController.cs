using Microsoft.AspNetCore.Mvc;
using SimpleApi.Services;
using SimpleApi.Models.DTOS;

namespace SimpleApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service) => _service = service;

    [HttpGet]
    public ActionResult<IEnumerable<ProductDto>> GetAll() =>
        Ok(_service.GetAll());

    [HttpGet("{id}")]
    public ActionResult<ProductDto> GetById(int id)
    {
        var product = _service.GetById(id);
        return product == null ? NotFound() : Ok(product);
    }
}
