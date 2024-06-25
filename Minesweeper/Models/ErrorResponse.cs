using System.Text.Json.Serialization;

namespace Minesweeper.Models
{
    [Serializable]
    public class ErrorResponse
    {
        [JsonPropertyName("error")]
        public string ErrorMessage { get; set; } = null!;
    }
}
