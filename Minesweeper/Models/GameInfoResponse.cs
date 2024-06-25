using System.Text.Json.Serialization;

namespace Minesweeper.Models
{
    [Serializable]
    public class GameInfoResponse
    {
        [JsonPropertyName("game_id")]
        public Guid GameId { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("mines_count")]
        public int MinesCount { get; set; }

        [JsonPropertyName("field")]
        public string[][] Field { get; set; } = null!;

        [JsonPropertyName("completed")]
        public bool IsGameCompleted { get; set; }
    }
}
