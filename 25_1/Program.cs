using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _25_1
{
	class Program
	{
		const string TestInput =
@"Begin in state A.
Perform a diagnostic checksum after 6 steps.

In state A:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state B.
  If the current value is 1:
    - Write the value 0.
    - Move one slot to the left.
    - Continue with state B.

In state B:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the left.
    - Continue with state A.
  If the current value is 1:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state A.";

		const string Input =
@"Begin in state A.
Perform a diagnostic checksum after 12208951 steps.

In state A:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state B.
  If the current value is 1:
    - Write the value 0.
    - Move one slot to the left.
    - Continue with state E.

In state B:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the left.
    - Continue with state C.
  If the current value is 1:
    - Write the value 0.
    - Move one slot to the right.
    - Continue with state A.

In state C:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the left.
    - Continue with state D.
  If the current value is 1:
    - Write the value 0.
    - Move one slot to the right.
    - Continue with state C.

In state D:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the left.
    - Continue with state E.
  If the current value is 1:
    - Write the value 0.
    - Move one slot to the left.
    - Continue with state F.

In state E:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the left.
    - Continue with state A.
  If the current value is 1:
    - Write the value 1.
    - Move one slot to the left.
    - Continue with state C.

In state F:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the left.
    - Continue with state E.
  If the current value is 1:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state A.";

		class StateAction
		{
			public bool ValueToWrite;
			public int Delta;
			public char NextState;
		}

		class State
		{
			public char StateName;
			public StateAction[] Actions = new StateAction[2];
		}

		class Tape
		{
			const int BlockSize = 1000;

			Dictionary<int, bool[]> _blocks = new Dictionary<int, bool[]>();

			bool[] GetBlock(ref int offset)
			{
				int blockNumber;

				if (offset >= 0)
					blockNumber = offset / BlockSize;
				else
					blockNumber = (offset - BlockSize + 1) / BlockSize;

				offset = ((offset % BlockSize) + BlockSize) % BlockSize;

				if (!_blocks.TryGetValue(blockNumber, out bool[] block))
					block = _blocks[blockNumber] = new bool[BlockSize];

				return block;
			}

			public bool this[int offset]
			{
				get
				{
					var block = GetBlock(ref offset);

					return block[offset];
				}
				set
				{
					var block = GetBlock(ref offset);

					block[offset] = value;
				}
			}

			public int Checksum
			{
				get
				{
					return _blocks.Sum(entry => entry.Value.Sum(bit => bit ? 1 : 0));
				}
			}
		}

		static void Main(string[] args)
		{
			char initialState = '\0';
			int runLimit = -1;

			var initialStateRegex = new Regex("Begin in state (?<InitialState>[A-Z]).");
			var runLimitRegex = new Regex(@"Perform a diagnostic checksum after (?<RunLimit>\d+) steps.");
			var startStateRegex = new Regex("In state (?<State>[A-Z]):");
			var startActionRegex = new Regex(@"  If the current value is (?<ActionValue>\d):");
			var actionWriteRegex = new Regex(@"    - Write the value (?<WriteValue>\d).");
			var actionMoveRegex = new Regex("    - Move one slot to the (?<Direction>left|right).");
			var actionNextStateRegex = new Regex("    - Continue with state (?<NextState>[A-Z]).");

			Dictionary<char, State> states = new Dictionary<char, State>();

			State currentParsingState = null;
			StateAction currentParsingAction = null;

			using (var reader = new StringReader(Input))
			{
				while (true)
				{
					string line = reader.ReadLine();

					if (line == null)
						break;

					if (initialStateRegex.Match(line) is Match initialStateMatch && initialStateMatch.Success)
						initialState = initialStateMatch.Groups["InitialState"].Value[0];
					else if (runLimitRegex.Match(line) is Match runLimitMatch && runLimitMatch.Success)
						runLimit = int.Parse(runLimitMatch.Groups["RunLimit"].Value);
					else if (startStateRegex.Match(line) is Match startStateMatch && startStateMatch.Success)
					{
						currentParsingState = new State();
						currentParsingState.StateName = startStateMatch.Groups["State"].Value[0];

						states[currentParsingState.StateName] = currentParsingState;
					}
					else if (startActionRegex.Match(line) is Match startActionMatch && startActionMatch.Success)
					{
						int actionValue = int.Parse(startActionMatch.Groups["ActionValue"].Value);

						currentParsingAction = currentParsingState.Actions[actionValue] = new StateAction();
					}
					else if (actionWriteRegex.Match(line) is Match actionWriteMatch && actionWriteMatch.Success)
						currentParsingAction.ValueToWrite = (int.Parse(actionWriteMatch.Groups["WriteValue"].Value) != 0);
					else if (actionMoveRegex.Match(line) is Match actionMoveMatch && actionMoveMatch.Success)
					{
						switch (actionMoveMatch.Groups["Direction"].Value)
						{
							case "left": currentParsingAction.Delta = -1; break;
							case "right": currentParsingAction.Delta = +1; break;
						}
					}
					else if (actionNextStateRegex.Match(line) is Match actionNextStateMatch && actionNextStateMatch.Success)
						currentParsingAction.NextState = actionNextStateMatch.Groups["NextState"].Value[0];
				}
			}

			var tape = new Tape();
			int position = 0;
			char stateName = initialState;

			for (int i = 0; i < runLimit; i++)
			{
				var state = states[stateName];

				int value = tape[position] ? 1 : 0;

				var action = state.Actions[value];

				tape[position] = action.ValueToWrite;
				position += action.Delta;
				stateName = action.NextState;
			}

			Console.WriteLine(tape.Checksum);
		}
	}
}
