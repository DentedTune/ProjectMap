namespace ProjectMap.WebApi.Models;

public class Object2D
{
    public Guid Id { get; set; }

    public string ObjectType { get; set; }

    public int PositionX { get; set; }

    public int PositionY { get; set; }

    public int Width { get; set; }

    public int Length { get; set; }

    public int Direction { get; set; }

    public float RotationZ { get; set; }

    public int SortingLayer { get; set; }
}