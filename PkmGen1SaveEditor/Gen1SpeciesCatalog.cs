using System;
using System.Collections.Generic;
using System.Text;

namespace PkmGen1SaveEditor;

internal static class Gen1SpeciesCatalog
{
    public static string GetName(byte speciesId)
    {
        return speciesId switch
        {
            0x01 => "Rhinoféros",
            0x02 => "Kangourex",
            0x03 => "Nidoran ♂",
            0x04 => "Mélofée",
            0x05 => "Piafabec",
            0x06 => "Voltorbe",
            0x07 => "Nidoking",
            0x08 => "Flagadoss",
            0x09 => "Herbizarre",
            0x0A => "Noadkoko",
            0x0B => "Excelangue",
            0x0C => "Noeunoeuf",
            0x0D => "Tadmorv",
            0x0E => "Ectoplasma",
            0x0F => "Nidoran ♀",

            0x10 => "Nidoqueen",
            0x11 => "Osselait",
            0x12 => "Rhinocorne",
            0x13 => "Lokhlass",
            0x14 => "Arcanin",
            0x15 => "Mew",
            0x16 => "Léviator",
            0x17 => "Kokiyas",
            0x18 => "Tentacool",
            0x19 => "Fantominus",
            0x1A => "Insécateur",
            0x1B => "Stari",
            0x1C => "Tortank",
            0x1D => "Scarabrute",
            0x1E => "Saquedeneu",

            0x21 => "Caninos",
            0x22 => "Onix",
            0x23 => "Rapasdepic",
            0x24 => "Roucool",
            0x25 => "Ramoloss",
            0x26 => "Kadabra",
            0x27 => "Gravalanch",
            0x28 => "Leveinard",
            0x29 => "Machopeur",
            0x2A => "M. Mime",
            0x2B => "Kicklee",
            0x2C => "Tygnon",
            0x2D => "Arbok",
            0x2E => "Parasect",
            0x2F => "Psykokwak",

            0x30 => "Soporifik",
            0x31 => "Grolem",
            0x33 => "Magmar",
            0x35 => "Élektek",
            0x36 => "Magnéton",
            0x37 => "Smogo",
            0x39 => "Férosinge",
            0x3A => "Otaria",
            0x3B => "Taupiqueur",
            0x3C => "Tauros",

            0x40 => "Canarticho",
            0x41 => "Mimitoss",
            0x42 => "Dracolosse",
            0x46 => "Doduo",
            0x47 => "Ptitard",
            0x48 => "Lippoutou",
            0x49 => "Sulfura",
            0x4A => "Artikodin",
            0x4B => "Électhor",
            0x4C => "Métamorph",
            0x4D => "Miaouss",
            0x4E => "Krabby",

            0x52 => "Goupix",
            0x53 => "Feunard",
            0x54 => "Pikachu",
            0x55 => "Raichu",
            0x58 => "Minidraco",
            0x59 => "Draco",
            0x5A => "Kabuto",
            0x5B => "Kabutops",
            0x5C => "Hypotrempe",
            0x5D => "Hypocéan",

            0x60 => "Sabelette",
            0x61 => "Sablaireau",
            0x62 => "Amonita",
            0x63 => "Amonistar",
            0x64 => "Rondoudou",
            0x65 => "Grodoudou",
            0x66 => "Évoli",
            0x67 => "Pyroli",
            0x68 => "Voltali",
            0x69 => "Aquali",
            0x6A => "Machoc",
            0x6B => "Nosferapti",
            0x6C => "Abo",
            0x6D => "Paras",
            0x6E => "Têtarte",
            0x6F => "Tartard",

            0x70 => "Aspicot",
            0x71 => "Coconfort",
            0x72 => "Dardargnan",
            0x74 => "Dodrio",
            0x75 => "Colossinge",
            0x76 => "Triopikeur",
            0x77 => "Aéromite",
            0x78 => "Lamantine",
            0x7B => "Chenipan",
            0x7C => "Chrysacier",
            0x7D => "Papilusion",
            0x7E => "Mackogneur",

            0x80 => "Akwakwak",
            0x81 => "Hypnomade",
            0x82 => "Nosferalto",
            0x83 => "Mewtwo",
            0x84 => "Ronflex",
            0x85 => "Magicarpe",
            0x88 => "Grotadmorv",
            0x8A => "Krabboss",
            0x8B => "Crustabri",
            0x8D => "Électrode",
            0x8E => "Mélodelfe",
            0x8F => "Smogogo",

            0x90 => "Persian",
            0x91 => "Ossatueur",
            0x93 => "Spectrum",
            0x94 => "Abra",
            0x95 => "Alakazam",
            0x96 => "Roucoups",
            0x97 => "Roucarnage",
            0x98 => "Staross",
            0x99 => "Bulbizarre",
            0x9A => "Florizarre",
            0x9B => "Tentacruel",
            0x9D => "Poissirène",
            0x9E => "Poissoroy",

            0xA3 => "Ponyta",
            0xA4 => "Galopa",
            0xA5 => "Rattata",
            0xA6 => "Rattatac",
            0xA7 => "Nidorino",
            0xA8 => "Nidorina",
            0xA9 => "Racaillou",
            0xAA => "Porygon",
            0xAB => "Ptéra",
            0xAD => "Magnéti",

            0xB0 => "Salamèche",
            0xB1 => "Carapuce",
            0xB2 => "Reptincel",
            0xB3 => "Carabaffe",
            0xB4 => "Dracaufeu",
            0xB9 => "Mystherbe",
            0xBA => "Ortide",
            0xBB => "Rafflesia",
            0xBC => "Chétiflor",
            0xBD => "Boustiflor",
            0xBE => "Empiflor",

            0x00 => "Emplacement vide",

            _ => $"Inconnu (0x{speciesId:X2})"
        };
    }

    public static IReadOnlyList<(byte Id, string Name)> GetAll()
    {
        List<(byte Id, string Name)> species = [];

        for (int rawId = 1; rawId <= byte.MaxValue; rawId++)
        {
            byte id = (byte)rawId;
            string name = GetName(id);

            if (!name.StartsWith("Inconnu", StringComparison.Ordinal))
            {
                species.Add((id, name));
            }
        }

        return species
            .OrderBy(entry => entry.Name, StringComparer.CurrentCulture)
            .ToArray();
    }
}
