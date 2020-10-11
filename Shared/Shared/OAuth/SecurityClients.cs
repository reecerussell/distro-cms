using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Shared.OAuth
{
    public class SecurityClients
    {
        private readonly ConcurrentDictionary<string, SecurityClientInfo> _clients;

        public SecurityClients(IEnumerable<SecurityClientInfo> clients)
        {
            _clients = new ConcurrentDictionary<string, SecurityClientInfo>(
                clients.ToDictionary(x => x.Id, x => x));
        }

        public string this[string id] => _clients[id]?.Secret;
    }
}
