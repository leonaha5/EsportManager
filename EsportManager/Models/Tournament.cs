namespace EsportManager.Models;

public class Tournament
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int EntryFee { get; set; }
    public int PrizePool { get; set; }
    public int MinSkillRequired { get; set; }
}