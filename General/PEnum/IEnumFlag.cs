namespace Pseudo
{
	public interface IEnumFlag : IEnum
	{
		new ByteFlag Value { get; }

		IEnumFlag Add(IEnumFlag flags);
		IEnumFlag Add(ByteFlag flags);
		IEnumFlag Add(byte flag);
		IEnumFlag Remove(IEnumFlag flags);
		IEnumFlag Remove(ByteFlag flags);
		IEnumFlag Remove(byte flag);
		IEnumFlag And(IEnumFlag flags);
		IEnumFlag And(ByteFlag flags);
		IEnumFlag Or(IEnumFlag flags);
		IEnumFlag Or(ByteFlag flags);
		IEnumFlag Xor(IEnumFlag flags);
		IEnumFlag Xor(ByteFlag flags);
		IEnumFlag Not();
		bool Has(byte flag);
		bool HasAll(IEnumFlag flags);
		bool HasAll(ByteFlag flags);
		bool HasAny(IEnumFlag flags);
		bool HasAny(ByteFlag flags);
		bool HasNone(IEnumFlag flags);
		bool HasNone(ByteFlag flags);
	}
}