using System.Threading.Tasks;
using Plugins.submodules.SharedCode.Logger;

namespace Server.Http
{
    public class HttpMatchFinishNotifierService
    {
        private readonly HttpWrapper httpWrapper;
        private readonly ILog log = LogManager.CreateLogger(typeof(HttpMatchFinishNotifierService));

        public HttpMatchFinishNotifierService(HttpWrapper httpWrapper)
        {
            this.httpWrapper = httpWrapper;
        }
        
        public async Task<bool> TryNotify(int matchId)
        {
            string pathname = "/GameServer/DeleteMatch";
            string query = $"?MatchId={matchId.ToString()}&secret={Globals.GameServerSecret}";
            return await httpWrapper.HttpDelete(pathname, query);
        }
    }
}