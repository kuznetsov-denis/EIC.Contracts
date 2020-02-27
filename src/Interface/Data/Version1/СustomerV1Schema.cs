using PipServices3.Commons.Convert;
using PipServices3.Commons.Validate;

namespace EIC.Contracts.Data.Version1
{
	public class CustomerV1Schema : ObjectSchema
	{
		public CustomerV1Schema()
		{
			//this.WithRequiredProperty("id", TypeCode.String);
			this.WithRequiredProperty("last_name", TypeCode.String);
			this.WithRequiredProperty("first_name", TypeCode.String);
			this.WithRequiredProperty("middle_name", TypeCode.String);
			this.WithRequiredProperty("gender", TypeCode.String);
			this.WithRequiredProperty("birthdate", TypeCode.DateTime);
			this.WithRequiredProperty("itin", TypeCode.String);
			this.WithRequiredProperty("passport", TypeCode.String);
		}
	}
}
