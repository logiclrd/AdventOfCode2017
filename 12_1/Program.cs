﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _12_1
{
	class Program
	{
		static void Main(string[] args)
		{
			var connections = new Dictionary<int, int[]>();

			using (var input = new StreamReader(typeof(Program).Assembly.GetManifestResourceStream(typeof(Program).Namespace + ".Input.txt")))
			{
				var parser = new Regex(@"^(?<ProgramID>\d+) \<-\> (?<PeerID>\d+)(, (?<PeerID>\d+))*$");

				while (true)
				{
					string line = input.ReadLine();

					if (string.IsNullOrWhiteSpace(line))
						break;

					var parse = parser.Match(line);

					if (!parse.Success)
						throw new Exception("Bad parse: " + line);

					int programID = int.Parse(parse.Groups["ProgramID"].Value);
					int[] peerIDs = parse.Groups["PeerID"].Captures.OfType<Capture>().Select(capture => int.Parse(capture.Value)).ToArray();

					connections[programID] = peerIDs;
				}
			}

			var reachable = new HashSet<int>();
			var queue = new Queue<int>();

			queue.Enqueue(0);

			while (queue.Count > 0)
			{
				int programID = queue.Dequeue();

				if (reachable.Add(programID))
					foreach (var peerID in connections[programID])
						queue.Enqueue(peerID);
			}

			Console.WriteLine(reachable.Count);
		}
	}
}
