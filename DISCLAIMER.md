# Disclaimer

Smart 451 Highline Radio Tool is an independent, unofficial software project
intended for educational, diagnostic, research, repair, and recovery purposes.

It is intended only for equipment that the user owns or is explicitly
authorized to diagnose, repair, modify, or service.

## No Affiliation

This project is not affiliated with, authorized by, sponsored by, approved by,
or endorsed by Bosch, Mercedes-Benz, smart, or any of their parent companies,
subsidiaries, affiliates, distributors, or dealers.

All trademarks, product names, part numbers, and registered marks remain the
property of their respective owners. References to them are made only for
identification and compatibility purposes.

## Experimental Software

The EEPROM structures, PIN decoding methods, status-byte interpretations, and
reset procedures implemented by this software were independently derived from
analysis and comparison of known EEPROM samples.

Not every hardware revision, software revision, firmware version, regional
variant, EEPROM layout, or radio configuration has been tested.

A result that appears valid does not guarantee compatibility with a particular
radio.

Lock/error-state detection and EEPROM reset behavior should be considered
experimental unless independently verified for the specific radio being
serviced.

## Risk of EEPROM Modification

Reading, writing, modifying, or programming EEPROM data can result in data
corruption, loss of configuration, an unusable radio, or other unintended
behavior.

Users must create and retain a verified, untouched backup of the original
EEPROM before making any modification.

Multiple EEPROM reads should be compared before relying on a dump.

This software does not program EEPROM hardware directly. Any EEPROM programming
is performed independently by the user using third-party hardware and software.

The user is solely responsible for verifying:

- the correct radio model;
- the correct EEPROM type;
- EEPROM orientation and electrical connections;
- programmer voltage and configuration;
- dump integrity;
- compatibility of any generated file;
- successful verification after programming.

## No Warranty

THE SOFTWARE IS PROVIDED "AS IS" AND "AS AVAILABLE".

TO THE MAXIMUM EXTENT PERMITTED BY APPLICABLE LAW, NO REPRESENTATION OR WARRANTY
IS MADE REGARDING THE SOFTWARE, INCLUDING ITS ACCURACY, RELIABILITY,
COMPATIBILITY, COMPLETENESS, FITNESS FOR A PARTICULAR PURPOSE, OR ABILITY TO
RECOVER, RESET, REPAIR, OR UNLOCK ANY DEVICE.

USE OF THE SOFTWARE AND ANY INFORMATION PRODUCED BY THE SOFTWARE IS AT THE
USER'S OWN RISK.

## Limitation of Liability

TO THE MAXIMUM EXTENT PERMITTED BY APPLICABLE LAW, THE AUTHOR, COPYRIGHT HOLDER,
AND CONTRIBUTORS SHALL NOT BE LIABLE FOR ANY LOSS, DAMAGE, CLAIM, COST, OR
EXPENSE ARISING FROM OR RELATED TO THE SOFTWARE OR ITS USE, INCLUDING, WITHOUT
LIMITATION:

- corrupted EEPROM data;
- damaged or unusable radios;
- damaged electronic components;
- vehicle electrical damage;
- loss of configuration or stored data;
- incorrect PIN or status results;
- programming errors;
- incompatibility with unsupported hardware;
- loss of use;
- loss of profits;
- consequential, incidental, indirect, or special damages.

NOTHING IN THIS DISCLAIMER EXCLUDES OR LIMITS LIABILITY WHERE SUCH EXCLUSION OR
LIMITATION IS PROHIBITED BY APPLICABLE LAW.

## Authorization and Lawful Use

The user is responsible for ensuring that their possession, access, analysis,
modification, and use of the equipment and EEPROM data are lawful and
authorized.

The project is not intended to facilitate unauthorized access to equipment,
theft, circumvention involving unlawfully obtained equipment, or any other
unlawful activity.

The author does not verify ownership or authorization and assumes no
responsibility for misuse by third parties.

## Backups

Never write modified EEPROM data without first retaining a verified copy of the
original EEPROM.

The original dump should be stored separately and never overwritten.

## Third-Party Tools

References to EEPROM programmers, software, hardware, or other third-party
products are provided for informational purposes only.

The author does not control and is not responsible for third-party products,
their operation, safety, accuracy, or compatibility.

## Acceptance of Risk

By using this software, the user acknowledges that automotive electronic
diagnosis and EEPROM modification carry inherent risks and accepts
responsibility for evaluating whether the software and any generated output are
appropriate for their particular equipment.

Where applicable law does not permit a particular exclusion or limitation, that
exclusion or limitation applies only to the maximum extent permitted by law.

---

Copyright © 2026 Veaci
