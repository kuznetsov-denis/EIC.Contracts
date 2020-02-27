using System;
using System.Runtime.Serialization;
using PipServices3.Commons.Data;

namespace EIC.Contracts.Data.Version1
{
	[DataContract]
	public class CustomerV1// : IStringIdentifiable
	{
		[DataMember(Name = "id")]
		public string Id { get; set; }

		[DataMember(Name = "last_name")]
		public string LastName { get; set; }

		[DataMember(Name = "first_name")]
		public string FirstName { get; set; }

		[DataMember(Name = "middle_name")]
		public string MiddleName { get; set; }

		[DataMember(Name = "gender")]
		public string Gender { get; set; }

		[DataMember(Name = "birthdate")]
		public DateTime Birthdate { get; set; }

		[DataMember(Name = "itin")]
		public string Itin { get; set; }

		[DataMember(Name = "passport")]
		public string Passport { get; set; }

		[DataMember(Name = "mobile_phone")]
		public string MobilePhone { get; set; }
	}
}
