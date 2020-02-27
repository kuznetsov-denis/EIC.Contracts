using MongoDB.Bson.Serialization.Attributes;
using PipServices3.Commons.Data;
using System;

namespace EIC.Contracts.Persistence.MongoDb
{
	[BsonIgnoreExtraElements]
	[BsonNoId]
	public class ContractMongoDbSchema : IStringIdentifiable
	{
		[BsonElement("id")]
		public string Id { get; set; }

		[BsonElement("number")]
		public string Number { get; set; }

		[BsonElement("date_org")]
		public DateTime DateOrg { get; set; }

		[BsonElement("date_from")]
		public DateTime DateFrom { get; set; }

		[BsonElement("date_into")]
		public DateTime DateInto { get; set; }

		[BsonElement("amount")]
		public double Amount { get; set; }

		[BsonElement("currency")]
		public string Currency { get; set; }

		[BsonElement("rate")]
		public double Rate { get; set; }

		[BsonElement("customer_id")]
		public string CustomerId { get; set; }
	}
}
