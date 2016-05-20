namespace Pseudo.Groupingz
{
	public interface IGroupManager<TElement, TId>
	{
		IGroupFactory<TElement> GroupFactory { get; }
		IConverter<TId, int> Converter { get; }

		IGroup<TElement> GetGroup(MatchType match, params TId[] filter);
		void Register(TElement element, TId filter);
		void Register(TElement element, params TId[] filters);
		void Unregister(TElement element, TId filter);
		void Unregister(TElement element, params TId[] filters);
	}
}