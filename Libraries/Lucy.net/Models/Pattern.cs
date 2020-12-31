using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Lucy
{
	/// <summary>
	/// Represents a pattern which is a string, or array of strings
	/// </summary>
	[JsonConverter(typeof(PatternConverter))]
	public class Pattern : IEnumerable<string>, IYamlConvertible
	{
		private List<string> patterns = new List<string>();

		public Pattern()
		{
		}

		public Pattern(string patternDefinition)
		{
			this.patterns.Add(patternDefinition.Trim());
		}

		public Pattern(string[] patternDefinitions)
		{
			if (patternDefinitions != null && patternDefinitions.Any())
			{
				this.patterns.AddRange(patternDefinitions.Select(pattern => pattern.Trim()));
			}
		}

		public IEnumerator<string> GetEnumerator() => this.patterns.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.patterns).GetEnumerator();

		public static implicit operator Pattern(string patternDefinition) => new Pattern(patternDefinition);
		public static implicit operator Pattern(JValue patternDefinition) => new Pattern((string)patternDefinition);

		public static implicit operator Pattern(string[] patternDefinitions) => new Pattern(patternDefinitions);
		public static implicit operator Pattern(JArray patternDefinitions) => new Pattern(patternDefinitions.ToObject<string[]>());

		public override string ToString() => $"[{this.patterns.FirstOrDefault()}, ...]";

		public void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			if (parser.TryConsume<Scalar>(out var scalar))
			{
				this.patterns.Add(scalar.Value);
				return;
			}
			else if (parser.TryConsume<SequenceStart>(out var seq))
			{
				while (parser.TryConsume<Scalar>(out var member))
				{
					this.patterns.Add(member.Value);
				}
				parser.Consume<SequenceEnd>();
			}
		}

		public void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			if (patterns.Count == 1)
			{
				emitter.Emit(new Scalar(patterns.First()));
			}
			else
			{
				emitter.Emit(new SequenceStart(null, null, true, SequenceStyle.Flow));
				foreach (var pattern in patterns)
				{
					emitter.Emit(new Scalar(pattern));
				}
				emitter.Emit(new SequenceEnd());
			}
		}
	}
}
