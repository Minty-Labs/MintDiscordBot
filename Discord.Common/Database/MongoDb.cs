using Discord.Common.Configs;
using MongoDB.Driver;

namespace Discord.Common.Database
{
    public static class MongoDb
    {
        private static MongoClient _instance;

        public static MongoClient Instance
        {
            get
            {
                _instance ??= (string.IsNullOrEmpty(CommonConfig.Config.Instance.MongoDBAddress)) ? new MongoClient() : new MongoClient(CommonConfig.Config.Instance.MongoDBAddress);
                return _instance;
            }
        }
    }
}
