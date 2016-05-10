
namespace Pseudo
{
	public interface ITimeChannel
	{
		Chronos.TimeChannels Channel { get; }
		float Time { get; }
		float TimeScale { get; set; }
		float DeltaTime { get; }
		float FixedDeltaTime { get; }
	}
}