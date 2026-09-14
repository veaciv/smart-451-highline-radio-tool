using System.Globalization;

namespace SmartHighlineTool;

public enum AppLanguage
{
    English,
    French,
    Russian,
    Romanian,
    Spanish
}

public static class Localization
{
    private static readonly IReadOnlyDictionary<string, string> English = new Dictionary<string, string>
    {
        ["app.title"] = "Smart 451 Highline Radio Tool",
        ["header.title"] = "Smart 451 Highline",
        ["header.subtitle"] = "M95128 EEPROM diagnostic & recovery utility",
        ["drop.file"] = "Drop a 16 KB M95128 .bin file here",
        ["open.eeprom"] = "Open EEPROM",
        ["status.none"] = "No EEPROM loaded",
        ["status.loaded"] = "16 KB M95128 dump loaded",
        ["radio.id"] = "Bosch / Smart ID: {0}",
        ["radio.code"] = "RADIO CODE",
        ["pin.decode"] = "Load an EEPROM to decode",
        ["pin.match"] = "Both encoded PIN copies match",
        ["pin.mismatch"] = "PIN copies do not match - do not use this code",
        ["pin.change"] = "Change PIN (Experimental)",
        ["state.title"] = "LOCK / ERROR STATE",
        ["state.unknown"] = "Unknown",
        ["state.present"] = "Reset values already present",
        ["state.values"] = "State: {0:X2} / {1:X2} / {2:X2}",
        ["reset.create"] = "Create Repair / Reset",
        ["advanced.show"] = "Show Advanced",
        ["advanced.hide"] = "Hide Advanced",
        ["advanced.file"] = "File size:      {0:N0} bytes",
        ["advanced.sha"] = "SHA-256:        {0}",
        ["advanced.radio"] = "Radio ID:       {0}",
        ["advanced.notDetected"] = "Not detected",
        ["advanced.copyA"] = "PIN COPY A",
        ["advanced.copyB"] = "PIN COPY B",
        ["advanced.offset"] = "Offset:         0x{0:X4}",
        ["advanced.raw"] = "Raw:            {0}",
        ["advanced.decoded"] = "Decoded:        {0}",
        ["advanced.stateBytes"] = "LOCK / ERROR STATE BYTES",
        ["advanced.verified"] = "VERIFIED REPAIR / RESET",
        ["advanced.unknownMeaning"] = "Individual state-byte meanings are not fully established.",
        ["dialog.open"] = "Open M95128 EEPROM Dump",
        ["dialog.savePin"] = "Save PIN Changed EEPROM",
        ["dialog.saveReset"] = "Save Repair / Reset EEPROM",
        ["dialog.filter"] = "EEPROM binary file (*.bin)|*.bin",
        ["dialog.filterAll"] = "EEPROM binary files (*.bin)|*.bin|All files (*.*)|*.*",
        ["error.readTitle"] = "Read Error",
        ["error.read"] = "Could not read EEPROM file.\r\n\r\n{0}",
        ["error.unsupportedTitle"] = "Unsupported EEPROM",
        ["error.safety"] = "Safety Check",
        ["error.pinChangeTitle"] = "PIN Change Failed",
        ["error.pinChange"] = "PIN change dump was NOT created.\r\n\r\n{0}",
        ["error.resetTitle"] = "Safety Check Failed",
        ["error.reset"] = "Repair/reset dump was NOT created.\r\n\r\n{0}",
        ["warning.noOverwritePin"] = "The modified EEPROM cannot overwrite the original dump.",
        ["warning.noOverwriteReset"] = "The repair/reset dump cannot overwrite the original EEPROM file.",
        ["success.pinTitle"] = "PIN Change Dump Created",
        ["success.pin"] = "PIN change dump created successfully.\r\n\r\nOriginal PIN: {0}\r\nNew PIN: {1}\r\n\r\n0x03F0 remains: {2:X2}\r\n0x03F8: {3:X2} -> {4:X2}\r\n0x03F9 remains: {5:X2}\r\n\r\nSaved as:\r\n{6}\r\n\r\nThe original EEPROM file was not modified.",
        ["success.resetTitle"] = "Repair / Reset Dump Created",
        ["success.reset"] = "Repair/reset dump created successfully.\r\n\r\nRecovered PIN: {0}\r\n\r\n0x03F0: {1:X2} -> 00\r\n0x03F8: {2:X2} -> 00\r\n0x03F9: {3:X2} -> 00\r\n\r\nSaved as:\r\n{4}\r\n\r\nOnly the three verified state bytes were modified.\r\nYour original EEPROM file was not modified.",
        ["dialog.pinTitle"] = "Experimental Radio PIN Change",
        ["dialog.pinHeading"] = "Change Radio PIN",
        ["dialog.experimental"] = "Experimental EEPROM modification",
        ["dialog.pinWarning"] = "This creates a modified COPY of the EEPROM. It does not program the radio directly.\r\n\r\nThe PIN encoding has been independently verified against multiple known EEPROM/code pairs and 7354 -> 1111 example.\r\n\r\nCompatibility with every radio or firmware revision is not guaranteed. Always retain the original EEPROM.",
        ["dialog.currentPin"] = "Current PIN: {0}",
        ["dialog.newPin"] = "New PIN",
        ["dialog.confirmPin"] = "Confirm PIN",
        ["dialog.pinEnter"] = "Enter a four-digit PIN.",
        ["dialog.pinDigits"] = "PIN must contain exactly 4 digits.",
        ["dialog.pinMismatch"] = "PIN entries do not match.",
        ["dialog.pinMatch"] = "PIN entries match",
        ["dialog.authorization"] = "I confirm that I am authorized to service this equipment and accept responsibility for using the generated EEPROM file.",
        ["dialog.cancel"] = "Cancel",
        ["dialog.createPin"] = "Create PIN Change Dump",
        ["dialog.repairTitle"] = "EEPROM Repair / Reset",
        ["dialog.repairHeading"] = "EEPROM Lock / Error-State Reset",
        ["dialog.repairSubtitle"] = "Please read before creating a modified EEPROM file.",
        ["dialog.repairWarning"] = "This operation creates a modified COPY of your EEPROM dump.\r\n\r\nThe original file will NOT be modified.\r\n\r\nThe generated repair file clears three known lock/error-state bytes:\r\n\r\n0x03F0 -> 00\r\n0x03F8 -> 00\r\n0x03F9 -> 00\r\n\r\nThis three-byte reset pattern has been verified using a working Smart 451 Highline radio recovery.\r\n\r\nThe exact individual meaning of each state byte has not been fully established.\r\n\r\nCompatibility with every radio, firmware version, or EEPROM layout is not guaranteed.\r\n\r\nAlways keep a verified untouched backup of the original EEPROM and verify the EEPROM after programming.",
        ["dialog.recoveredPin"] = "Recovered PIN: {0}",
        ["dialog.stateDetails"] = "03F0: {0:X2} -> 00    03F8: {1:X2} -> 00    03F9: {2:X2} -> 00",
        ["dialog.repairAuthorization"] = "I confirm that I am authorized to service this equipment and accept responsibility for programming and using the generated EEPROM file.",
        ["dialog.createRepair"] = "Create Repair Dump"
    };

