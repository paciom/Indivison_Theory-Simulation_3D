using Models;
using Zenject;

namespace Collisions
{
    public class DotConnector : IDotConnector
    {
        private readonly IAppStateFactory _appStateFactory;
        private  AppState State => _appStateFactory.State;

        [Inject]
        public DotConnector(IAppStateFactory appStateFactory)
        {
            _appStateFactory = appStateFactory;
        }

        public void Connect(Dot a, Dot b)
        {
            if (AreConnected(a, b))
            {
                return;
            }

            int aConnected = Disconnect(a) ?? a.Id;
            int bConnected = Disconnect(b) ?? b.Id;

            var dots = State.Data.Dots;
            _connect(dots[aConnected], dots[bConnected]); // Assuming dots will always contain the keys.
        }

        private void _connect(Dot a, Dot b)
        {
            a.ConnectedDotId = b.Id;
            b.ConnectedDotId = a.Id;
        }

        public bool AreConnected(Dot a, Dot b)
        {
            return a.ConnectedDotId != null && a.ConnectedDotId == b.ConnectedDotId;
        }

        public int? Disconnect(Dot d)
        {
            var connectedId = d.ConnectedDotId;
            if (connectedId != null)
            {
                if (State.Data.Dots.TryGetValue(connectedId.Value, out var connectedDot))
                {
                    connectedDot.ConnectedDotId = null;
                    d.ConnectedDotId = null;

                    // SwapLocations(conn); // Uncomment and implement this if needed.
                    return connectedId;
                }
            }

            return null;
        }
    }
}
