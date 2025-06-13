using System.ComponentModel.DataAnnotations; //make sure to include this namespace for data annotations
namespace Bts.Models.DTO
{
    public class ErrorObjectDTO
    {
        public int ErrorNumber { get; set; }
        public string? ErrorMessage { get; set; }
    }
}