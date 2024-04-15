namespace APBD06.DTOs;

public record GetAllAnimalsResponse(int Id, string Name, string? Description, string Category, string Area);
public record CreateAnimalRequest(string Name, string? Description, string Category, string Area);