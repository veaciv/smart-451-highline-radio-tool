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

    public bool PinsMatch =>
        PinA == PinB &&
        PinA.Length == 4 &&
        PinA.All(char.IsDigit);

    public byte Counter { get; init; }

    // Research / state bytes.
    public byte Status03F8 { get; init; }
    public byte Status03F9 { get; init; }

    public string Sha256 { get; init; } = "";
}

public static class EepromAnalyzer
{
    public const int ExpectedSize = 16384;

    public const int PinAOffset = 0x03E0;
    public const int PinBOffset = 0x03E8;

    public const int CounterOffset = 0x03F0;

    // Martech code-change output resets this byte to 00.
    public const int PinChangeStateOffset = 0x03F8;

    private static readonly byte[] PinMaskA =
    {
        0x03, 0x07, 0x08, 0x09
    };

    private static readonly byte[] PinMaskB =
    {
        0x01, 0x06, 0x04, 0x02
    };

    public static EepromResult Analyze(byte[] data)
    {
        if (data.Length != ExpectedSize)
        {
            return new EepromResult
            {
                IsValid = false,
                Error =
                    $"Invalid size: {data.Length:N0} bytes. " +
                    "Expected 16,384 bytes."
            };
        }

        string pinA = DecodePin(
            data,
            PinAOffset,
            PinMaskA);

        string pinB = DecodePin(
            data,
            PinBOffset,
            PinMaskB);

        return new EepromResult
        {
            IsValid = true,

            RadioId = FindRadioId(data),

            PinA = pinA,
            PinB = pinB,

            Counter = data[CounterOffset],

            Status03F8 = data[0x03F8],
            Status03F9 = data[0x03F9],

            Sha256 = Convert.ToHexString(
                SHA256.HashData(data)
            ).ToLowerInvariant()
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
            digits[i] =
                data[offset + i] ^ xorMask[i];

            if (digits[i] < 0 || digits[i] > 9)
                return "????";
        }

        return string.Concat(digits);
    }

    private static string? FindRadioId(byte[] data)
    {
        string text =
            Encoding.ASCII.GetString(data);

        for (int i = 0;
             i <= text.Length - 10;
             i++)
        {
            string candidate =
                text.Substring(i, 10);

            if (candidate.All(char.IsDigit) &&
                (
                    candidate.StartsWith("7640") ||
                    candidate.StartsWith("7649")
                ))
            {
                return candidate;
            }
        }

        return null;
    }

    //
    // COUNTER RESET
    //

    public static byte[] CreateCounterResetDump(
        byte[] original)
    {
        ValidateOriginal(original);

        EepromResult before =
            Analyze(original);

        if (!before.PinsMatch)
        {
            throw new InvalidOperationException(
                "PIN copies do not match. " +
                "Counter reset has been refused.");
        }

        byte[] reset =
            (byte[])original.Clone();

        reset[CounterOffset] = 0x00;

        EepromResult after =
            Analyze(reset);

        if (before.PinA != after.PinA ||
            before.PinB != after.PinB)
        {
            throw new InvalidOperationException(
                "Counter reset unexpectedly changed PIN data.");
        }

        if (!after.PinsMatch)
        {
            throw new InvalidOperationException(
                "PIN copies no longer match.");
        }

        VerifyOnlyAllowedOffsetsChanged(
            original,
            reset,
            new[]
            {
                CounterOffset
            });

        if (reset[CounterOffset] != 0x00)
        {
            throw new InvalidOperationException(
                "Counter reset verification failed.");
        }

        return reset;
    }

    //
    // PIN CHANGE
    //

    public static byte[] CreatePinChangeDump(
        byte[] original,
        string newPin)
    {
        ValidateOriginal(original);

        if (newPin is null ||
            newPin.Length != 4 ||
            !newPin.All(char.IsDigit))
        {
            throw new ArgumentException(
                "PIN must contain exactly four digits.",
                nameof(newPin));
        }

        EepromResult before =
            Analyze(original);

        if (!before.PinsMatch)
        {
            throw new InvalidOperationException(
                "Existing PIN copies do not match. " +
                "PIN change has been refused.");
        }

        if (before.PinA == newPin)
        {
            throw new InvalidOperationException(
                "The requested PIN is already stored in this EEPROM.");
        }

        byte[] modified =
            (byte[])original.Clone();

        //
        // Encode PIN Copy A + B.
        //

        for (int i = 0; i < 4; i++)
        {
            int digit =
                newPin[i] - '0';

            modified[PinAOffset + i] =
                (byte)(digit ^ PinMaskA[i]);

            modified[PinBOffset + i] =
                (byte)(digit ^ PinMaskB[i]);
        }

        //
        // Martech-generated code-change example
        // resets 0x03F8 to 00.
        //
        // Important:
        // this does NOT reset the attempt counter at 0x03F0.
        //

        modified[PinChangeStateOffset] = 0x00;

        //
        // Verify resulting EEPROM decodes back
        // to exactly the requested PIN.
        //

        EepromResult after =
            Analyze(modified);

        if (!after.PinsMatch)
        {
            throw new InvalidOperationException(
                "Generated PIN copies do not match.");
        }

        if (after.PinA != newPin ||
            after.PinB != newPin)
        {
            throw new InvalidOperationException(
                "Generated EEPROM does not decode " +
                "to the requested PIN.");
        }

        //
        // Counter MUST remain unchanged.
        //

        if (after.Counter != before.Counter)
        {
            throw new InvalidOperationException(
                "PIN change unexpectedly modified " +
                "the attempt counter.");
        }

        //
        // Only these locations may change:
        //
        // 03E0–03E3
        // 03E8–03EB
        // 03F8
        //

        var allowedOffsets =
            new HashSet<int>();

        for (int i = 0; i < 4; i++)
        {
            allowedOffsets.Add(
                PinAOffset + i);

            allowedOffsets.Add(
                PinBOffset + i);
        }

        allowedOffsets.Add(
            PinChangeStateOffset);

        VerifyOnlyAllowedOffsetsChanged(
            original,
            modified,
            allowedOffsets);

        return modified;
    }

    //
    // HELPERS
    //

    private static void ValidateOriginal(
        byte[] original)
    {
        if (original.Length != ExpectedSize)
        {
            throw new InvalidDataException(
                "EEPROM must be exactly 16,384 bytes.");
        }
    }

    private static void VerifyOnlyAllowedOffsetsChanged(
        byte[] original,
        byte[] modified,
        IEnumerable<int> allowedOffsets)
    {
        var allowed =
            allowedOffsets.ToHashSet();

        for (int i = 0;
             i < original.Length;
             i++)
        {
            if (original[i] == modified[i])
                continue;

            if (!allowed.Contains(i))
            {
                throw new InvalidOperationException(
                    $"Unexpected modification at 0x{i:X4}.");
            }
        }
    }
}