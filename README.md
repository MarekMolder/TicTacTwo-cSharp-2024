# Tic-Tac-Two

**Tic-Tac-Two** is a cross-platform game where players can play **Player vs Player**, **Player vs AI**, or **AI vs AI**. The game is implemented in both a console application and a web app using Razor Pages. It allows players to start the game in the console, save the game state, and continue playing on the web (and vice versa).

## Features
- **Two-player gameplay**: Play with a friend or against AI.
- **Game modes**:
  - **Player vs Player** (matching or contrasting pieces)
  - **Player vs AI**
  - **AI vs AI** (AI makes its own moves)
- **Cross-platform game state persistence**: Save the game in either the console or web app and continue from where you left off, regardless of the platform.
- **Flexible board sizes**: Supports dynamic grid sizes (e.g., 3x3, 5x5, etc.).
- **Saving and loading game state**: Initially implemented using a file system (JSON), later extended to support SQLite databases.
- **Game configuration**: User-configurable settings for the board size, number of pieces, and game modes.
- **Authentication**: Supports username-based authentication for games.

- ## Project Structure
- **ConsoleApp**: The main console application where the game is played and configured.
- **ConsoleUI**: Handles the console-specific user interface (UI) design for interacting with the game logic.
- **DAL (Data Access Layer)**: Handles data persistence. Implements repository patterns for game state storage in both file system (JSON) and database (SQLite).
- **Domain**: Contains the core logic for the game, such as the game state, game rules, and player interactions.
- **GameBrain**: Contains the brain of the game (game logic and state updates).
- **MenuSystem**: Handles the navigation and menu-driven interface.
- **WebApp**: The web version of the game, built using ASP.NET Core Razor Pages, supporting multiple games and user authentication.

- ## Installation
### Prerequisites
- .NET 9 installed
- A web browser for the Razor Pages application
- SQLite for database persistence

- ### Running the Console Application
1. Navigate to the `ConsoleApp` directory.
2. Run the following command:
   ```sh
   dotnet run
   ```
3. Follow the on-screen instructions to start a new game or load a saved one.

### Running the Web Application
1. Navigate to the `WebApp` directory.
2. Run the following command:
   ```sh
   dotnet run
   ```
3. Open a web browser and go to `http://localhost:5000`.
4. Start a new game or continue a saved one.

## License
This project is licensed under the MIT License.
