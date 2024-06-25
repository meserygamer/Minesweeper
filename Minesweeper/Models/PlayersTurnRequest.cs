using System.Text.Json.Serialization;

namespace Minesweeper.Models
{
    [Serializable]
    public class PlayersTurnRequest
    {
        [JsonPropertyName("game_id")]
        public Guid GameId { get; set; }

        [JsonPropertyName("col")]
        public int Column { get; set; }

        [JsonPropertyName("row")]
        public int Row { get; set; }
    }
}
