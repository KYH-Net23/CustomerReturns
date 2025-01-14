﻿namespace ReturnProvider.Models.Entities;

public class ReturnEntity
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string CustomerEmail { get; set; } = null!;
    public string ReturnReason { get; set; } = null!;
    public string ResolutionType { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
