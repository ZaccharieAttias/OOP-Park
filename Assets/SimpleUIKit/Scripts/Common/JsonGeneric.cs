using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.Common
{
	public static class JsonGeneric
	{
		public static string ToJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

		public static T FromJson<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static List<T> FromJsonList<T>(string json)
		{
			var wrapper = FromJson<Wrapper<T>>("{\"Items\":" + json + "}");

			return wrapper.Items;
		}

		public static string ToJson<T>(List<T> array, bool minimize = true)
		{
			if (array == null) throw new ArgumentException(nameof(array));

			var wrapper = new Wrapper<T> { Items = array };
			var json = ToJson(wrapper);

			json = json.Substring(9, json.Length - 9 - 1);

			return minimize ? Minimize(json) : json;
		}

		[Serializable]
		private class Wrapper<T>
		{
			public List<T> Items;
		}

		public static string Minimize(string json)
		{
			if (json == null) return json;

			json = Regex.Replace(json, "\"\\w+\":\\[\\],", "");
			json = Regex.Replace(json, ",\"\\w+\":\\[\\]", "");

			return json;
		}
	}
}