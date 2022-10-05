using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using WaterToolkit;

namespace FlappyBird
{
	public interface IDataVersion
	{
		public IDataVersion ToPrevVersion();
		public IDataVersion ToNextVersion();
	}

	public class FlappyData
	{
		private List<Dictionary<string, List<object>>> _dataSlots = new List<Dictionary<string, List<object>>>();

		public string filePath => PathUtilities.GetPath($@"C:\Users\Admor\Desktop\flappy-data.json");

		public int slotCount => _dataSlots.Count;

		public T Get<T>(int typeSlotId = 0) where T : new() => Get<T>(0, typeSlotId);

		public T Get<T>(int slotId, int typeSlotId = 0) where T : new()
		{
			List<object> dataList = GetDataList(slotId, typeof(T));

			while(dataList.Count <= typeSlotId) {
				dataList.Add(new T());
			}

			if(dataList[typeSlotId] == null) {
				dataList[typeSlotId] = new T();
			}

			return (T)dataList[typeSlotId];
		}

		public void Delete<T>(int typeSlotId = 0) => Delete<T>(0, typeSlotId);

		public void Delete<T>(int slotId, int typeSlotId = 0)
		{
			List<object> dataList = GetDataList(slotId, typeof(T));

			if(dataList.Count <= typeSlotId) {
				return;
			}

			dataList[typeSlotId] = null;
		}

		public void Serialize()
		{
			File.WriteAllText(
				PathUtilities.GetPath(filePath),
				JsonConvert.SerializeObject(
					_dataSlots, Formatting.Indented,
					new JsonSerializerSettings {
						ContractResolver = AllFieldsContractResolver.instance,
					})
			);
		}

		public void Deserialize()
		{
			_dataSlots = JsonConvert.DeserializeObject<List<Dictionary<string, List<object>>>>(
				File.ReadAllText(filePath), 
				new JsonSerializerSettings {
					ContractResolver = AllFieldsContractResolver.instance
				});

			JsonSerializer serializer = new JsonSerializer();
			serializer.ContractResolver = AllFieldsContractResolver.instance;

			for(int i = 0; i < _dataSlots.Count; i++) {
				Dictionary<string, List<object>> dataSlot = _dataSlots[i];

				foreach(KeyValuePair<string, List<object>> pair in dataSlot) {
					Type dataType = Type.GetType(pair.Key);
					List<object> dataList = pair.Value;
					for(int a = 0; a < dataList.Count; a++) {
						dataList[a] = ((JObject)dataList[a]).ToObject(dataType, serializer);

						if(dataList[a] is IDataVersion dataVersion) {
							IDataVersion next = null;
							do {
								next = dataVersion.ToNextVersion();
								if(next != null) { dataVersion = next; }
							} while(next != null);

							dataList[a] = dataVersion;
						}
					}
				}

				foreach(string key in dataSlot.Keys.ToArray()) {
					Type dataType = Type.GetType(key);
					Type dataListType = dataSlot[key].FirstOrDefault(o => o != null).GetType();

					if(dataType != dataListType) {
						List<object> dataList = dataSlot[key];
						dataSlot.Remove(key);
						dataSlot[dataListType.AssemblyQualifiedName] = dataList;
					}
				}
			}
		}

		private List<object> GetDataList(int slotId, Type type)
		{
			string typeName = type.AssemblyQualifiedName;

			while(_dataSlots.Count <= slotId) {
				_dataSlots.Add(new Dictionary<string, List<object>>());
			}

			if(_dataSlots[slotId] == null) {
				_dataSlots[slotId] = new Dictionary<string, List<object>>();
			}

			Dictionary<string, List<object>> dataDirectory = _dataSlots[slotId];
			bool hasList = dataDirectory.TryGetValue(typeName, out List<object> list);
			if(!hasList) {
				list = dataDirectory[typeName] = new List<object>();
			}

			return list;
		}

		private class AllFieldsContractResolver : DefaultContractResolver
		{
			internal static readonly AllFieldsContractResolver instance = new AllFieldsContractResolver();

			protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
			{
				List<JsonProperty> props = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
					.Select(p => base.CreateProperty(p, memberSerialization))
					.ToList();

				props.ForEach(p => { p.Writable = true; p.Readable = true; });

				return props;
			}
		}
	}
}
