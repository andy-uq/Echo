using System.Linq;

namespace Echo.Engine
{
	public class IdleTimer
	{
		private const int SampleCount = 5 * 60;
		private readonly double[] _data;
		private int _tail;
		private int _position;

		public IdleTimer()
		{
			_data = new double[SampleCount + 1];
			_data[0] = 1.0;
			_tail = 0;
		}

		public double Idle => _data.Take(_tail).DefaultIfEmpty(1.0).Average() * 100.0;

		public void Enqueue(double idle)
		{
			_data[_position] = idle;
			_position++;

			if (_tail < SampleCount)
			{
				_tail++;
			}
			else
			{
				if (_position >= _tail)
					_position = 0;
			}
		}
	}
}