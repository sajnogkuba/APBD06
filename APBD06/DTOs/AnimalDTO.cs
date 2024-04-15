namespace APBD06.DTOs;

public record GetAllAnimalsResponse(int Id, string Name, string? Description, string Category, string Area);