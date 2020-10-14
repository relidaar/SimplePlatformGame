using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SimplePlatformGame.GameObjects;

namespace SimplePlatformGame.JsonObjects
{
    public class JsonEnemy : JsonGameObject
    {
        public float Speed { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Direction Direction { get; set; }
    }
}