using PipServices3.Commons.Convert;
using PipServices3.Commons.Validate;

namespace EIC.Contracts.Data.Version1
{
	public class ContractV1Schema : ObjectSchema
	{
		public ContractV1Schema()
		{
			this.WithRequiredProperty("number", TypeCode.String);
			this.WithRequiredProperty("date_org", TypeCode.DateTime);
			this.WithRequiredProperty("date_from", TypeCode.DateTime);
			this.WithRequiredProperty("date_into", TypeCode.DateTime);
			this.WithRequiredProperty("amount", TypeCode.Double);
			this.WithRequiredProperty("currency", TypeCode.String);
			this.WithRequiredProperty("rate", TypeCode.Double);
			this.WithRequiredProperty("customer", new CustomerV1Schema());
		}
	}
}
