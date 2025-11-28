using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileCompressionApp
{
    public class LZWCompressor
    {
        public void Compress(string inputFile, string outputFile)
        {
            byte[] inputData = File.ReadAllBytes(inputFile);

            using (FileStream fs = new FileStream(outputFile, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                // Initialize dictionary with all possible bytes
                Dictionary<List<byte>, int> dictionary = new Dictionary<List<byte>, int>(new ByteListComparer());
                for (int i = 0; i < 256; i++)
                {
                    dictionary[new List<byte> { (byte)i }] = i;
                }

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

                        if (nextCode < 4096) // Limit dictionary size
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

                // Write compressed data
                writer.Write(output.Count);
                foreach (int code in output)
                {
                    writer.Write((ushort)code); // Using 2 bytes per code
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
}