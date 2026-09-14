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

    // Lock / error-state bytes.
    // Their individual meanings are not fully established.
    public byte State03F0 { get; init; }
    public byte State03F8 { get; init; }
    public byte State03F9 { get; init; }

    public bool ResetValuesAlreadyPresent =>
        State03F0 == 0x00 &&
        State03F8 == 0x00 &&
        State03F9 == 0x00;

    public string Sha256 { get; init; } = "";
}

public static class EepromAnalyzer
{
    public const int ExpectedSize = 16384;

    public const int PinAOffset = 0x03E0;
    public const int PinBOffset = 0x03E8;

    public const int State03F0Offset = 0x03F0;
    public const int State03F8Offset = 0x03F8;
    public const int State03F9Offset = 0x03F9;

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

            State03F0 = data[State03F0Offset],
            State03F8 = data[State03F8Offset],
            State03F9 = data[State03F9Offset],

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
            {
                return "????";
            }
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
    // LOCK / ERROR RESET
    //

    public static byte[] CreateResetDump(
        byte[] original)
    {
        ValidateOriginal(original);

        EepromResult before =
            Analyze(original);

        if (!before.PinsMatch)
        {
            throw new InvalidOperationException(
                "PIN copies do not match. " +
                "Repair/reset operation has been refused.");
        }

        byte[] reset =
            (byte[])original.Clone();

        //
        // Verified working recovery pattern:
        //
        // 03F0 -> 00
        // 03F8 -> 00
        // 03F9 -> 00
        //

        reset[State03F0Offset] = 0x00;
        reset[State03F8Offset] = 0x00;
        reset[State03F9Offset] = 0x00;

        EepromResult after =
            Analyze(reset);

        //
        // PIN data must remain untouched.
        //

        if (!after.PinsMatch ||
            after.PinA != before.PinA ||
            after.PinB != before.PinB)
        {
            throw new InvalidOperationException(
                "Safety check failed: repair/reset " +
                "operation changed PIN data.");
        }

        //
        // Verify only the expected three
        // state bytes were modified.
        //

        VerifyOnlyAllowedOffsetsChanged(
            original,
            reset,
            new[]
            {
                State03F0Offset,
                State03F8Offset,
                State03F9Offset
            });

        //
        // Final state verification.
        //

        if (after.State03F0 != 0x00 ||
            after.State03F8 != 0x00 ||
            after.State03F9 != 0x00)
        {
            throw new InvalidOperationException(
                "Repair/reset verification failed.");
        }

        return reset;
    }

    //
    // EXPERIMENTAL PIN CHANGE
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
                "The requested PIN is already stored " +
                "in this EEPROM.");
        }

        byte[] modified =
            (byte[])original.Clone();

        //
        // Encode both redundant PIN copies.
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
        // Verified code-change samples
        // clear 0x03F8.
        //
        // Do NOT modify 03F0 or 03F9 here.
        //

        modified[State03F8Offset] = 0x00;

        EepromResult after =
            Analyze(modified);

        //
        // Verify generated PIN.
        //

        if (!after.PinsMatch ||
            after.PinA != newPin ||
            after.PinB != newPin)
        {
            throw new InvalidOperationException(
                "Generated EEPROM does not decode " +
                "to the requested PIN.");
        }

        //
        // PIN change must not modify
        // 03F0 or 03F9.
        //

        if (after.State03F0 != before.State03F0 ||
            after.State03F9 != before.State03F9)
        {
            throw new InvalidOperationException(
                "PIN change unexpectedly modified " +
                "unrelated state bytes.");
        }

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
            State03F8Offset);

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
            {
                continue;
            }

            if (!allowed.Contains(i))
            {
                throw new InvalidOperationException(
                    $"Unexpected modification at 0x{i:X4}.");
            }
        }
    }
}