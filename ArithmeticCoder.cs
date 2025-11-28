using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class ArithmeticCoder
{
    private const int PRECISION = 32;
    private const ulong MAX_RANGE = 1UL << PRECISION;
    private const ulong HALF_RANGE = MAX_RANGE >> 1;
    private const ulong QUARTER_RANGE = HALF_RANGE >> 1;
    private const ulong THREE_QUARTER_RANGE = HALF_RANGE + QUARTER_RANGE;

    public class SymbolStats
    {
        public Dictionary<byte, ulong> Frequencies { get; } = new Dictionary<byte, ulong>();
        public ulong Total { get; set; }
        public Dictionary<byte, (ulong low, ulong high)> Ranges { get; } = new Dictionary<byte, (ulong, ulong)>();
    }

    public static void EncodeFile(string inputFile, string outputFile)
    {
        byte[] inputData = File.ReadAllBytes(inputFile);
        byte[] encodedData = Encode(inputData);
        File.WriteAllBytes(outputFile, encodedData);
    }

    public static void DecodeFile(string inputFile, string outputFile)
    {
        byte[] inputData = File.ReadAllBytes(inputFile);
        byte[] decodedData = Decode(inputData);
        File.WriteAllBytes(outputFile, decodedData);
    }

    public static byte[] Encode(byte[] input)
    {
        if (input == null || input.Length == 0)
            return Array.Empty<byte>();

        var stats = CalculateStatistics(input);
        BuildRanges(stats);

        using var outputStream = new MemoryStream();
        using var writer = new BinaryWriter(outputStream);

        // Записываем размер оригинальных данных
        writer.Write(input.Length);

        // Записываем статистику символов
        writer.Write(stats.Frequencies.Count);
        foreach (var pair in stats.Frequencies)
        {
            writer.Write(pair.Key);
            writer.Write(pair.Value);
        }

        ulong low = 0;
        ulong high = MAX_RANGE - 1;
        int underflow = 0;

        foreach (byte symbol in input)
        {
            var range = high - low + 1;
            var symbolRange = stats.Ranges[symbol];

            high = low + (range * symbolRange.high) / stats.Total - 1;
            low = low + (range * symbolRange.low) / stats.Total;

            while (true)
            {
                if (high < HALF_RANGE)
                {
                    WriteBit(writer, 0);
                    while (underflow > 0)
                    {
                        WriteBit(writer, 1);
                        underflow--;
                    }
                }
                else if (low >= HALF_RANGE)
                {
                    WriteBit(writer, 1);
                    while (underflow > 0)
                    {
                        WriteBit(writer, 0);
                        underflow--;
                    }
                    low -= HALF_RANGE;
                    high -= HALF_RANGE;
                }
                else if (low >= QUARTER_RANGE && high < THREE_QUARTER_RANGE)
                {
                    underflow++;
                    low -= QUARTER_RANGE;
                    high -= QUARTER_RANGE;
                }
                else
                {
                    break;
                }

                low <<= 1;
                high = (high << 1) | 1;
            }
        }

        // Завершаем кодирование
        underflow++;
        if (low < QUARTER_RANGE)
        {
            WriteBit(writer, 0);
            for (int i = 0; i < underflow; i++)
                WriteBit(writer, 1);
        }
        else
        {
            WriteBit(writer, 1);
            for (int i = 0; i < underflow; i++)
                WriteBit(writer, 0);
        }

        FlushBits(writer);
        return outputStream.ToArray();
    }

    public static byte[] Decode(byte[] encoded)
    {
        if (encoded == null || encoded.Length == 0)
            return Array.Empty<byte>();

        using var inputStream = new MemoryStream(encoded);
        using var reader = new BinaryReader(inputStream);

        // Читаем размер оригинальных данных
        int originalSize = reader.ReadInt32();

        // Читаем статистику символов
        int symbolCount = reader.ReadInt32();
        var stats = new SymbolStats();

        for (int i = 0; i < symbolCount; i++)
        {
            byte symbol = reader.ReadByte();
            ulong frequency = reader.ReadUInt64();
            stats.Frequencies[symbol] = frequency;
            stats.Total += frequency;
        }

        BuildRanges(stats);

        // Инициализируем декодер
        ulong value = 0;
        for (int i = 0; i < PRECISION; i++)
        {
            value = (value << 1) | (ulong )ReadBit(reader);
        }

        ulong low = 0;
        ulong high = MAX_RANGE - 1;

        var output = new List<byte>();

        for (int i = 0; i < originalSize; i++)
        {
            var range = high - low + 1;
            var scaledValue = ((value - low + 1) * stats.Total - 1) / range;

            // Находим символ по scaledValue
            byte symbol = FindSymbol(stats, scaledValue);
            output.Add(symbol);

            var symbolRange = stats.Ranges[symbol];
            high = low + (range * symbolRange.high) / stats.Total - 1;
            low = low + (range * symbolRange.low) / stats.Total;

            while (true)
            {
                if (high < HALF_RANGE)
                {
                    // Ничего не делаем
                }
                else if (low >= HALF_RANGE)
                {
                    value -= HALF_RANGE;
                    low -= HALF_RANGE;
                    high -= HALF_RANGE;
                }
                else if (low >= QUARTER_RANGE && high < THREE_QUARTER_RANGE)
                {
                    value -= QUARTER_RANGE;
                    low -= QUARTER_RANGE;
                    high -= QUARTER_RANGE;
                }
                else
                {
                    break;
                }

                low <<= 1;
                high = (high << 1) | 1;
                value = (value << 1) | (ulong) ReadBit(reader);
            }
        }

        return output.ToArray();
    }

    private static SymbolStats CalculateStatistics(byte[] data)
    {
        var stats = new SymbolStats();

        foreach (byte b in data)
        {
            if (!stats.Frequencies.ContainsKey(b))
                stats.Frequencies[b] = 0;
            stats.Frequencies[b]++;
            stats.Total++;
        }

        // Гарантируем, что все символы имеют хотя бы частоту 1
        for (int i = 0; i < 256; i++)
        {
            byte symbol = (byte)i;
            if (!stats.Frequencies.ContainsKey(symbol))
            {
                stats.Frequencies[symbol] = 1;
                stats.Total++;
            }
        }

        return stats;
    }

    private static void BuildRanges(SymbolStats stats)
    {
        ulong cumulative = 0;
        foreach (var pair in stats.Frequencies.OrderBy(x => x.Key))
        {
            stats.Ranges[pair.Key] = (cumulative, cumulative + pair.Value);
            cumulative += pair.Value;
        }
    }

    private static byte FindSymbol(SymbolStats stats, ulong value)
    {
        foreach (var pair in stats.Ranges)
        {
            if (value >= pair.Value.low && value < pair.Value.high)
                return pair.Key;
        }
        throw new InvalidOperationException("Symbol not found");
    }

    // Вспомогательные методы для работы с битами
    private static byte currentBit;
    private static int bitPosition;
    private static byte bitBuffer;

    private static void WriteBit(BinaryWriter writer, int bit)
    {
        bitBuffer = (byte)((bitBuffer << 1) | (bit & 1));
        bitPosition++;

        if (bitPosition == 8)
        {
            writer.Write(bitBuffer);
            bitBuffer = 0;
            bitPosition = 0;
        }
    }

    private static void FlushBits(BinaryWriter writer)
    {
        if (bitPosition > 0)
        {
            bitBuffer <<= (8 - bitPosition);
            writer.Write(bitBuffer);
            bitBuffer = 0;
            bitPosition = 0;
        }
    }

    private static int ReadBit(BinaryReader reader)
    {
        if (bitPosition == 0)
        {
            if (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                bitBuffer = reader.ReadByte();
                bitPosition = 8;
            }
            else
            {
                return 0; // Достигнут конец потока
            }
        }

        int bit = (bitBuffer >> (bitPosition - 1)) & 1;
        bitPosition--;
        return bit;
    }
}