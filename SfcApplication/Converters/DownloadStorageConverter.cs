using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Downloader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SfcApplication.Converters
{
    public class DownloadStorageConverter:JsonConverter<IStorage>
    {
        #region Overrides of JsonConverter<IStorage>

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, IStorage value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>Reads the JSON representation of the object.</summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read. If there is no existing value then <c>null</c> will be used.</param>
        /// <param name="hasExistingValue">The existing value has a value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override IStorage ReadJson(JsonReader reader, Type objectType, IStorage existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var obj = JObject.Load(reader); // Throws an exception if the current token is not an object.
            if (obj.ContainsKey(nameof(FileStorage.FileName)))
            {
                var filename = obj[nameof(FileStorage.FileName)]?.Value<string>();
                return new FileStorage(filename);
            }

            if (obj.ContainsKey(nameof(MemoryStorage.Data)))
            {
                var data = obj[nameof(MemoryStorage.Data)]?.Value<string>();
                return new MemoryStorage() { Data = data };
            }

            return null;
        }

        #endregion
    }
}
