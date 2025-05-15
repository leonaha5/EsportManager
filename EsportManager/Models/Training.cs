namespace EsportManager.Models;

public class Training
{
    public int Id { get; set; }
    public string Type { get; set; }
    public int SkillIncrease { get; set; }
    public int FatigueIncrease { get; set; }
    public int StressIncrease { get; set; }
}