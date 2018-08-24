using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreBasics.Configuration
{
    class WorldSettings
    {
        public Temperature MinTemperature { get; set; }

        public Temperature MaxTemperature { get; set; }

        public int HumanPopulation { get; set; }

        public override string ToString()
        {
            return $"Min temperature: {MinTemperature}\nMax temperature: {MaxTemperature}\nHuman population: {HumanPopulation}";
        }
    }

    class Temperature
    {
        public int Celsius { get; set; }

        public string Location { get; set; }

        public override string ToString()
        {
            return $"{Celsius}ºC at {Location}";
        }
    }
}
