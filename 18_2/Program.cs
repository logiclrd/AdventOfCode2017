using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _18_2
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
			[Mnemonic("snd")] Send,
			[Mnemonic("set")] Set,
			[Mnemonic("add")] Add,
			[Mnemonic("mul")] Multiply,
			[Mnemonic("mod")] Modulus,
			[Mnemonic("rcv")] Receive,
			[Mnemonic("jgz")] Branch,
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
			public int ProgramID;
			public Queue<long> InputQueue = new Queue<long>();
			public int ValuesSent = 0;
			public int ValuesReceived = 0;
			public long[] Registers = new long[26];
			List<Instruction> instructions = new List<Instruction>();
			public int InstructionPointer;

			public Machine(int programID)
			{
				ProgramID = programID;
				Registers['p' - 'a'] = programID;
			}

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

			public int RunUntilIOWait(Queue<long> receiver)
			{
				int instructionsExecuted = 0;

				while (true)
				{
					var instruction = instructions[InstructionPointer];

					switch (instruction.Type)
					{
						case InstructionType.Send:
							var valueToSend = instruction.Operand1_get();
							receiver.Enqueue(valueToSend);
							Console.WriteLine("Machine {0} sent value {1}", ProgramID, valueToSend);
							ValuesSent++;
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
						case InstructionType.Receive:
							if (this.InputQueue.Count > 0)
							{
								var receivedValue = this.InputQueue.Dequeue();
								Console.WriteLine("Machine {0} received value {1}", ProgramID, receivedValue);
								instruction.Operand1_set(receivedValue);
								ValuesReceived++;
							}
							else
							{
								Console.WriteLine("Machine {0} blocking on I/O", ProgramID);
								return instructionsExecuted;
							}

							break;
						case InstructionType.Branch:
							if (instruction.Operand1_get() > 0)
								InstructionPointer = (int)(InstructionPointer + instruction.Operand2_get() - 1);
							break;
					}

					InstructionPointer++;
					instructionsExecuted++;
				}
			}
		}

		static void Main(string[] args)
		{
			var machine0 = new Machine(0);
			var machine1 = new Machine(1);

			machine0.LoadInstructions(Input);
			machine1.LoadInstructions(Input);

			while (machine0.RunUntilIOWait(machine1.InputQueue) + machine1.RunUntilIOWait(machine0.InputQueue) > 0)
				;

			Console.WriteLine("Machine 0 has sent {0} values", machine0.ValuesSent);
			Console.WriteLine("Machine 0 has received {0} values", machine0.ValuesReceived);
			Console.WriteLine("Machine 1 has sent {0} values", machine1.ValuesSent);
			Console.WriteLine("Machine 1 has received {0} values", machine1.ValuesReceived);
		}
	}
}
