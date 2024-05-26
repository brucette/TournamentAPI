2# TournamentAPI

## Overview

This project involves building a custom API backend with ASP.NET Core, allowing users to perform CRUD operations on Tournaments and their associated Games. Users can make specific API calls to retrieve information about tournaments and games within each tournament, and use PUT, PATCH, POST, and DELETE requests to modify the database. The project also implements a repository pattern.

## Features

- Retrieve information about tournaments and games within each tournament
- Perform CRUD operations on tournaments and games
- Repository pattern

## Endpoints

### Tournaments

- `GET /api/tournaments` - Retrieves all tournaments
- `GET /api/tournaments?includeGames=true` - Retrieves all tournaments and their games
- `GET /api/tournaments/{id}` - Retrieves a specific tournament by ID
- `POST /api/tournaments` - Creates a new tournament
- `PUT /api/tournaments/{id}` - Updates an existing tournament
- `PATCH /api/tournaments/{id}` - Partially updates an existing tournament
- `DELETE /api/tournaments/{id}` - Deletes a specific tournament

### Games

- `GET /api/tournaments/{tournamentId}/games` - Retrieves all games for a specific tournament
- `GET /api/tournaments/{tournamentId}/games/title/{title}` - Retrieves games by title within a tournament
- `GET /api/games` - Retrieves all games
- `POST /api/tournaments/{tournamentId}/games` - Creates a new game within a tournament
- `PUT /api/tournaments/{tournamentId}/games/{gameId}` - Updates an existing game within a tournament
- `PATCH /api/tournaments/{tournamentId}/games/{gameId}` - Partially updates an existing game within a tournament
- `DELETE /api/tournaments/{tournamentId}/games/{gameId}` - Deletes a specific game within a tournament

## Technologies Used

- ASP.NET Core
- Entity Framework Core
- AutoMapper