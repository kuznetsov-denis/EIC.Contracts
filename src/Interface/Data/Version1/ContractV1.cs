using System;
using System.Runtime.Serialization;
using PipServices3.Commons.Data;

namespace EIC.Contracts.Data.Version1
{
	[DataContract]
	public class ContractV1 : IStringIdentifiable
	{
		[DataMember(Name = "id")]
		public string Id { get; set; }

		[DataMember(Name = "number")]
		public string Number { get; set; }

		[DataMember(Name = "date_org")]
		public DateTime DateOrg { get; set; }

		[DataMember(Name = "date_from")]
		public DateTime DateFrom { get; set; }

		[DataMember(Name = "date_into")]
		public DateTime DateInto { get; set; }

		[DataMember(Name = "amount")]
		public double Amount { get; set; }

		[DataMember(Name = "currency")]
		public string Currency { get; set; }

		[DataMember(Name = "rate")]
		public double Rate { get; set; }

		[DataMember(Name = "customer")]
		public CustomerV1 Customer { get; set; } = new CustomerV1();
	}
}
