using System;

namespace AspNetCoreBasics.WebApp
{
    public interface IMemeService
    {
        string GiveMeAMemeExclamationMark();
    }

    public class HardcodedMemeService : IMemeService
    {
        private static readonly string[] SomeMemes = new[] { "Be Like Bill", "'But That's None of My Business'", "Kylo Ren's Tri-Lightsaber", "Grumpy Cat", "Ylvis: What Does the Fox Say?", "The New Old Spice Guy", "Rickroll Someone", "More Cowbell", "U Mad Bro", "Randall, the Honey Badger Animal Narrator", "Doge! Much Wow, Such Fun...", "Gas Pump Karaoke Prank", "Leekspin: the Loituma Girl", "Where the Hell Is Matt Harding ? : Dancing Around the World", "All Your Base Are Belong to Us", "Good Guy Greg", "Domo-Kun", "The Spinning Ballerina Illusion", "Gangnam Style, by Rapper Psy", "LOLcats", "Ermahgerd", "The Double Rainbow", "Epic Fail!", "Lady Gaga: Bad Romance", "Mr. Rogers Remixed: Garden of Your Mind", "NONONO Cat", "OK, Go! The White Knuckles Music Video", "Numa Numa Dance", "Rebecca Black - 'Friday' Music Video", "Star Wars Kid", "People of Wal-Mart", "David After the Dentist", "Achmed the Dead Terrorist", "Chuck Norris", "Angry German Kid", "The Famous Dancing Baby", "Dogs Go to Heaven", "Charlie Bit Me!", "Ask a Ninja", "Engrish Funny", "OK, Go: Here It Goes Again (The Treadmill Dance)", "Nuts the Squirrel", "One Red Paperclip", "David Elsewhere Breakdance", "Bert is Evil", "Jizz In My Pants", "Demotivational Posters", "The Hamsterdance", "Dramatic Chipmunk", "Diet Coke and Mentos" };

        public string GiveMeAMemeExclamationMark()
        {
            var rand = new Random();

            return SomeMemes[rand.Next(0, SomeMemes.Length - 1)];
        }
    }
}