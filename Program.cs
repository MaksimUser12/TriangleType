using System;
using System.Collections.Generic;
using System.IO;
using Serilog;
using Serilog.Events;

public class TriangleCalculator
{
    private static readonly ILogger Logger = new LoggerConfiguration()
        .WriteTo.File("triangle_log.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

    public static (string, List<(int, int)>) CalculateTriangleTypeAndCoordinates(string sideA, string sideB, string sideC)
    {
        List<(int, int)> coordinates = new List<(int, int)>();
        string triangleType = "";

        try
        {
            float a = float.Parse(sideA);
            float b = float.Parse(sideB);
            float c = float.Parse(sideC);

            // Check if the input represents a valid triangle
            if (a + b > c && a + c > b && b + c > a)
            {
                if (a == b && b == c)
                {
                    triangleType = "равносторонний"; 
                }
                else if (a == b || b == c || a == c)
                {
                    triangleType = "равнобедренный"; 
                }
                else
                {
                    triangleType = "разносторонний";
                }

                // Calculate scaled coordinates for a 100x100 px field
                coordinates.Add(((int)(a * 100), 50)); // Centering horizontally
                coordinates.Add(((int)(c * 100), 0));  // Vertex at the top
                coordinates.Add(((int)(b * 100), 100)); // Vertex at the bottom
            }
            else
            {
                triangleType = "не треугольник"; // Not a triangle
                coordinates.Add((-1, -1));
                coordinates.Add((-1, -1));
                coordinates.Add((-1, -1));
            }
        }
        catch (FormatException)
        {
            // Handle invalid input
            triangleType = "";
            coordinates.Add((-2, -2));
            coordinates.Add((-2, -2));
            coordinates.Add((-2, -2));
        }

        LogRequest(DateTime.Now, sideA, sideB, sideC, triangleType, coordinates);
        return (triangleType, coordinates);
    }

    public static void LogRequest(DateTime timestamp, string sideA, string sideB, string sideC, string triangleType, List<(int, int)> coordinates)
    {
        Logger.Information("Timestamp: {Timestamp}", timestamp);
        Logger.Information("Input Sides: A={SideA}, B={SideB}, C={SideC}", sideA, sideB, sideC);
        Logger.Information("Triangle Type: {TriangleType}", triangleType);
        Logger.Information("Coordinates: A({A1}, {A2}), B({B1}, {B2}), C({C1}, {C2})",
            coordinates[0].Item1, coordinates[0].Item2, coordinates[1].Item1, coordinates[1].Item2, coordinates[2].Item1, coordinates[2].Item2);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter side A length:");
        string sideA = Console.ReadLine();
        Console.WriteLine("Enter side B length:");
        string sideB = Console.ReadLine();
        Console.WriteLine("Enter side C length:");
        string sideC = Console.ReadLine();

        (string triangleType, List<(int, int)> coordinates) = TriangleCalculator.CalculateTriangleTypeAndCoordinates(sideA, sideB, sideC);

        Console.WriteLine($"Triangle Type: {triangleType}");
        Console.WriteLine($"Coordinates: A({coordinates[0].Item1}, {coordinates[0].Item2}), " +
            $"B({coordinates[1].Item1}, {coordinates[1].Item2}), " +
            $"C({coordinates[2].Item1}, {coordinates[2].Item2})");
    }
}