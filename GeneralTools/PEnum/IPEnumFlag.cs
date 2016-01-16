namespace Pseudo
{
	public interface IPEnumFlag : IPEnum
	{
		new ByteFlag Value { get; }

		IPEnumFlag Add(IPEnumFlag flags);
		IPEnumFlag Add(ByteFlag flags);
		IPEnumFlag Add(byte flag);
		IPEnumFlag Remove(IPEnumFlag flags);
		IPEnumFlag Remove(ByteFlag flags);
		IPEnumFlag Remove(byte flag);
		IPEnumFlag And(IPEnumFlag flags);
		IPEnumFlag And(ByteFlag flags);
		IPEnumFlag Or(IPEnumFlag flags);
		IPEnumFlag Or(ByteFlag flags);
		IPEnumFlag Xor(IPEnumFlag flags);
		IPEnumFlag Xor(ByteFlag flags);
		IPEnumFlag Not();
		bool Has(byte flag);
		bool HasAll(IPEnumFlag flags);
		bool HasAll(ByteFlag flags);
		bool HasAny(IPEnumFlag flags);
		bool HasAny(ByteFlag flags);
		bool HasNone(IPEnumFlag flags);
		bool HasNone(ByteFlag flags);
	}
}