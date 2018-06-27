using CitizenFX.Core;
using FreeroamServer.Http;
using System;

namespace FreeroamServer
{
    class Idiot : BaseScript
    {
        public Idiot()
        {
            EventHandlers["freeroam:getIdiotMessage"] += new Action(() =>
            {
                Request request = new Request();
                // TODO: POST to my webserver
            });
        }
    }
}
