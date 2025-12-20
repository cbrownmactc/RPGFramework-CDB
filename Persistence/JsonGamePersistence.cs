using RPGFramework.Geography;

namespace RPGFramework.Persistence
{
    /// <summary>
    /// Provides JSON-based persistence for game data, including areas and players.
    /// </summary>
    /// <remarks> <see cref="JsonGamePersistence"/> implements the <see cref="IGamePersistence"/>
    /// interface to load and save game data using JSON files stored on disk. 
    /// Areas and players are persisted in separate directories, and each entity is 
    /// stored as an individual file. This class is not
    /// thread-safe. Concurrent calls to persistence methods may result in inconsistent 
    /// data if not externally synchronized.</remarks>
    public sealed class JsonGamePersistence : IGamePersistence
    {
        public Task<Area?> LoadAreaAsync(string areaName)
        {
            var area = ObjectStorage.LoadObject<Area>($"data/areas/",$"{areaName}");            
            return Task.FromResult(area);
        }

        public Task<IReadOnlyDictionary<int, Area>> LoadAreasAsync()
        {
            var areas = ObjectStorage.LoadAllObjects<Area>("data/areas/");
            var dict = areas.ToDictionary(a => a.Id);
            return Task.FromResult((IReadOnlyDictionary<int, Area>)dict);
        }

        public Task<IReadOnlyDictionary<string, Player>> LoadPlayersAsync()
        {
            var players = ObjectStorage.LoadAllObjects<Player>("data/players/");
            var dict = players.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
            return Task.FromResult((IReadOnlyDictionary<string, Player>)dict);
        }

        public Task SaveAreasAsync(IEnumerable<Area> areas)
        {
            foreach (var area in areas)
            {
                ObjectStorage.SaveObject(area, "data/areas/", $"{area.Name}.json");
            }

            return Task.CompletedTask;
        }

        public Task SavePlayersAsync(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                ObjectStorage.SaveObject(player, "data/players/", $"{player.Name}.json");
            }

            return Task.CompletedTask;
        }

        public Task SavePlayerAsync(Player player)
        {
            ObjectStorage.SaveObject(player, "data/players/", $"{player.Name}.json");
            return Task.CompletedTask;
        }
    }
}
