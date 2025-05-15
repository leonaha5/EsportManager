namespace EsportManager.Models;

public class Player
{
    public int Id { get; set; }
    public string Nickname { get; set; }
    public int SkillLevel { get; set; }
    public int StressLevel { get; set; }
    public int FatigueLevel { get; set; }
    public int Points { get; set; }
    public string Game { get; set; }
    public decimal Money { get; set; }
}