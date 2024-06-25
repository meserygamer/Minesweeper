using Microsoft.AspNetCore.Mvc;
using Minesweeper.Models;
using Minesweeper.Models.Game;

namespace Minesweeper.Controllers
{
    [Controller]
    [Route("")]
    public class GameController : Controller
    {
        public GameController(GameRepository gameRepository) 
        {
            _gameRepository = gameRepository;
        }


        private GameRepository _gameRepository;


        [HttpPost]
        [Route("new")]
        public IActionResult CreateGame([FromBody] GameSettingsRequest gameSettings)
        {
            if( gameSettings.Width < 2 || gameSettings.Width > 30)
                return BadRequest(new ErrorResponse() 
                { 
                    ErrorMessage = "Ширина поля должна быть не менее 2 и не более 30"
                });

            if (gameSettings.Height < 2 || gameSettings.Height > 30)
                return BadRequest(new ErrorResponse()
                {
                    ErrorMessage = "Высота поля должна быть не менее 2 и не более 30"
                });

            if (gameSettings.MinesCount >= gameSettings.Width * gameSettings.Height)
                return BadRequest(new ErrorResponse()
                {
                    ErrorMessage = $"количество мин должно быть не менее 1 и не более {gameSettings.Width * gameSettings.Height - 1}"
                });

            SingleGame game = _gameRepository.CreateSingleGame(gameSettings.Height, gameSettings.Width, gameSettings.MinesCount);
            GameInfoResponse gameInfoResponse = new GameInfoResponse()
            {
                GameId = game.GameId,
                Width = game.GameField.Width,
                Height = game.GameField.Height,
                MinesCount = game.GameField.MinesCount,
                Field = game.PlayersVisionOnField
            };

            return Ok(gameInfoResponse);
        }

        [HttpPost]
        [Route("turn")]
        public IActionResult MakeTurn([FromBody] PlayersTurnRequest playersTurn)
        {
            if(!_gameRepository.Games.TryGetValue(playersTurn.GameId, out SingleGame? game))
                return BadRequest(new ErrorResponse()
                {
                    ErrorMessage = $"игра с идентификатором {playersTurn.GameId} не была создана или устарела (неактуальна)"
                });

            if (game.IsGameComplete)
                return BadRequest(new ErrorResponse()
                { 
                    ErrorMessage = "Игра завершена, вы больше не можете делать новые ходы"
                });

            if(playersTurn.Row > game.GameField.Height - 1 || playersTurn.Row < 0)
                return BadRequest(new ErrorResponse()
                {
                    ErrorMessage = $"в игре нет ряда с номером {playersTurn.Row}"
                });

            if (playersTurn.Column > game.GameField.Width - 1 || playersTurn.Column < 0)
                return BadRequest(new ErrorResponse()
                {
                    ErrorMessage = $"в игре нет колонки с номером {playersTurn.Row}"
                });

            FieldCoordinates playersTurnCordinates = new FieldCoordinates() { YRow = playersTurn.Row, XColumn = playersTurn.Column };
            if (game.FieldVision.IsCellVisibleForUser(playersTurnCordinates))
                return BadRequest(new ErrorResponse()
                {
                    ErrorMessage = $"Клетка в колонке {playersTurn.Column} строки {playersTurn.Row} уже открыта"
                });

            game.MakeTurn(playersTurnCordinates);

            GameInfoResponse gameInfoResponse = new GameInfoResponse()
            {
                GameId = game.GameId,
                Width = game.GameField.Width,
                Height = game.GameField.Height,
                MinesCount = game.GameField.MinesCount,
                Field = game.PlayersVisionOnField
            };

            return Ok(gameInfoResponse);
        }
    }
}
