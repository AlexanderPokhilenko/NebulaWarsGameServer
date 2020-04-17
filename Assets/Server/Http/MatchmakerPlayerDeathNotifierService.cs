using System.Threading.Tasks;
using Server.Http;

public class MatchmakerPlayerDeathNotifierService
{
    private readonly HttpWrapper httpWrapper;

    public MatchmakerPlayerDeathNotifierService(HttpWrapper httpWrapper)
    {
        this.httpWrapper = httpWrapper;
    }
    
    public async Task<bool> Notify(PlayerDeathData playerDeathData)
    {
        string pathname = "/GameServer/PlayerDeath";
        string query = $"?accountId={playerDeathData.PlayerId}" +
                       $"&placeInBattle={playerDeathData.PlaceInBattle}" +
                       $"&MatchId={playerDeathData.MatchId}";
        return await httpWrapper.HttpDelete(pathname, query);
    }
}