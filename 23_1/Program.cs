using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _23_1
{
	class Program
	{
		const string Input =
@"set b 81
set c b
jnz a 2
jnz 1 5
mul b 100
sub b -100000
set c b
sub c -17000
set f 1
set d 2
set e 2
set g d
mul g e
sub g b
jnz g 2
set f 0
sub e -1
set g e
sub g b
jnz g -8
sub d -1
set g d
sub g b
jnz g -13
jnz f 2
sub h -1
set g b
sub g c
jnz g 2
jnz 1 3
sub b -17
jnz 1 -23";

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
			[Mnemonic("set")] Set,
			[Mnemonic("sub")] Subtract,
			[Mnemonic("mul")] Multiply,
			[Mnemonic("jnz")] Branch,
		}

		static Dictionary<string, InstructionType> InstructionTypeByMnemonic = typeof(InstructionType).GetFields(BindingFlags.Public | BindingFlags.Static)
			.Select(field => (Field: field, MnemonicAttribute: field.GetCustomAttribute<MnemonicAttribute>()))
			.ToDictionary(pair => pair.MnemonicAttribute.Mnemonic, pair => (InstructionType)pair.Field.GetValue(null));

		class Instruction
		{
			public InstructionType Type;
			public Action<long> Operand1_set;
			public Func<long> Operand1_get;
			public Func<long> Operand2_get;
		}

		class Machine
		{
			public long[] Registers = new long[26];
			List<Instruction> instructions = new List<Instruction>();
			public int InstructionPointer;

			public void LoadInstructions(string input)
			{
				using (var reader = new StringReader(input))
				{
					while (true)
					{
						var code = reader.ReadLine();

						if (code == null)
							break;

						string[] parts = code.Split(' ');

						var instruction = new Instruction();

						instruction.Type = InstructionTypeByMnemonic[parts[0]];

						if (long.TryParse(parts[1], out long literal1))
							instruction.Operand1_get = () => literal1;
						else
						{
							int registerIndex = char.ToLowerInvariant(parts[1][0]) - 'a';

							instruction.Operand1_get = () => Registers[registerIndex];
							instruction.Operand1_set = (value) => Registers[registerIndex] = value;
						}

						if (parts.Length >= 3)
						{
							if (long.TryParse(parts[2], out long literal2))
								instruction.Operand2_get = () => literal2;
							else
							{
								int registerIndex = char.ToLowerInvariant(parts[2][0]) - 'a';

								instruction.Operand2_get = () => Registers[registerIndex];
							}
						}

						instructions.Add(instruction);
					}
				}
			}

			public int RunUntilIPOutOfBounds()
			{
				int multiplicationsPerformed = 0;

				while (InstructionPointer < instructions.Count)
				{
					var instruction = instructions[InstructionPointer];

					switch (instruction.Type)
					{
						case InstructionType.Set:
							instruction.Operand1_set(instruction.Operand2_get());
							break;
						case InstructionType.Subtract:
							instruction.Operand1_set(
								instruction.Operand1_get() - instruction.Operand2_get());
							break;
						case InstructionType.Multiply:
							instruction.Operand1_set(
								instruction.Operand1_get() * instruction.Operand2_get());
							multiplicationsPerformed++;
							break;
						case InstructionType.Branch:
							if (instruction.Operand1_get() != 0)
								InstructionPointer = (int)(InstructionPointer + instruction.Operand2_get() - 1);
							break;
					}

					InstructionPointer++;
				}

				return multiplicationsPerformed;
			}
		}

		static void Main(string[] args)
		{
			var machine = new Machine();

			machine.LoadInstructions(Input);

			Console.WriteLine("Multiplications: {0}", machine.RunUntilIPOutOfBounds());
		}
	}
}
