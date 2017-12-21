using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Zlingo
{
    class Spel
    {
        private PinsInit pin;
        private Random R = new Random();

        StorageFolder opslagfolder = ApplicationData.Current.LocalFolder;
        private int AantalPogingen = 0;
        private int AantalWoorden = 0;
        public int AantalFoutief = 0;


        private List<string> woordenl = new List<string>() {"appel", "aldus", "afwas", "aftel", "aarde", "armen", "actie", "apart", "adres", "avond", "aders", "alarm", "boten", "balen", "beter", "bomen", "boren", "boven", "boxen", "brood", "broek", "brand", "breed", "benen", "beeld", "brief", "beten", "basis", "blauw", "beren", "buren", "banen", "bloed", "broer", "blond", "boter", "beleg", "breng", "baken", "beker", "blind", "bezig", "baden", "bedel", "bazen", "bazin", "baren", "beden", "beken", "bezem", "baard", "bidet", "breuk", "conus", "cello", "creme", "cloud", "cacao", "cadet", "cavia", "ceder", "combi", "china", "clown", "draai", "deden", "dalen", "derde", "delen", "dwaas", "daden", "dader", "dames", "diner", "datum", "dozen", "dreun", "duits", "dagen", "deren", "dwerg", "dwaal", "dwing", "druil", "droog", "draad", "dweil", "drank", "duren", "dwars", "drugs", "daten", "daler", "doorn", "disco", "degen", "droom", "dient", "drone", "dadel", "duwen", "druif", "deken", "deler", "elven", "eigen", "enger", "engel", "elder", "enkel", "effen", "email", "egaal", "fiets", "friet", "files", "forel", "films", "feest", "fruit", "falen", "flora", "fauna", "feeen", "freak", "forum", "fusie", "geven", "gaven", "groen", "graai", "getal", "grens", "grond", "groef", "graal", "gewei", "games", "grote", "groet", "garen", "gebak", "graag", "genre", "glans", "geluk", "geeuw", "horen", "heren", "halen", "hagel", "haren", "helen", "harde", "hemel", "hoofd", "huren", "hamer", "haken", "heden", "hotel", "hobby", "heler", "hoger", "ieder", "index", "immer", "icoon", "inlog", "inzet", "innig", "jovel", "jaren", "jicht", "jabot", "jacht", "jaden", "jagen", "jager", "japon", "jarig", "jawel", "jeans", "jemig", "jeugd", "joint", "jonas", "joule", "koken", "kreet", "koker", "kerst", "kegel", "koude", "kader", "krent", "kamer", "kaars", "kaart", "kraan", "krant", "keren", "kruid", "kerel", "kubus", "kraal", "kleur", "kroon", "klein", "korst", "klopt", "kabel", "kunst", "kopje", "krans", "klimt", "kater", "klink", "kudde", "kruis", "lopen", "laten", "lepel", "links", "laden", "leven", "lezen", "lucht", "lenen", "laser", "lente", "licht", "lader", "leder", "lunch", "lijst", "leger", "leden", "legen", "lagen", "lezer", "lever", "lingo", "loper", "luier", "lager", "leeuw", "maand", "malen", "maken", "media", "meter", "motor", "maten", "markt", "mazen", "molen", "meest", "meren", "model", "meden", "maden", "macht", "meeuw", "mager", "magen", "maren", "manen", "noord", "nieuw", "negen", "namen", "neven", "nodig", "naden", "neder", "nemen", "onder", "optel", "ovaal", "ovale", "onwel", "optie", "orden", "oppas", "ouder", "ophef", "oases", "palen", "plein", "pegel", "paars", "prijs", "piano", "pixel", "paden", "pasta", "pizza", "poten", "paard", "puber", "pauze", "preek", "polis", "pater", "proef", "panda", "penis", "prins", "pluto", "polen", "plint", "quota", "quant", "quark", "queue", "quilt", "quote", "robot", "reken", "raden", "regen", "radio", "rente", "regio", "rugby", "reden", "roken", "ruzie", "ruist", "regel", "racen", "races", "riool", "ramen", "radar", "roman", "rokje", "razen", "roede", "staan", "staal", "speel", "steeg", "stoel", "stook", "steek", "schep", "spijs", "stoep", "shirt", "samen", "sites", "sport", "spalk", "sjaal", "storm", "staat", "steun", "strak", "serie", "shows", "schat", "snoep", "sfeer", "smeer", "speer", "scene", "speld", "smeed", "smaak", "super", "stand", "steer", "smelt", "sedan", "skier", "sluis", "sneer", "steel", "truck", "terug", "typen", "talen", "taboe", "tegel", "taart", "tafel", "trouw", "teken", "teren", "taken", "treur", "tenen", "titel", "thuis", "tiara", "teder", "toets", "tabak", "trein", "tarwe", "telen", "teler", "uiten", "uilig", "uitje", "uiver", "ultra", "uniek", "uppie", "uraan", "uiers", "velen", "vloer", "video", "varen", "vegen", "veren", "vader", "vaten", "vuren", "vrouw", "vlees", "vogel", "vroeg", "vezel", "veins", "vorst", "veder", "vanaf", "vieze", "veger", "villa", "veler", "vrede", "vries", "woord", "wagen", "wonen", "waren", "warme", "weten", "water", "weren", "wazig", "wegen", "weven", "wezen", "weken", "wraak", "wilde", "wreed", "wrede", "wenst", "woest", "xenon", "yacht", "yucca", "zwaar", "zware", "zesde", "zagen", "zalig", "zomer", "zeden", "zwart", "zeven", "zicht", "zadel", "zweet", "zenuw", "zweer", "zweef", "zaden", "zaken", "zeker", "zever", "zeeen"};

        public List<int> TeamScores = new List<int>() { 0, 0, 0, 0 };

        public int teamnummer = 0; 
        private int beurtnummer = 0;
        private int Beginronde;
        public int rondenummer = 0;
        private int TeamAantal = 0;
        private int AantalGoed = 0;
        
        public int Correct = 0; // 1 na correct antwoord
        public int Wissel = 0; // 1 is de beurt van de wissel

        public string Speelwoord = "";
        public string Showwoord = "";
        public string BijnaGoed = "";
        public Spel(PinsInit pins, int AantalTeams) // Begin nieuw spel
        {
            TeamAantal = AantalTeams;
            pin = pins;
            Speelwoord = woordenl[R.Next(woordenl.Count-1)];
            pin.lcd.ClearDisplay();
            Task.Delay(5).Wait();
            pin.lcd.Write("Eerste woord: "+Speelwoord);
            
            teamnummer = R.Next(TeamAantal);
            Beginronde = teamnummer;
            Showwoord = Speelwoord[0].ToString() + "....";
        }

        public void Controle(string antwoord) // Controleren of een antwoord overeen komt met het speelwoord
        {
            BijnaGoed = "";
            AantalPogingen += 1;

            if (antwoord == Speelwoord) // Woord klopt
            {
                Correct = 1;
                NieuwWoord();
                beurtnummer = 0;
                AantalGoed += 1;
                TeamScores[teamnummer] += 25;
                if (AantalGoed == 3) { Wissel = 2; Wisselbeurt(); Correct = 0; }
            } 
            else // Woord klopt niet
            {
                for (int i = 0; i < antwoord.Length; i++)
                {
                    int AantalLettersAntwoord = 5 - Showwoord.Replace(antwoord[i].ToString(), "").Length;
                    int AantalLettersBijnaGoed = BijnaGoed.Length - BijnaGoed.Replace(antwoord[i].ToString(), "").Length;
                    int AantalLettersSpeelwoord = 5 - Speelwoord.Replace(antwoord[i].ToString(), "").Length;

                    if (antwoord[i] == Speelwoord[i])
                    {
                        Showwoord = Showwoord.Remove(i, 1);
                        Showwoord = Showwoord.Insert(i, Speelwoord[i].ToString());
                        AantalLettersAntwoord = 5 - Showwoord.Replace(antwoord[i].ToString(), "").Length;
                        if (AantalLettersBijnaGoed + AantalLettersAntwoord > AantalLettersSpeelwoord)
                        {
                            for (int z = 0; z < BijnaGoed.Length; z++)
                            {
                                if (BijnaGoed[z] == antwoord[i]) { BijnaGoed = BijnaGoed.Remove(BijnaGoed[z]); z = BijnaGoed.Length; }
                            }
                        }
                    }
                    else
                    { 
                        for (int y = 0; y < Speelwoord.Length; y++)
                        {
                            if (antwoord[i] == Speelwoord[y] & (AantalLettersBijnaGoed + AantalLettersAntwoord) < AantalLettersSpeelwoord)
                            {
                                BijnaGoed = BijnaGoed + antwoord[i];
                               
                                y = 5;
                            }
                        }
                    }
                }
                beurtnummer += 1;
            }

            if (beurtnummer == 5) { Wissel = 1; Wisselbeurt(); }
        }

        private void NieuwWoord() // Nieuw woord wordt aaangemaakt en getoont op het scherm
        {
            AantalWoorden += 1;
            woordenl.Remove(Speelwoord);
            Speelwoord = woordenl[R.Next(woordenl.Count - 1)];
            Showwoord = Speelwoord[0].ToString() + "....";
            pin.lcd.ClearDisplay();
            Task.Delay(5).Wait();
            pin.lcd.Write("Nieuw woord: " + Speelwoord);
        }

        public void Wisselbeurt() // De volgende speler krijgt de beurt
        {
            if (teamnummer != 0)
            {
                if (teamnummer + 1 < TeamAantal) teamnummer += 1;
                else teamnummer = 0;
            }
            else teamnummer += 1;
            if (Beginronde == teamnummer) rondenummer += 1;
            if (rondenummer != 10) NieuwWoord();
            beurtnummer = 0;
            AantalGoed = 0;
        }

        public async void Dataopslag() // Data wordt opgeslagen in de locale folder van het device
        {
            try
            {
                StorageFile Aantalfout = await opslagfolder.CreateFileAsync("AantalFout.txt");
                StorageFile Aantalpogingen = await opslagfolder.CreateFileAsync("AantalPogingen.txt");
                StorageFile AantalPunten = await opslagfolder.CreateFileAsync("AantalPunten.txt");
                await FileIO.WriteTextAsync(Aantalfout, AantalFoutief.ToString());
                await FileIO.WriteTextAsync(Aantalpogingen, (AantalPogingen / AantalWoorden).ToString());
                for (int i = 0; i < TeamAantal;) await FileIO.WriteTextAsync(AantalPunten, TeamScores[i].ToString());
            }
            catch ( Exception ex) { }
        
        }
        
    }
}
