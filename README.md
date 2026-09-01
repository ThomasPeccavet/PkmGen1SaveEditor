<div align="center">

<img src="PkmGen1SaveEditor/asset/PkmnGen1Save.png" width="260" alt="Logo Pkm Gen 1 Save Editor">

# Pkm Gen 1 Save Editor

**Un éditeur Windows moderne pour les sauvegardes de Pokémon Rouge et Bleu.**

[![Build](https://github.com/ThomasPeccavet/PkmGen1SaveEditor/actions/workflows/build.yml/badge.svg)](https://github.com/ThomasPeccavet/PkmGen1SaveEditor/actions/workflows/build.yml)
![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)
![Windows Forms](https://img.shields.io/badge/UI-Windows%20Forms-0078D4?logo=windows11&logoColor=white)
[![Licence MIT](https://img.shields.io/badge/Licence-MIT-2EA44F)](LICENSE.txt)
![Statut](https://img.shields.io/badge/Statut-Alpha-F5A623)

[Fonctionnalités](#fonctionnalités) ·
[Compatibilité](#compatibilité) ·
[Installation](#installation) ·
[Utilisation](#utilisation) ·
[Feuille de route](#feuille-de-route)

</div>

---

## Présentation

Pkm Gen 1 Save Editor est une application non officielle permettant d’ouvrir,
de consulter et de modifier une sauvegarde Game Boy de **Pokémon Rouge ou
Pokémon Bleu** depuis une interface Windows Forms.

L’éditeur manipule directement les structures binaires de la première
génération : informations du dresseur, équipe, statistiques des Pokémon,
douze boîtes PC, inventaires et Pokédex. Les checksums concernés sont recalculés
après chaque modification afin de préserver l’intégrité de la sauvegarde.

L’interface adopte un style **glass moderne et épuré** : cartes translucides,
palette claire, actions hiérarchisées et mise en page adaptative. Les fenêtres
restent confortables à lire lorsque leur taille ou l’échelle Windows change.

> [!WARNING]
> Le projet est encore en version alpha. Conservez toujours une copie intacte
> de votre sauvegarde originale avant d’effectuer une modification.

## Fonctionnalités

### Gestion des sauvegardes

- ouverture des fichiers Game Boy bruts de 32 Kio (`.sav`) ;
- vérification de la taille et du checksum principal ;
- détection des sauvegardes incompatibles ou corrompues ;
- export vers un nouveau fichier `_edited.sav` ;
- conservation du fichier original par défaut.

### Dresseur

- lecture et modification du nom du joueur ;
- lecture et modification du nom du rival ;
- lecture et modification de l’argent, de 0 à 999 999 ₽ ;
- lecture du temps de jeu ;
- lecture et modification des huit badges de Kanto.

### Équipe Pokémon

- affichage du surnom, de l’espèce, des types, du niveau, des PV, du statut et
  des attaques ;
- fiche détaillée avec modification du niveau, de l’expérience et des
  statistiques ;
- édition des quatre attaques, des PP actuels et des PP Plus ;
- édition des DV Attaque, Défense, Vitesse et Spécial de 0 à 15 ;
- calcul automatique du DV des PV selon les règles de la génération I ;
- édition des cinq EV, appelés « Stat Exp » en génération I, de 0 à 65 535 ;
- recalcul des statistiques à partir de l’espèce, du niveau, des DV et des EV ;
- ajout cohérent d’un Pokémon à partir de son espèce et de son niveau ;
- remplacement, suppression et duplication ;
- réorganisation de l’ordre de l’équipe ;
- soin collectif des PV, statuts et PP ;
- sprites Rouge/Bleu chargés à la demande et mis en cache ;
- protection contre une équipe vide ou supérieure à six membres.

### Boîtes PC

- lecture des 12 boîtes et de leur capacité de 20 Pokémon ;
- ajout et suppression de Pokémon stockés ;
- dépôt depuis l’équipe et retrait vers l’équipe ;
- déplacement d’un Pokémon entre deux boîtes ;
- recherche par surnom, espèce, type ou attaque ;
- modification des attaques, PP, DV et EV depuis une boîte ;
- synchronisation de la boîte active et des banques de stockage.

### Inventaire

- lecture et modification des 20 lignes du sac ;
- lecture et modification des 50 lignes du PC objets ;
- ajout, remplacement et suppression d’un objet avec une quantité de 1 à 99 ;
- modification de l’argent de 0 à 999 999 ₽ et des Jetons du Casino de 0 à
  9 999 ;
- catalogue français des objets, CT et CS de Pokémon Rouge/Bleu ;
- filtres d’ajout rapide pour les **CT/CS**, **Poké Balls**, **soins** et
  **objets clés** ;
- détection en direct des identifiants impossibles, quantités invalides,
  objets uniques dupliqués et lignes ordinaires répétées ;
- respect automatique des marqueurs de fin et des capacités du jeu.

### Pokédex

- affichage des 151 Pokémon dans l’ordre du Pokédex national ;
- modification indépendante des indicateurs **Vu** et **Capturé** ;
- cohérence automatique : un Pokémon capturé est toujours marqué comme vu ;
- boutons **Tout voir**, **Tout capturer** et **Réinitialiser** ;
- compteurs de progression vus et capturés ;
- recherche instantanée par numéro ou par nom français.

### Intégrité des données

- encodage et décodage du texte de la génération I ;
- lecture et écriture de l’argent en BCD ;
- lecture et écriture des Jetons du Casino en BCD ;
- lecture et écriture des listes d’objets terminées par `0xFF` ;
- lecture et écriture des deux tableaux binaires du Pokédex ;
- lecture et écriture des valeurs 16 et 24 bits en big-endian ;
- recalcul du checksum principal ;
- recalcul des checksums des deux banques PC et de chaque boîte ;
- validation des limites avant toute écriture binaire.

## Aperçu de l’interface

L’application comporte un thème commun à toutes les fenêtres :

- surfaces glass aux angles arrondis et fond bleu-gris doux ;
- hiérarchie claire entre actions principales, secondaires et destructives ;
- formulaires construits avec des dispositions adaptatives plutôt que des
  positions fixes ;
- tableaux allégés, lignes plus hautes et colonnes recentrées sur les données
  essentielles ;
- fiche Pokémon répartie en trois onglets : **Statistiques**,
  **Attaques & PP** et **DV & EV** ;
- dimensions minimales explicites pour éviter les champs ou textes tronqués.

Les captures de l’application seront ajoutées après validation visuelle de cette
interface sur Windows, afin que le README montre exactement le rendu distribué.

## Compatibilité

| Jeu | Région/langue | État |
|---|---|:---:|
| Pokémon Rouge / Bleu | Français | ✅ Pris en charge |
| Pokémon Red / Blue | Anglais | 🧪 Structure prise en charge, tests à compléter |
| Pokémon Jaune / Yellow | Toutes | ❌ Non pris en charge |
| Formats avec en-tête ou pied d’émulateur | Toutes | ❌ Non pris en charge |

Le fichier doit faire exactement **32 768 octets**. La sélection de langue
présente dans l’interface est encore informative : la détection automatique de
la version et de la langue reste à développer.

Les ROM, BIOS et clés de console ne sont ni nécessaires ni fournis.

## Prérequis

- Windows 10 ou Windows 11 ;
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) ;
- [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/)
  avec la charge de travail **Développement Desktop .NET**.

## Installation

Le projet est actuellement distribué sous forme de code source.

```bash
git clone https://github.com/ThomasPeccavet/PkmGen1SaveEditor.git
cd PkmGen1SaveEditor
```

Ouvrez ensuite `PkmGen1SaveEditor.slnx` dans Visual Studio, attendez la
restauration des dépendances, puis lancez le projet avec `F5`.

Pour compiler en ligne de commande :

```bash
dotnet build PkmGen1SaveEditor.slnx --configuration Release
```

Les futures versions prêtes à l’emploi seront publiées dans les
[Releases GitHub](https://github.com/ThomasPeccavet/PkmGen1SaveEditor/releases).

## Utilisation

1. Exportez le fichier `.sav` depuis votre émulateur, flashcart ou lecteur de
   cartouche.
2. Lancez l’application et cliquez sur **Ouvrir une sauvegarde**.
3. Modifiez les informations du dresseur si nécessaire.
4. Utilisez **Gérer l’équipe et les boîtes PC**, **Gérer l’inventaire** ou
   **Gérer le Pokédex** selon les données à modifier.
5. Dans l’inventaire, corrigez toutes les erreurs affichées par le contrôle
   d’intégrité avant de cliquer sur **Appliquer**.
6. Dans le Pokédex, utilisez la recherche ou les actions collectives, puis
   cliquez sur **Appliquer**.
7. Cliquez sur **Enregistrer sous…** dans la fenêtre principale.
8. Importez le fichier `_edited.sav` dans votre émulateur ou votre matériel.

> [!TIP]
> Travaillez toujours sur une copie. Ne remplacez votre sauvegarde originale
> qu’après avoir vérifié le résultat dans le jeu.

### Sprites

Les sprites ne sont pas inclus dans le dépôt. Ils sont téléchargés à la demande
depuis le dépôt public de [PokeAPI/sprites](https://github.com/PokeAPI/sprites),
puis conservés en mémoire pendant la session. L’édition reste fonctionnelle sans
connexion Internet ; seul l’aperçu du sprite sera absent.

## Structure du projet

```text
PkmGen1SaveEditor/
├── .github/workflows/build.yml       # Compilation Windows automatique
├── PkmGen1SaveEditor.slnx
├── README.md
├── LICENSE.txt
└── PkmGen1SaveEditor/
    ├── Gen1SaveFile.cs               # Dresseur, équipe et checksum principal
    ├── Gen1SaveFile.Storage.cs       # Équipe avancée et boîtes PC
    ├── Gen1SaveFile.Inventory.cs     # Sac, PC objets, argent et Jetons
    ├── Gen1SaveFile.Pokedex.cs       # Pokémon vus et capturés
    ├── Gen1InventoryModels.cs        # Modèles et alertes d’inventaire
    ├── Gen1ItemCatalog.cs            # Objets, CT, CS et catégories
    ├── Gen1PokedexEntry.cs           # Entrée du Pokédex
    ├── Gen1Pokemon.cs                # Modèle d’un Pokémon
    ├── Gen1SpeciesCatalog.cs         # Noms et identifiants internes
    ├── Gen1SpeciesData.cs            # Stats, types et croissance
    ├── Gen1MoveCatalog.cs            # Attaques de la génération I
    ├── Gen1StatBlock.cs              # Résultat d’un calcul de statistiques
    ├── PokemonSpriteService.cs       # Chargement et cache des sprites
    ├── GlassPanel.cs                  # Carte glass arrondie
    ├── ModernTheme.cs                # Thème et styles partagés
    ├── MainForm.cs                    # Fenêtre principale
    ├── TeamForm.cs                    # Équipe et stockage PC
    ├── InventoryForm.cs               # Sac et PC objets
    ├── InventoryItemForm.cs           # Ajout et modification d’un objet
    ├── PokedexForm.cs                 # Vus, capturés et progression
    ├── PokemonDetailsForm.cs          # Statistiques détaillées
    ├── AddPokemonForm.cs              # Création et remplacement
    └── BoxSelectionForm.cs            # Choix d’une boîte PC
```

## Détails techniques

| Donnée | Représentation |
|---|---|
| Taille de sauvegarde | 32 Kio / 32 768 octets |
| Noms | Table de caractères Pokémon génération I |
| Argent | 3 octets BCD |
| Jetons du Casino | 2 octets BCD |
| Badges | 8 indicateurs binaires dans un octet |
| Sac | 20 couples identifiant/quantité + marqueur `0xFF` |
| PC objets | 50 couples identifiant/quantité + marqueur `0xFF` |
| Pokédex | 19 octets « vus » + 19 octets « capturés » |
| Expérience | entier 24 bits big-endian |
| DV | 4 valeurs de 4 bits ; DV PV dérivé |
| EV / Stat Exp | 5 entiers 16 bits big-endian |
| PP | 6 bits de PP actuels + 2 bits de PP Plus |
| Équipe | 1 à 6 structures de 44 octets |
| Boîte PC | 12 boîtes de 20 Pokémon |
| Intégrité | checksums principal, banques PC et boîtes |

La logique binaire est séparée de l’interface afin de faciliter les tests et
l’ajout futur d’autres versions du jeu.

## Limites connues

- Pokémon Jaune n’est pas pris en charge.
- La langue et la version exactes ne sont pas encore détectées automatiquement.
- Le temps de jeu est affiché mais n’est pas modifiable.
- Les options du jeu ne sont pas encore éditables.
- Les objets internes inutilisés, les badges et les objets temporaires comme
  la Safari Ball sont volontairement refusés par l’éditeur d’inventaire.
- La création d’un Pokémon est cohérente mais ne simule pas encore tous les
  détails d’une capture naturelle dans le jeu.
- Les sauvegardes enrichies d’un en-tête ou d’un pied propre à un émulateur ne
  sont pas reconnues.

## Feuille de route

### Version 0.1.0

- [x] ouvrir, vérifier et exporter une sauvegarde ;
- [x] modifier le dresseur, l’argent et les badges ;
- [x] consulter et modifier les statistiques de l’équipe ;
- [x] ajouter, remplacer, supprimer, dupliquer et réorganiser des Pokémon ;
- [x] gérer les 12 boîtes PC et les transferts ;
- [x] afficher les types, attaques et sprites ;
- [x] éditer les attaques, PP, DV et EV de l’équipe et des boîtes ;
- [x] recalculer les statistiques depuis les DV et EV ;
- [x] unifier l’interface glass moderne sur toutes les fenêtres ;
- [x] modifier le sac, le PC objets, l’argent et les Jetons du Casino ;
- [x] ajouter rapidement les CT/CS, Poké Balls, soins et objets clés ;
- [x] détecter les objets impossibles, quantités invalides et doublons ;
- [x] modifier les 151 entrées vues et capturées du Pokédex ;
- [x] rechercher dans le Pokédex et afficher les compteurs de progression ;
- [x] compiler automatiquement chaque Pull Request sur Windows ;
- [ ] ajouter des tests automatisés sur des sauvegardes anonymisées ;
- [ ] publier la première version Windows.

### Évolutions envisagées

- [ ] sauvegarde automatique de secours avant modification ;
- [ ] glisser-déposer d’un fichier `.sav` ;
- [ ] détection automatique du jeu, de la langue et de la région ;
- [ ] historique d’annulation/rétablissement ;
- [ ] prise en charge de Pokémon Jaune ;
- [ ] rapport de validation avant export.

## Contribuer

Les retours, rapports de bugs et contributions sont bienvenus dans les
[Issues GitHub](https://github.com/ThomasPeccavet/PkmGen1SaveEditor/issues).

Pour contribuer au code :

1. créez une branche dédiée ;
2. conservez la logique de sauvegarde séparée de l’interface ;
3. utilisez uniquement des copies anonymisées de sauvegardes ;
4. vérifiez la compilation en mode `Release` ;
5. décrivez clairement le changement dans la Pull Request.

## Remerciements

- [pret/pokered](https://github.com/pret/pokered) pour la documentation issue du
  désassemblage des jeux ;
- [PokeAPI/sprites](https://github.com/PokeAPI/sprites) pour l’accès aux sprites
  utilisés à la demande ;
- les communautés de préservation et de rétro-ingénierie Pokémon.

## Mentions légales

Pokémon et les noms associés sont des marques de Nintendo, Game Freak et
Creatures. Ce projet amateur et non officiel n’est affilié, approuvé ou sponsorisé
par aucune de ces sociétés.

Le dépôt ne contient ni ROM, ni BIOS, ni clé de console. Les utilisateurs sont
responsables de l’export légal de leurs propres sauvegardes.

## Licence

Le code source est distribué sous [licence MIT](LICENSE.txt).

Copyright © 2026 [Thomas Peccavet](https://github.com/ThomasPeccavet).
