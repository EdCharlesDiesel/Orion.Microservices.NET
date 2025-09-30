namespace Orion.Domain.DTOs
{
    public interface IShipperFullEditDto
    {
        int Id { get; set; }         

        string CompanyName { get; }

        string Phone { get; set; }

        int OrderId { get; set; }

    }
}