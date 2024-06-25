using System.Text.Json.Serialization;

namespace Minesweeper.Models
{
    [Serializable]
    public class GameSettingsRequest
    {
        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("mines_count")]
        public int MinesCount { get; set; }
    }
}
