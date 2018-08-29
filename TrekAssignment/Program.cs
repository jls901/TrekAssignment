using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrekAssignment
{
    class Program
    {
        private static readonly string householdBikeDatarUri = "https://trekhiringassignments.blob.core.windows.net/interview/bikes.json"; 
        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            var getBikeDataTask = GetHouseholdBikeData();
            getBikeDataTask.Wait();
            var householdBikeData = getBikeDataTask.Result;
            var rankedHouseholdBikeCombos = RankHouseholdBikeCombinations(householdBikeData); 
            PrintRankedBikeComboResults(rankedHouseholdBikeCombos);
        }

        private static async Task<IEnumerable<HouseholdBikesModel>> GetHouseholdBikeData()
        {
            var response = await client.GetAsync(householdBikeDatarUri);
            var rawHouseholdBikeResults = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<HouseholdBikesModel>>(rawHouseholdBikeResults);
        }

        private static IDictionary<string, int> RankHouseholdBikeCombinations(IEnumerable<HouseholdBikesModel> householdBikeCombinations)
        {
            var result = new Dictionary<string, int>();
            foreach (var household in householdBikeCombinations)
            {
                var householdBikeHash = household.ToString();
                if(result.ContainsKey(householdBikeHash))
                {
                    result[householdBikeHash] += 1; 
                }
                else
                {
                    result.Add(householdBikeHash, 1);
                }
            }
            return result;
        }

        private static void PrintRankedBikeComboResults(IDictionary<string, int> bikeCombos)
        {
            var bikeComboPlace = 1; 
            foreach(var combo in bikeCombos.OrderByDescending(x => x.Value).Take(20))
            {
                var comboPlace = string.Format("#{0} Combo", bikeComboPlace); 
                var bikeCombo = string.Format("Bikes: {0}", combo.Key); 
                var numberOfBikeComboOwners = string.Format("Number of households having combo: {0}", combo.Value); 
                Console.WriteLine(comboPlace);
                Console.WriteLine(bikeCombo);
                Console.WriteLine(numberOfBikeComboOwners);
                Console.WriteLine();
                bikeComboPlace++;
            }
        }
    }
}
