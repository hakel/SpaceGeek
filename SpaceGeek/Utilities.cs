using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;

namespace SpaceGeek
{
    public class Utilities
    {
        public static List<FactResource> GetResources()
        {
            List<FactResource> resources = new List<FactResource>();

            FactResource enUSResource = new FactResource("en-US");
            enUSResource.SkillName = "American Science Facts";
            enUSResource.GetFactMessage = "Here's your science fact: ";
            enUSResource.HelpMessage = "You can say tell me a science fact, or, you can say exit... What can I help you with?";
            enUSResource.HelpReprompt = "You can say tell me a science fact to start";
            enUSResource.StopMessage = "Goodbye!";
            enUSResource.Facts = GetFacts();

            resources.Add(enUSResource);

            return resources;
        }

        public static List<string> GetFacts()
        {
            List<string> facts = new List<string>();

            string baseUrl = "http://api.nytimes.com/svc/search/v2/articlesearch.json?q=cincinnati&sort=newest&api-key=5476602b245f4bc38ffbad32c1eb11f5";
            //The 'using' will help to prevent memory leaks.
            //Create a new instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(baseUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    dynamic responseData = JsonConvert.DeserializeObject(responseString);

                    int docCount = (int)responseData.response.docs.Count;

                    for (int i = 0; i < docCount; i++)
                    {
                        string hl = responseData.response.docs[i].headline.main;
                        facts.Add(hl);
                    }
                }
            }

            return facts;

        }
        public static List<string> GetFactsOld()
        {
            List<string> facts = new List<string>();

            facts.Add("A year on Mercury is just 88 days long.");
            facts.Add("Despite being farther from the Sun, Venus experiences higher temperatures than Mercury.");
            facts.Add("Venus rotates counter-clockwise, possibly because of a collision in the past with an asteroid.");
            facts.Add("On Mars, the Sun appears about half the size as it does on Earth.");
            facts.Add("Earth is the only planet not named after a god.");
            facts.Add("Jupiter has the shortest day of all the planets.");
            facts.Add("The Milky Way galaxy will collide with the Andromeda Galaxy in about 5 billion years.");
            facts.Add("The Sun contains 99.86% of the mass in the Solar System.");
            facts.Add("The Sun is an almost perfect sphere.");
            facts.Add("A total solar eclipse can happen once every 1 to 2 years. This makes them a rare event.");
            facts.Add("Saturn radiates two and a half times more energy into space than it receives from the sun.");
            facts.Add("The temperature inside the Sun can reach 15 million degrees Celsius.");
            facts.Add("The Moon is moving approximately 3.8 cm away from our planet every year.");

            return facts;

        }
        public static string EmitNewFact(FactResource resource, bool withPreface)
        {
            Random r = new Random();
            if (withPreface)
            {
                return resource.GetFactMessage + resource.Facts[r.Next(resource.Facts.Count)];
            }
            else
            {
                return resource.Facts[r.Next(resource.Facts.Count)];
            }
        }

    }
}
