using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _18_1
{
	class Program
	{
		const string Input = RealInput;

		const string TestInput =
@"set a 1
add a 2
mul a a
mod a 5
snd a
set a 0
rcv a
jgz a -1
set a 1
jgz a -2";

		const string RealInput =
@"set i 31
set a 1
mul p 17
jgz p p
mul a 2
add i -1
jgz i -2
add a -1
set i 127
set p 735
mul p 8505
mod p a
mul p 129749
add p 12345
mod p a
set b p
mod b 10000
snd b
add i -1
jgz i -9
jgz a 3
rcv b
jgz b -1
set f 0
set i 126
rcv a
rcv b
set p a
mul p -1
add p b
jgz p 4
snd a
set a b
jgz 1 3
snd b
set f 1
add i -1
jgz i -11
snd a
jgz f -16
jgz a -19";

		class MnemonicAttribute : Attribute
		{
			public string Mnemonic;

			public MnemonicAttribute(string mnemonic)
			{
				this.Mnemonic = mnemonic;
			}
		}

		enum InstructionType
		{
			[Mnemonic("snd")] Sound,
			[Mnemonic("set")] Set,
			[Mnemonic("add")] Add,
			[Mnemonic("mul")] Multiply,
			[Mnemonic("mod")] Modulus,
			[Mnemonic("rcv")] Recover,
			[Mnemonic("jgz")] Branch,
		}

		class Instruction
		{
			public InstructionType Type;
			public Action<long> Operand1_set;
			public Func<long> Operand1_get;
			public Func<long> Operand2_get;
		}

		static void Main(string[] args)
		{
			var instructionTypeByMnemonic = typeof(InstructionType).GetFields(BindingFlags.Public | BindingFlags.Static)
				.Select(field => (Field: field, MnemonicAttribute: field.GetCustomAttribute<MnemonicAttribute>()))
				.ToDictionary(pair => pair.MnemonicAttribute.Mnemonic, pair => (InstructionType)pair.Field.GetValue(null));

			long[] registers = new long[26];
			List<Instruction> instructions = new List<Instruction>();

			using (var reader = new StringReader(Input))
			{
				while (true)
				{
					var code = reader.ReadLine();

					if (code == null)
						break;

					string[] parts = code.Split(' ');

					var instruction = new Instruction();

					instruction.Type = instructionTypeByMnemonic[parts[0]];

					if (long.TryParse(parts[1], out long literal1))
						instruction.Operand1_get = () => literal1;
					else
					{
						int registerIndex = char.ToLowerInvariant(parts[1][0]) - 'a';

						instruction.Operand1_get = () => registers[registerIndex];
						instruction.Operand1_set = (value) => registers[registerIndex] = value;
					}

					if (parts.Length >= 3)
					{
						if (long.TryParse(parts[2], out long literal2))
							instruction.Operand2_get = () => literal2;
						else
						{
							int registerIndex = char.ToLowerInvariant(parts[2][0]) - 'a';

							instruction.Operand2_get = () => registers[registerIndex];
						}
					}

					instructions.Add(instruction);
				}
			}

			long lastFrequencyPlayed = 0;
			int instructionPointer = 0;
			int timeElapsed = 0;

			while (instructionPointer >= 0)
			{
				var instruction = instructions[instructionPointer];

				switch (instruction.Type)
				{
					case InstructionType.Sound:
						lastFrequencyPlayed = instruction.Operand1_get();
						Console.WriteLine("{0}  SOUND: {1}", timeElapsed, lastFrequencyPlayed);
						break;
					case InstructionType.Set:
						instruction.Operand1_set(instruction.Operand2_get());
						break;
					case InstructionType.Add:
						instruction.Operand1_set(
							instruction.Operand1_get() + instruction.Operand2_get());
						break;
					case InstructionType.Multiply:
						instruction.Operand1_set(
							instruction.Operand1_get() * instruction.Operand2_get());
						break;
					case InstructionType.Modulus:
						instruction.Operand1_set(
							instruction.Operand1_get() % instruction.Operand2_get());
						break;
					case InstructionType.Recover:
						if (instruction.Operand1_get() != 0)
						{
							Console.WriteLine("RECOVERED: {0}", lastFrequencyPlayed);
							instructionPointer = -2;
						}

						break;
					case InstructionType.Branch:
						if (instruction.Operand1_get() > 0)
							instructionPointer = (int)(instructionPointer + instruction.Operand2_get() - 1);
						break;
				}

				instructionPointer++;
				timeElapsed++;
			}
		}
	}
}
