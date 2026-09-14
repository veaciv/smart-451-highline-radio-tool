# Smart 451 Highline Radio Tool

A lightweight Windows utility for analyzing **Bosch Highline radios used in the Smart Fortwo 451** with an **M95128 EEPROM**.

The tool can recover the 4-digit radio security code from a valid EEPROM dump, verify the redundant stored PIN data, inspect known lock/error-state bytes, and generate a reset EEPROM file when required.

This project contains no Bosch, Mercedes-Benz, or Smart firmware, software, EEPROM images, cryptographic material, or proprietary binaries. The application operates only on EEPROM data independently supplied by the user.

The project is intended solely for diagnosis, maintenance, and repair of equipment owned by the user or that the user is authorized to service.

Built by **Veaci**.

[[`Smart451HighlineRadioTool-v1.2-win-x64.zip`][(../../[bin/Release/Smart451HighlineRadioTool-v1.2-win-x64.zip](https://github.com/veaciv/smart-451-highline-radio-tool/blob/main/bin/Release/Smart451HighlineRadioTool-v1.2-win-x64.zip))]](https://github.com/veaciv/smart-451-highline-radio-tool/tree/main/bin/Release)

---

## Features

- Open or drag-and-drop an M95128 EEPROM dump
- Validate the expected 16 KB EEPROM size
- Detect Bosch/Smart radio identifiers when available
- Decode the 4-digit radio security PIN
- Independently decode both stored PIN copies
- Verify both PIN copies match before presenting the code as valid
- Inspect known lock/error-state bytes
- Flag a possible blocked/error condition
- Generate a separate reset EEPROM dump
- Preserve the original EEPROM file
- Verify that PIN data remains unchanged after reset
- SHA-256 identification of loaded dumps
- Advanced view showing raw EEPROM values and offsets
- Fully offline
- No installation required when using the self-contained Windows release

---

## Supported Hardware

The tool is currently intended for:

- Smart Fortwo 451
- Bosch Smart Highline / Navigation radio
- M95128 SPI EEPROM
- Known Bosch Highline radio variants using the same EEPROM structure

One specifically tested unit:

- Smart part number: `A 451 906 94 00`
- Bosch part number: `7 640 082 110 001`
- Bosch identifier: `7640082110`

### Important

This project is **not a universal Bosch radio decoder**.

Do not assume compatibility with another radio simply because it contains an M95128 EEPROM.

---

## EEPROM Requirements

The expected EEPROM dump size is:

`16,384 bytes`

The application will reject files that do not match the expected size.

A proper EEPROM dump should always be read multiple times before analysis.

Recommended procedure:

1. Read the EEPROM.
2. Save the dump.
3. Read it again.
4. Compare both files byte-for-byte.
5. Only use a dump when repeated reads are identical.
6. Keep the verified original dump permanently.

Never experiment on your only EEPROM backup.

---

## Radio Code Recovery

During reverse engineering, several EEPROM dumps with already-known radio codes were compared.

Known reference examples included:

| Reference | Confirmed PIN |
|---|---:|
| A | `0990` |
| B | `1633` |
| C | `4038` |
| Tested Smart 451 | `7354` |

The PIN is stored in two independently encoded locations in the EEPROM.

### PIN Copy A

Location:

`0x03E0 - 0x03E3`

XOR mask:

`03 07 08 09`

Each stored byte is XORed with its corresponding mask byte to recover one PIN digit.

### PIN Copy B

Location:

`0x03E8 - 0x03EB`

XOR mask:

`01 06 04 02`

The second copy independently produces the same four-digit PIN.

The application only reports a PIN as verified when:

`PIN Copy A == PIN Copy B`

If the two copies do not match, the application will warn the user instead of presenting the result as a valid radio code.

---

## Example

Example EEPROM values:

### Copy A

Raw:

`04 04 0D 0D`

Decode:

`04 XOR 03 = 07`

`04 XOR 07 = 03`

`0D XOR 08 = 05`

`0D XOR 09 = 04`

Result:

`7354`

### Copy B

Raw:

`06 05 01 06`

Decode:

`06 XOR 01 = 07`

`05 XOR 06 = 03`

`01 XOR 04 = 05`

`06 XOR 02 = 04`

Result:

`7354`

Both independent copies produce the same PIN.

---

## Blocked / Error State

Some Smart Highline radios may become blocked after repeated incorrect PIN attempts.

Comparison of original blocked EEPROM dumps with known working reset dumps identified several relevant bytes in the EEPROM state area.

Known reset pattern:

| Offset | Reset value |
|---|---:|
| `0x03F0` | `00` |
| `0x03F8` | `00` |
| `0x03F9` | `00` |

In known blocked examples, `0x03F9 = 01` has correlated with a blocked/error condition.

### Important

The exact meaning of every bit and every possible state in this area has **not been fully reverse engineered**.

For this reason the application reports:

**Possible blocked / error state**

rather than claiming with certainty that every EEPROM containing a particular value is blocked.

---

## Reset Function

The application can create a reset EEPROM dump.

It does **not** overwrite the file you opened.

Instead, it creates a separate file such as:

`radio.reset.bin`

The known reset procedure clears:

`0x03F0`

`0x03F8`

`0x03F9`

to:

`00`

Before exporting the reset dump, the application verifies that:

- EEPROM size remains unchanged
- PIN Copy A remains unchanged
- PIN Copy B remains unchanged
- Both PIN copies still match
- Only the expected reset bytes were modified

The original EEPROM dump should always be retained.

---

## Recommended Recovery Procedure

If you have a locked Smart Highline radio:

1. Remove the radio.
2. Identify the M95128 EEPROM.
3. Read it using a suitable SPI EEPROM programmer.
4. Make multiple identical reads.
5. Save the original dump somewhere safe.
6. Open the verified dump with Smart 451 Highline Radio Tool.
7. Record the recovered PIN.
8. Check the reported lock/error status.
9. If required, generate a reset dump.
10. Program the reset dump back to the EEPROM.
11. Read the EEPROM again after programming and verify the written data.
12. Reinstall the radio.
13. Enter the recovered PIN.

Do not repeatedly guess radio codes. Some units limit the number of incorrect attempts.

---

## CH341A Note

A CH341A can be used to read the M95128 EEPROM, but users should verify:

- correct EEPROM selection
- correct SOIC-8 orientation
- correct supply voltage
- pin 1 orientation
- stable and repeatable EEPROM reads

In-circuit reads may not always work correctly because other components remain connected to the SPI bus.

If repeated reads differ, stop and correct the read setup before using the dump.

---

## Build Requirements

The project is written in:

- C#
- Windows Forms
- .NET 10

Recommended development environment:

- Visual Studio 2022 or newer
- .NET Desktop Development workload

---

## Building

Open the solution in Visual Studio and build in Release mode.

For a self-contained Windows x64 build:

`dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true`

The resulting executable can be distributed without requiring the user to separately install .NET.

---

## Releases

Compiled Windows builds should be downloaded from the project's **GitHub Releases** section.

For transparency, source code is publicly available in this repository.

When possible, SHA-256 hashes should be published alongside release binaries.

---

## Safety

EEPROM programming can permanently alter radio configuration if incorrect data is written.

Always:

- Keep the original EEPROM dump
- Verify repeated reads
- Verify the correct EEPROM type
- Verify file size
- Never write a dump from an unrelated radio
- Never overwrite your only backup
- Read the EEPROM again after programming
- Compare the programmed EEPROM against the intended file

If you are unsure, do not write to the EEPROM.

---

## Disclaimer

This software is provided for educational, diagnostic, repair, and recovery purposes.

It is intended for use only with equipment that you own or are authorized to service.

The project is based on independent reverse engineering and comparison of EEPROM data from known Smart/Bosch Highline radio examples.

Compatibility with every hardware revision, firmware version, regional variant, or EEPROM configuration is **not guaranteed**.

The authors and contributors make no warranty that the software is error-free or suitable for any particular purpose.

Use of this software, EEPROM programming, radio modification, and any resulting damage or data loss are entirely at the user's own risk.

The author is not responsible for:

- damaged EEPROMs
- damaged radios
- corrupted configuration data
- incorrect programming
- locked radios
- lost data
- vehicle electrical damage
- incompatibility with unsupported hardware
- any direct or indirect damages resulting from use of this software

Always retain an untouched backup of the original EEPROM before making any modification.

This project is **not affiliated with, authorized by, sponsored by, or endorsed by Bosch, Mercedes-Benz, Smart, or any of their subsidiaries or affiliates**.

All product names, trademarks, and registered trademarks belong to their respective owners.

---

## Security / Ownership

This project is not intended to facilitate access to stolen equipment.

Only use the tool on radios and vehicles you own or have explicit authorization to repair or service.

---

## Contributing

Contributions are welcome.

Useful contributions include:

- Additional confirmed Smart Highline EEPROM + PIN reference pairs
- Additional confirmed blocked/reset EEPROM comparisons
- Support for additional verified Bosch Highline variants
- Improved radio identification
- Bug reports
- UI improvements
- Documentation improvements

When submitting EEPROM research, remove or redact personally identifying information when appropriate.

Do not submit EEPROM dumps from equipment you are not authorized to analyze.

---

## Known Limitations

Current limitations include:

- Only M95128-based Smart Highline EEPROM layouts are supported
- Not every Bosch Highline hardware/software revision has been tested
- Block/error-state detection is based on confirmed EEPROM comparisons but is not yet a complete reverse engineering of every possible status value
- EEPROM reading and writing must currently be performed using separate programmer software
- The application does not communicate directly with CH341A hardware

Future releases may expand support as additional verified reference dumps become available.

---

## License

Released under the **MIT License**.

See the `LICENSE` file for details.

---

## Author

**Veaci**

Smart 451 Highline Radio Tool

© 2026 Veaci
