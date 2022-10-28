using Flurl.Http.Configuration;
using Flurl.Util;
using Flurl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.Extensions
{
    public class DataContractUrlEncodedSerializer : ISerializer
    {
        /// <summary>
        /// Serializes the specified object utilizing DataContract and DataMembers if present.
        /// </summary>
        /// <param name="obj">The object.</param>
        public string Serialize(object obj)
        {
            if (obj == null)
                return null;

            bool useDataMembers = TryGetDataMembers(obj, out IDictionary<string, string> dataMembers);

            var qp = new QueryParamCollection();
            foreach (var kv in obj.ToKeyValuePairs())
            {
                string key = useDataMembers && dataMembers.TryGetValue(kv.Key, out string dataKey) ? dataKey : kv.Key;
                qp.AddOrReplace(key, kv.Value, false, NullValueHandling.Ignore);
            }

            return qp.ToString(true);
        }

        private bool TryGetDataMembers(object obj, out IDictionary<string, string> dataMembers)
        {
            dataMembers = null;
            if (HasDataContract(obj))
            {
                dataMembers = obj.GetType().GetProperties()
                    .Where(HasDataMember)
                    .Select(ToDataMember)
                    .ToDictionary(p => p.PropertyName, p => p.DataMemberName);

                return true;
            }

            return false;
        }

        private bool HasDataMember(PropertyInfo pi) =>
            Attribute.IsDefined(pi, typeof(DataMemberAttribute));

        private bool HasDataContract(object obj) =>
            obj.GetType().GetCustomAttributes(typeof(DataContractAttribute), true).Any();

        private static (string PropertyName, string DataMemberName) ToDataMember(PropertyInfo pi) =>
            (pi.Name, ((DataMemberAttribute)Attribute.GetCustomAttribute(pi, typeof(DataMemberAttribute))).Name);

        /// <summary>
        /// Deserializes the specified s.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s">The s.</param>
        /// <exception cref="NotImplementedException">Deserializing to UrlEncoded not supported.</exception>
        public T Deserialize<T>(string s)
        {
            throw new NotImplementedException("Deserializing to UrlEncoded is not supported.");
        }

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <exception cref="NotImplementedException">Deserializing to UrlEncoded not supported.</exception>
        public T Deserialize<T>(Stream stream)
        {
            throw new NotImplementedException("Deserializing to UrlEncoded is not supported.");
        }
    }
}
