using System;

namespace day10
{
	public class GiveCommandData
	{
		public int Id;
		public GiveCommandOutputTypes GiveCommandOutputType;

		public GiveCommandData(int id, string giveType)
		{
			Id = id;
			var parseResult = Enum.TryParse(giveType, true, out GiveCommandOutputType);
			if (!parseResult)
			{
				throw new ArgumentOutOfRangeException(giveType);
			}
		}

		public override string ToString()
		{
			return $"{GiveCommandOutputType}->{Id}";
		}
	}
}