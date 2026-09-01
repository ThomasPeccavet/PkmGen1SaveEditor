namespace PkmGen1SaveEditor;

internal static class Gen1MoveCatalog
{
    private static readonly byte[] BasePp =
    [
        0, 35, 25, 10, 15, 20, 20, 15, 15, 15, 35, 30, 5, 10, 30, 30, 35, 35, 20, 15, 20, 20, 10, 20, 30, 5, 25, 15, 15, 15, 25, 20, 5, 35, 15, 20, 20, 20, 15, 30, 35, 20, 20, 30, 25, 40, 20, 15, 20, 20, 20, 30, 25, 15, 30, 25, 5, 15, 10, 5, 20, 20, 20, 5, 35, 20, 25, 20, 20, 20, 15, 20, 10, 10, 40, 25, 10, 35, 30, 15, 20, 40, 10, 15, 30, 15, 20, 10, 15, 10, 5, 10, 10, 25, 10, 20, 40, 30, 30, 20, 20, 15, 10, 40, 15, 20, 30, 20, 20, 10, 40, 40, 30, 30, 30, 20, 30, 10, 10, 20, 5, 10, 30, 20, 20, 20, 5, 15, 10, 20, 15, 15, 35, 20, 15, 10, 20, 30, 15, 40, 20, 15, 10, 5, 10, 30, 10, 15, 20, 15, 40, 40, 10, 5, 15, 10, 10, 10, 15, 30, 30, 10, 10, 20, 10, 10

    ];

    public static byte GetBasePp(byte moveId) =>
        moveId < BasePp.Length ? BasePp[moveId] : (byte)0;

    public static IEnumerable<(byte Id, string Name)> GetAll()
    {
        yield return (0, "— Aucun —");

        for (int moveId = 1; moveId <= 0xA5; moveId++)
        {
            byte id = (byte)moveId;
            yield return (id, GetName(id));
        }
    }