    private static readonly IReadOnlyDictionary<AppLanguage, IReadOnlyDictionary<string, string>> Translations = new Dictionary<AppLanguage, IReadOnlyDictionary<string, string>>
    {
        [AppLanguage.French] = new Dictionary<string, string> { ["app.title"] = "Outil radio Smart 451 Highline", ["header.subtitle"] = "Utilitaire de diagnostic et de récupération EEPROM M95128", ["drop.file"] = "Déposez ici un fichier .bin M95128 de 16 Ko", ["open.eeprom"] = "Ouvrir l'EEPROM", ["status.none"] = "Aucune EEPROM chargée", ["status.loaded"] = "Dump M95128 de 16 Ko chargé", ["radio.id"] = "Identifiant Bosch / Smart : {0}", ["radio.code"] = "CODE RADIO", ["pin.decode"] = "Chargez une EEPROM pour décoder", ["pin.match"] = "Les deux copies codées du PIN correspondent", ["pin.mismatch"] = "Les copies du PIN ne correspondent pas - n'utilisez pas ce code", ["pin.change"] = "Modifier le PIN (expérimental)", ["state.title"] = "ÉTAT DE VERROUILLAGE / ERREUR", ["state.unknown"] = "Inconnu", ["state.present"] = "Valeurs de réinitialisation déjà présentes", ["state.values"] = "État : {0:X2} / {1:X2} / {2:X2}", ["reset.create"] = "Créer une réparation / réinitialisation", ["advanced.show"] = "Afficher les détails", ["advanced.hide"] = "Masquer les détails", ["advanced.notDetected"] = "Non détecté", ["advanced.copyA"] = "COPIE PIN A", ["advanced.copyB"] = "COPIE PIN B", ["advanced.stateBytes"] = "OCTETS D'ÉTAT DE VERROUILLAGE / ERREUR", ["advanced.verified"] = "RÉPARATION / RÉINITIALISATION VÉRIFIÉE", ["advanced.unknownMeaning"] = "La signification précise de chaque octet d'état n'est pas entièrement établie.", ["dialog.pinTitle"] = "Modification expérimentale du PIN radio", ["dialog.pinHeading"] = "Modifier le PIN radio", ["dialog.experimental"] = "Modification expérimentale de l'EEPROM", ["dialog.pinWarning"] = "Cette opération crée une COPIE modifiée de l'EEPROM. Elle ne programme pas directement la radio.\r\n\r\nLa compatibilité avec toutes les radios ou versions de micrologiciel n'est pas garantie. Conservez toujours l'EEPROM originale.", ["dialog.currentPin"] = "PIN actuel : {0}", ["dialog.cancel"] = "Annuler", ["dialog.newPin"] = "Nouveau PIN", ["dialog.confirmPin"] = "Confirmer le PIN", ["dialog.pinEnter"] = "Saisissez un PIN à quatre chiffres.", ["dialog.pinDigits"] = "Le PIN doit contenir exactement 4 chiffres.", ["dialog.pinMismatch"] = "Les PIN ne correspondent pas.", ["dialog.pinMatch"] = "Les PIN correspondent", ["dialog.authorization"] = "Je confirme être autorisé à intervenir sur cet équipement et j'assume la responsabilité de l'utilisation du fichier EEPROM généré.", ["dialog.createPin"] = "Créer le dump de changement de PIN", ["dialog.repairTitle"] = "Réparation / réinitialisation EEPROM", ["dialog.repairHeading"] = "Réinitialisation de l'état de verrouillage / erreur", ["dialog.repairSubtitle"] = "Veuillez lire avant de créer un fichier EEPROM modifié.", ["dialog.recoveredPin"] = "PIN récupéré : {0}", ["dialog.stateDetails"] = "03F0 : {0:X2} -> 00    03F8 : {1:X2} -> 00    03F9 : {2:X2} -> 00", ["dialog.repairAuthorization"] = "Je confirme être autorisé à intervenir sur cet équipement et j'assume la responsabilité de la programmation et de l'utilisation du fichier EEPROM généré.", ["dialog.createRepair"] = "Créer le dump de réparation" },
        [AppLanguage.Russian] = new Dictionary<string, string> { ["app.title"] = "Инструмент радио Smart 451 Highline", ["header.subtitle"] = "Утилита диагностики и восстановления EEPROM M95128", ["drop.file"] = "Перетащите сюда файл .bin M95128 размером 16 КБ", ["open.eeprom"] = "Открыть EEPROM", ["status.none"] = "EEPROM не загружена", ["status.loaded"] = "Загружен дамп M95128 размером 16 КБ", ["radio.id"] = "Bosch / Smart ID: {0}", ["radio.code"] = "КОД РАДИО", ["pin.decode"] = "Загрузите EEPROM для декодирования", ["pin.match"] = "Обе закодированные копии PIN совпадают", ["pin.mismatch"] = "Копии PIN не совпадают - не используйте этот код", ["pin.change"] = "Изменить PIN (экспериментально)", ["state.title"] = "СОСТОЯНИЕ БЛОКИРОВКИ / ОШИБКИ", ["state.unknown"] = "Неизвестно", ["state.present"] = "Значения сброса уже установлены", ["state.values"] = "Состояние: {0:X2} / {1:X2} / {2:X2}", ["reset.create"] = "Создать восстановление / сброс", ["advanced.show"] = "Показать подробности", ["advanced.hide"] = "Скрыть подробности", ["advanced.notDetected"] = "Не обнаружен", ["advanced.copyA"] = "КОПИЯ PIN A", ["advanced.copyB"] = "КОПИЯ PIN B", ["advanced.stateBytes"] = "БАЙТЫ СОСТОЯНИЯ БЛОКИРОВКИ / ОШИБКИ", ["advanced.verified"] = "ПРОВЕРЕННОЕ ВОССТАНОВЛЕНИЕ / СБРОС", ["advanced.unknownMeaning"] = "Точное значение каждого байта состояния установлено не полностью.", ["dialog.cancel"] = "Отмена", ["dialog.newPin"] = "Новый PIN", ["dialog.confirmPin"] = "Подтвердите PIN", ["dialog.pinEnter"] = "Введите PIN из четырёх цифр.", ["dialog.pinDigits"] = "PIN должен содержать ровно 4 цифры.", ["dialog.pinMismatch"] = "PIN не совпадают.", ["dialog.pinMatch"] = "PIN совпадают", ["dialog.createPin"] = "Создать дамп смены PIN", ["dialog.createRepair"] = "Создать дамп восстановления" },
        [AppLanguage.Romanian] = new Dictionary<string, string> { ["app.title"] = "Instrument radio Smart 451 Highline", ["header.subtitle"] = "Utilitar de diagnosticare și recuperare EEPROM M95128", ["drop.file"] = "Plasați aici un fișier .bin M95128 de 16 KB", ["open.eeprom"] = "Deschide EEPROM", ["status.none"] = "Niciun EEPROM încărcat", ["status.loaded"] = "Dump M95128 de 16 KB încărcat", ["radio.id"] = "Bosch / Smart ID: {0}", ["radio.code"] = "COD RADIO", ["pin.decode"] = "Încărcați un EEPROM pentru decodare", ["pin.match"] = "Ambele copii codificate ale PIN-ului coincid", ["pin.mismatch"] = "Copiile PIN-ului nu coincid - nu folosiți acest cod", ["pin.change"] = "Schimbă PIN-ul (experimental)", ["state.title"] = "STARE BLOCARE / EROARE", ["state.unknown"] = "Necunoscut", ["state.present"] = "Valorile de resetare sunt deja prezente", ["state.values"] = "Stare: {0:X2} / {1:X2} / {2:X2}", ["reset.create"] = "Creează reparare / resetare", ["advanced.show"] = "Afișează detaliile", ["advanced.hide"] = "Ascunde detaliile", ["advanced.notDetected"] = "Nedetectat", ["advanced.copyA"] = "COPIA PIN A", ["advanced.copyB"] = "COPIA PIN B", ["advanced.stateBytes"] = "OCTEȚI STARE BLOCARE / EROARE", ["advanced.verified"] = "REPARARE / RESETARE VERIFICATĂ", ["advanced.unknownMeaning"] = "Semnificația exactă a fiecărui octet de stare nu este pe deplin stabilită.", ["dialog.cancel"] = "Anulează", ["dialog.newPin"] = "PIN nou", ["dialog.confirmPin"] = "Confirmă PIN-ul", ["dialog.pinEnter"] = "Introduceți un PIN de patru cifre.", ["dialog.pinDigits"] = "PIN-ul trebuie să conțină exact 4 cifre.", ["dialog.pinMismatch"] = "PIN-urile nu coincid.", ["dialog.pinMatch"] = "PIN-urile coincid", ["dialog.createPin"] = "Creează dump schimbare PIN", ["dialog.createRepair"] = "Creează dump reparare" },
        [AppLanguage.Spanish] = new Dictionary<string, string> { ["app.title"] = "Herramienta de radio Smart 451 Highline", ["header.subtitle"] = "Utilidad de diagnóstico y recuperación EEPROM M95128", ["drop.file"] = "Suelta aquí un archivo .bin M95128 de 16 KB", ["open.eeprom"] = "Abrir EEPROM", ["status.none"] = "No hay EEPROM cargada", ["status.loaded"] = "Volcado M95128 de 16 KB cargado", ["radio.id"] = "Bosch / Smart ID: {0}", ["radio.code"] = "CÓDIGO DE RADIO", ["pin.decode"] = "Carga una EEPROM para decodificar", ["pin.match"] = "Las dos copias codificadas del PIN coinciden", ["pin.mismatch"] = "Las copias del PIN no coinciden - no uses este código", ["pin.change"] = "Cambiar PIN (experimental)", ["state.title"] = "ESTADO DE BLOQUEO / ERROR", ["state.unknown"] = "Desconocido", ["state.present"] = "Los valores de reinicio ya están presentes", ["state.values"] = "Estado: {0:X2} / {1:X2} / {2:X2}", ["reset.create"] = "Crear reparación / reinicio", ["advanced.show"] = "Mostrar detalles", ["advanced.hide"] = "Ocultar detalles", ["advanced.notDetected"] = "No detectado", ["advanced.copyA"] = "COPIA DE PIN A", ["advanced.copyB"] = "COPIA DE PIN B", ["advanced.stateBytes"] = "BYTES DE ESTADO DE BLOQUEO / ERROR", ["advanced.verified"] = "REPARACIÓN / REINICIO VERIFICADO", ["advanced.unknownMeaning"] = "El significado exacto de cada byte de estado no está completamente establecido.", ["dialog.cancel"] = "Cancelar", ["dialog.newPin"] = "PIN nuevo", ["dialog.confirmPin"] = "Confirmar PIN", ["dialog.pinEnter"] = "Introduce un PIN de cuatro dígitos.", ["dialog.pinDigits"] = "El PIN debe contener exactamente 4 dígitos.", ["dialog.pinMismatch"] = "Los PIN no coinciden.", ["dialog.pinMatch"] = "Los PIN coinciden", ["dialog.createPin"] = "Crear volcado de cambio de PIN", ["dialog.createRepair"] = "Crear volcado de reparación" }
    };

    public static AppLanguage CurrentLanguage { get; set; } = AppLanguage.English;

    public static string T(string key, params object[] args)
    {
        string value = English.TryGetValue(key, out string? english) ? english : key;
        if (CurrentLanguage != AppLanguage.English && Translations.TryGetValue(CurrentLanguage, out var language) && language.TryGetValue(key, out string? translated))
        {
            value = translated;
        }

        return args.Length == 0 ? value : string.Format(CultureInfo.CurrentCulture, value, args);
    }
}
