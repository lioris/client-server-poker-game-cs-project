using Contracts;
using System.ServiceModel;

namespace client
{
    class Proxy
    {
        private DuplexChannelFactory<IGameService> channel;
        private IGameService proxy;
        #region Singelton
        private static Proxy _instance;

        private Proxy()
        {
            // creat a channel to comunicate with the game server
            channel = new DuplexChannelFactory<IGameService>(ClientCallBack.Instance, "GameServiceEndpoint");
            proxy = channel.CreateChannel();
        }

        public static Proxy Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Proxy();

                return _instance;
            }
        }
        #endregion

        internal IGameService GetProxy()
        {
            return proxy;
        }
    }
}