    public static string GetName(byte moveId)
    {
        return moveId switch
        {
            0x00 => "—",

            0x01 => "Écras'Face",
            0x02 => "Poing-Karaté",
            0x03 => "Torgnoles",
            0x04 => "Poing Comète",
            0x05 => "Ultimapoing",
            0x06 => "Jackpot",
            0x07 => "Poing Feu",
            0x08 => "Poing Glace",
            0x09 => "Poing Éclair",
            0x0A => "Griffe",
            0x0B => "Force Poigne",
            0x0C => "Guillotine",
            0x0D => "Coupe-Vent",
            0x0E => "Danse-Lames",
            0x0F => "Coupe",

            0x10 => "Tornade",
            0x11 => "Cru-Aile",
            0x12 => "Cyclone",
            0x13 => "Vol",
            0x14 => "Étreinte",
            0x15 => "Souplesse",
            0x16 => "Fouet Lianes",
            0x17 => "Écrasement",
            0x18 => "Double Pied",
            0x19 => "Ultimawashi",
            0x1A => "Pied Sauté",
            0x1B => "Mawashi Geri",
            0x1C => "Jet de Sable",
            0x1D => "Coup d'Boule",
            0x1E => "Koud'Korne",
            0x1F => "Furie",

            0x20 => "Empal'Korne",
            0x21 => "Charge",
            0x22 => "Plaquage",
            0x23 => "Ligotage",
            0x24 => "Bélier",
            0x25 => "Mania",
            0x26 => "Damoclès",
            0x27 => "Mimi-Queue",
            0x28 => "Dard-Venin",
            0x29 => "Double-Dard",
            0x2A => "Dard-Nuée",
            0x2B => "Groz'Yeux",
            0x2C => "Morsure",
            0x2D => "Rugissement",
            0x2E => "Hurlement",
            0x2F => "Berceuse",

            0x30 => "Ultrason",
            0x31 => "Sonicboom",
            0x32 => "Entrave",
            0x33 => "Acide",
            0x34 => "Flammèche",
            0x35 => "Lance-Flammes",
            0x36 => "Brume",
            0x37 => "Pistolet à O",
            0x38 => "Hydrocanon",
            0x39 => "Surf",
            0x3A => "Laser Glace",
            0x3B => "Blizzard",
            0x3C => "Rafale Psy",
            0x3D => "Bulles d'O",
            0x3E => "Onde Boréale",
            0x3F => "Ultralaser",

            0x40 => "Picpic",
            0x41 => "Bec Vrille",
            0x42 => "Sacrifice",
            0x43 => "Balayage",
            0x44 => "Riposte",
            0x45 => "Frappe Atlas",
            0x46 => "Force",
            0x47 => "Vol-Vie",
            0x48 => "Méga-Sangsue",
            0x49 => "Vampigraine",
            0x4A => "Croissance",
            0x4B => "Tranch'Herbe",
            0x4C => "Lance-Soleil",
            0x4D => "Poudre Toxik",
            0x4E => "Para-Spore",
            0x4F => "Poudre Dodo",

            0x50 => "Danse-Fleur",
            0x51 => "Sécrétion",
            0x52 => "Draco-Rage",
            0x53 => "Danse Flammes",
            0x54 => "Éclair",
            0x55 => "Tonnerre",
            0x56 => "Cage Éclair",
            0x57 => "Fatal-Foudre",
            0x58 => "Jet-Pierres",
            0x59 => "Séisme",
            0x5A => "Abîme",
            0x5B => "Tunnel",
            0x5C => "Toxik",
            0x5D => "Choc Mental",
            0x5E => "Psyko",
            0x5F => "Hypnose",

            0x60 => "Yoga",
            0x61 => "Hâte",
            0x62 => "Vive-Attaque",
            0x63 => "Frénésie",
            0x64 => "Téléport",
            0x65 => "Ombre Nocturne",
            0x66 => "Copie",
            0x67 => "Grincement",
            0x68 => "Reflet",
            0x69 => "Soin",
            0x6A => "Armure",
            0x6B => "Lilliput",
            0x6C => "Brouillard",
            0x6D => "Onde Folie",
            0x6E => "Repli",
            0x6F => "Boul'Armure",

            0x70 => "Bouclier",
            0x71 => "Mur Lumière",
            0x72 => "Buée Noire",
            0x73 => "Protection",
            0x74 => "Puissance",
            0x75 => "Patience",
            0x76 => "Métronome",
            0x77 => "Mimique",
            0x78 => "Destruction",
            0x79 => "Bomb'Œuf",
            0x7A => "Léchouille",
            0x7B => "Purédpois",
            0x7C => "Détritus",
            0x7D => "Massd'Os",
            0x7E => "Déflagration",
            0x7F => "Cascade",

            0x80 => "Claquoir",
            0x81 => "Météores",
            0x82 => "Coud'Krâne",
            0x83 => "Picanon",
            0x84 => "Constriction",
            0x85 => "Amnésie",
            0x86 => "Télékinésie",
            0x87 => "E-Coque",
            0x88 => "Pied Voltige",
            0x89 => "Intimidation",
            0x8A => "Dévorêve",
            0x8B => "Gaz Toxik",
            0x8C => "Pilonnage",
            0x8D => "Vampirisme",
            0x8E => "Grobisou",
            0x8F => "Piqué",

            0x90 => "Morphing",
            0x91 => "Écume",
            0x92 => "Uppercut",
            0x93 => "Spore",
            0x94 => "Flash",
            0x95 => "Vague Psy",
            0x96 => "Trempette",
            0x97 => "Acidarmure",
            0x98 => "Pince-Masse",
            0x99 => "Explosion",
            0x9A => "Combo-Griffe",
            0x9B => "Osmerang",
            0x9C => "Repos",
            0x9D => "Éboulement",
            0x9E => "Croc de Mort",
            0x9F => "Affûtage",

            0xA0 => "Conversion",
            0xA1 => "Triplattaque",
            0xA2 => "Croc Fatal",
            0xA3 => "Tranche",
            0xA4 => "Clonage",
            0xA5 => "Lutte",

            _ => $"Attaque inconnue (0x{moveId:X2})"
        };
    }
}
