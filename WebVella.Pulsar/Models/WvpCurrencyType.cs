using System.ComponentModel;

namespace WebVella.Pulsar.Models
{
	public class WvpCurrencyType
	{
		[Description("symbol")]
		public string Symbol { get; set; }

		[Description("symbolNative")]
		public string SymbolNative { get; set; }

		[Description("name")]
		public string Name { get; set; }

		[Description("namePlural")]
		public string NamePlural { get; set; }

		[Description("code")]
		public string Code { get; set; }

		[Description("decimalDigits")]
		public int DecimalDigits { get; set; }

		[Description("rounding")]
		public int Rounding { get; set; }

		[Description("symbolPlacement")]
		public CurrencySymbolPlacement SymbolPlacement { get; set; } = CurrencySymbolPlacement.After;
	}

	public enum CurrencySymbolPlacement
	{
		Before = 1,
		After
	}

}
