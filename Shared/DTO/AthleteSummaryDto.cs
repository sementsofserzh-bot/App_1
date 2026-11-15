namespace Shared
{
    /// <summary>
    /// Содержит краткую информацию о спортсмене: идентификатор и ФИО.
    /// </summary>
    public class AthleteSummaryDto
    {
        
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}