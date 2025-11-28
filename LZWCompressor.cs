using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class LZWCompressor
{
    private const int MAX_DICT_SIZE = 4096;
    public static void Compress(string inputFile, string outputFile)
    {
        byte[] inputData = File.ReadAllBytes(inputFile);

        using (FileStream fs = new FileStream(outputFile, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            Dictionary<List<byte>, int> dictionary = new Dictionary<List<byte>, int>(new ByteListComparer());
            for (int i = 0; i < 256; i++)
                dictionary[new List<byte> { (byte)i }] = i;

            List<byte> current = new List<byte>();
            List<int> output = new List<int>();
            int nextCode = 256;

            foreach (byte b in inputData)
            {
                List<byte> extended = new List<byte>(current) { b };

                if (dictionary.ContainsKey(extended))
                {
                    current = extended;
                }
                else
                {
                    output.Add(dictionary[current]);

                    if (nextCode < 4096)
                    {
                        dictionary[extended] = nextCode++;
                    }

                    current = new List<byte> { b };
                }
            }

            if (current.Count > 0)
            {
                output.Add(dictionary[current]);
            }

            writer.Write(output.Count);
            foreach (int code in output)
            {
                writer.Write((ushort)code); // Using 2 bytes per code
            }
        }
    }

    public static void Decompress(string inputFile, string outputFile)
    {
        using (FileStream fs = new FileStream(inputFile, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(fs))
        using (FileStream outputFs = new FileStream(outputFile, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(outputFs))
        {
            int codeCount = reader.ReadInt32();

            Dictionary<int, List<byte>> dictionary = new Dictionary<int, List<byte>>();
            for (int i = 0; i < 256; i++)
            {
                dictionary[i] = new List<byte> { (byte)i };
            }

            List<ushort> compressedCodes = new List<ushort>();
            for (int i = 0; i < codeCount; i++)
            {
                compressedCodes.Add(reader.ReadUInt16());
            }

            int nextCode = 256;
            List<byte> previous = dictionary[compressedCodes[0]];
            writer.Write(previous.ToArray());

            for (int i = 1; i < compressedCodes.Count; i++)
            {
                ushort code = compressedCodes[i];
                List<byte> current;

                if (dictionary.ContainsKey(code))
                {
                    current = dictionary[code];
                }
                else if (code == nextCode)
                {
                    current = [.. previous, previous[0]];
                }
                else
                {
                    throw new InvalidDataException($"Invalid LZW code: {code}");
                }

                writer.Write(current.ToArray());

                if (nextCode < MAX_DICT_SIZE)
                {
                    List<byte> newEntry = new List<byte>(previous);
                    newEntry.Add(current[0]);
                    dictionary[nextCode] = newEntry;
                    nextCode++;
                }

                previous = current;
            }
        }
    }

    private class ByteListComparer : IEqualityComparer<List<byte>>
    {
        public bool Equals(List<byte> x, List<byte> y)
        {
            if (x == null || y == null) return false;
            if (x.Count != y.Count) return false;
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] != y[i]) return false;
            }
            return true;
        }

        public int GetHashCode(List<byte> obj)
        {
            int hash = 17;
            foreach (byte b in obj)
            {
                hash = hash * 31 + b;
            }
            return hash;
        }
    }
}