using System.Security.Cryptography;
using System.Text;

namespace SmartHighlineTool;

public sealed class EepromResult
{
    public bool IsValid { get; init; }
    public string Error { get; init; } = "";

    public string? RadioId { get; init; }

    public string PinA { get; init; } = "";
    public string PinB { get; init; } = "";

    public bool PinsMatch => PinA == PinB && PinA.Length == 4;

    public byte Status03F0 { get; init; }
    public byte Status03F8 { get; init; }
    public byte Status03F9 { get; init; }

    public bool PossibleBlocked => Status03F9 == 0x01;

    public string Sha256 { get; init; } = "";
}

public static class EepromAnalyzer
{
    public const int ExpectedSize = 16384;

    public static EepromResult Analyze(byte[] data)
    {
        if (data.Length != ExpectedSize)
        {
            return new EepromResult
            {
                IsValid = false,
                Error = $"Invalid size: {data.Length:N0} bytes. Expected 16,384 bytes."
            };
        }

        string pinA = DecodePin(
            data,
            0x03E0,
            new byte[] { 0x03, 0x07, 0x08, 0x09 });

        string pinB = DecodePin(
            data,
            0x03E8,
            new byte[] { 0x01, 0x06, 0x04, 0x02 });

        return new EepromResult
        {
            IsValid = true,
            RadioId = FindRadioId(data),

            PinA = pinA,
            PinB = pinB,

            Status03F0 = data[0x03F0],
            Status03F8 = data[0x03F8],
            Status03F9 = data[0x03F9],

            Sha256 = Convert.ToHexString(
                SHA256.HashData(data)).ToLowerInvariant()
        };
    }

    private static string DecodePin(
        byte[] data,
        int offset,
        byte[] xorMask)
    {
        var digits = new int[4];

        for (int i = 0; i < 4; i++)
        {
            digits[i] = data[offset + i] ^ xorMask[i];

            if (digits[i] is < 0 or > 9)
                return "????";
        }

        return string.Concat(digits);
    }

    private static string? FindRadioId(byte[] data)
    {
        string text = Encoding.ASCII.GetString(data);

        for (int i = 0; i <= text.Length - 10; i++)
        {
            string candidate = text.Substring(i, 10);

            if (candidate.All(char.IsDigit) &&
                (candidate.StartsWith("7640") ||
                 candidate.StartsWith("7649")))
            {
                return candidate;
            }
        }

        return null;
    }

    public static byte[] CreateResetDump(byte[] original)
    {
        if (original.Length != ExpectedSize)
            throw new InvalidDataException("EEPROM must be exactly 16,384 bytes.");

        // Always work on a COPY.
        byte[] reset = (byte[])original.Clone();

        reset[0x03F0] = 0x00;
        reset[0x03F8] = 0x00;
        reset[0x03F9] = 0x00;

        // Verify PIN hasn't changed.
        var before = Analyze(original);
        var after = Analyze(reset);

        if (before.PinA != after.PinA ||
            before.PinB != after.PinB)
        {
            throw new InvalidOperationException(
                "Safety check failed: reset operation changed PIN data.");
        }

        // Verify ONLY the expected three bytes changed.
        for (int i = 0; i < original.Length; i++)
        {
            if (original[i] == reset[i])
                continue;

            if (i != 0x03F0 &&
                i != 0x03F8 &&
                i != 0x03F9)
            {
                throw new InvalidOperationException(
                    $"Unexpected modification at 0x{i:X4}.");
            }
        }

        return reset;
    }
}