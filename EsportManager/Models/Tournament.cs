using System;

namespace EsportManager.Models;

public class Tournament
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime EntryFee { get; set; }
    public decimal PrizePool { get; set; }
    public int MinSkillRequired { get; set; }
}