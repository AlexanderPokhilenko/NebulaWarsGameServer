﻿using System.Threading.Tasks;
using Code.Common;

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
        
        public async Task<bool> Notify(int matchId)
        {
            string pathname = "/GameServer/DeleteRoom";
            string query = $"?MatchId={matchId.ToString()}";
            return await httpWrapper.HttpDelete(pathname, query);
        }
    }
}