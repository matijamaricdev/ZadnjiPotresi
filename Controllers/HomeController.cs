using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZadnjiPotresi.Models;

namespace ZadnjiPotresi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private string Title { get; set; }
        private string Url { get; set; }
        //scraping website
        private string siteUrl = "https://www.volcanodiscovery.com/earthquakes/croatia-showMore.html";
        CancellationTokenSource cancellationToken = new CancellationTokenSource();
        HttpClient httpClient = new HttpClient();

        internal async void ScrapeWebsite()
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage request = await httpClient.GetAsync(siteUrl);
            cancellationToken.Token.ThrowIfCancellationRequested();
            Stream response = await request.Content.ReadAsStreamAsync();
            cancellationToken.Token.ThrowIfCancellationRequested();
            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(response);
        }

        public async Task<IActionResult> Index()
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage request = await httpClient.GetAsync(siteUrl);
            cancellationToken.Token.ThrowIfCancellationRequested();
            Stream response = await request.Content.ReadAsStreamAsync();
            cancellationToken.Token.ThrowIfCancellationRequested();
            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(response);
            List<string> Results = GetScrapeResults(document);
            List<string> Info = GetInfoResults(document);
            ViewData["Potresi"] = Results;
            ViewData["Info"] = Info;

            return await Task.Run(() => View());
        }

        private List<string> GetInfoResults(IHtmlDocument document)
        {
            List<string> allInfos = AllInfo(document);
            return allInfos;
        }

        private List<string> AllInfo(IHtmlDocument document)
        {
            List<string> returnAllInfos = new List<string>();
            IEnumerable<IElement> allInfos = document.All.Where(x => x.ClassName == "textbox");
            AllInfos(returnAllInfos, allInfos);
            return returnAllInfos;
        }

        private void AllInfos(List<string> returnAllInfos, IEnumerable<IElement> allInfos)
        {

            foreach (var c in allInfos)
            {
                if (returnAllInfos.Count != 1)
                {
                    string[] infos = c.InnerHtml.Split("<br>")[0].Split(',');
                    string secondNumberInfo = infos[1].Split("quake of magnitude")[0].Split("by")[1];
                    string secondMagInfo = infos[1].Split("quake of magnitude")[1];
                    string thirdNumberInfo = infos[2].Split("quakes between")[0];
                    string thirdMagInfo = infos[2].Split("quakes between")[1].Split("and")[0];
                    string thirdMag2Info = infos[2].Split("quakes between")[1].Split("and")[1];
                    string fourthNumberInfo = infos[3].Split("quakes between")[0];
                    string fourthMagInfo = infos[3].Split("quakes between")[1].Split("and")[0];
                    string fourthMag2Info = infos[3].Split("quakes between")[1].Split("and")[1];
                    string fifthNumberInfo = infos[4].Split("quakes between")[0];
                    string fifthMagInfo = infos[4].Split("quakes between")[1].Split("and")[0];
                    string fifthMag2Info = infos[4].Split("quakes between")[1].Split("and")[1];
                    string sixthNumberInfo = infos[5].Split("quakes between")[0].Split("and")[1];
                    string sixthMagInfo = infos[5].Split("quakes between")[1].Split("and")[0];
                    string sixthMag2Info = infos[5].Split("quakes between")[1].Split("and")[1].Split("There")[0];
                    string finalString = String.Format("Hrvatsku je do sada u zadnjih 30 dana potreslo: {0} potres sa magnitudom {1}, {2} potresa sa magnitudom između {3} i {4}," +
                        "{5} potresa sa magnitudom između {6} i {7}, {8} potresa sa magnitudom između {9} i {10} i {11} potresa sa magnitudom između {12} i {13}",
                         secondNumberInfo, secondMagInfo, thirdNumberInfo, thirdMagInfo, thirdMag2Info, fourthNumberInfo, fourthMagInfo, fourthMag2Info, fifthNumberInfo,
                         fifthMagInfo, fifthMag2Info, sixthNumberInfo, sixthMagInfo, sixthMag2Info);

                    returnAllInfos.Add(finalString);
                }
            }
        }

        public IActionResult Doniraj()
        {
            return View();
        }


        private List<string> GetScrapeResults(IHtmlDocument document)
        {
            List<string> allEarthquakes = AllEarthQuakesIznad3(document);
            return allEarthquakes;
        }

        private List<string> AllEarthQuakesIspod3(IHtmlDocument document)
        {
            List<string> returnAllEarthQuakes = new List<string>();
            List<string> returnAllRegions = new List<string>();
            IEnumerable<IElement> allEarthQuakesMag1 = document.All.Where(x => x.ClassName == "q1");
            IEnumerable<IElement> allEarthQuakesMag2 = document.All.Where(x => x.ClassName == "q2");
            IEnumerable<IElement> allRegions = document.All.Where(x => x.ClassName == "listRegion");

            PotresiManjihMagnituda(returnAllEarthQuakes, allEarthQuakesMag1, allEarthQuakesMag2);
            return returnAllEarthQuakes;
        }

        private List<string> AllEarthQuakesIznad3(IHtmlDocument document)
        {
            List<string> returnAllEarthQuakes = new List<string>();
            List<string> returnAllRegions = new List<string>();
            IEnumerable<IElement> allEarthQuakesMag1 = document.All.Where(x => x.ClassName == "q1");
            IEnumerable<IElement> allEarthQuakesMag2 = document.All.Where(x => x.ClassName == "q2");
            IEnumerable<IElement> allEarthQuakesMag3 = document.All.Where(x => x.ClassName == "q3");
            IEnumerable<IElement> allEarthQuakesMag4 = document.All.Where(x => x.ClassName == "q4");
            IEnumerable<IElement> allEarthQuakesMag5 = document.All.Where(x => x.ClassName == "q5");
            IEnumerable<IElement> allEarthQuakesMag6 = document.All.Where(x => x.ClassName == "q6");
            IEnumerable<IElement> allRegions = document.All.Where(x => x.ClassName == "listRegion");

            PotresiMagnituzaIznadTri(returnAllEarthQuakes, allEarthQuakesMag3, allEarthQuakesMag4, allEarthQuakesMag5, allEarthQuakesMag6);

            return returnAllEarthQuakes;
        }

        private static void PotresiMagnituzaIznadTri(List<string> returnAllEarthQuakes, IEnumerable<IElement> allEarthQuakesMag3, IEnumerable<IElement> allEarthQuakesMag4, IEnumerable<IElement> allEarthQuakesMag5, IEnumerable<IElement> allEarthQuakesMag6)
        {
            returnAllEarthQuakes.Add("Zadnji potresi iznad magnitude 6");
            returnAllEarthQuakes.Add("Vrijeme potresa | magnituda | dubina potresa | država | lokacija | grad");

            foreach (var c in allEarthQuakesMag6)
            {

                string[] infos = c.InnerHtml.Split(">");

                var time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0];
                if (time.EndsWith('('))
                {
                    time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0].Substring(0, time.Length - 2);
                }

                var magnitude = c.InnerHtml.Split(">")[9].Substring(0, 3);
                if (magnitude.Contains("td"))
                {
                    magnitude = c.InnerHtml.Split(">")[11].Substring(0, 3);
                }

                var depth = c.InnerHtml.Split(">")[11].Split('&')[0];
                if (depth.Contains("di"))
                {
                    depth = c.InnerHtml.Split(">")[13].Substring(0, 2);
                }
                if (depth.Contains("&"))
                {
                    depth = depth.Substring(0, 1);
                }

                var country = c.InnerHtml.Split(">")[19].Split("<br")[0];
                if (country.Contains("<"))
                {
                    country = c.InnerHtml.Split(">")[28].Split('<')[0];
                }

                var location = c.InnerHtml.Split(">")[21].Split('<')[0];

                var city = c.InnerHtml.Split(">")[24];
                if (city.Contains("<"))
                {
                    city = city.Substring(0, city.Length - 3);
                }

                var addThis = String.Format("{0} | {1} | {2} km | {3} | {4} {5}", time, magnitude, depth, country, location, city);

                if (addThis.Length > 75)
                {
                    continue;
                }
                returnAllEarthQuakes.Add(addThis);
            }

            returnAllEarthQuakes.Add("Zadnji potresi iznad magnitude 5");
            returnAllEarthQuakes.Add("Vrijeme potresa, magnituda, dubina potresa, država, lokacija, grad");

            foreach (var c in allEarthQuakesMag5)
            {

                string[] infos = c.InnerHtml.Split(">");

                var time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0];
                if (time.EndsWith('('))
                {
                    time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0].Substring(0, time.Length - 2);
                }

                var magnitude = c.InnerHtml.Split(">")[9].Substring(0, 3);
                if (magnitude.Contains("td"))
                {
                    magnitude = c.InnerHtml.Split(">")[11].Substring(0, 3);
                }

                var depth = c.InnerHtml.Split(">")[11].Split('&')[0];
                if (depth.Contains("di"))
                {
                    depth = c.InnerHtml.Split(">")[13].Substring(0, 2);
                }
                if (depth.Contains("&"))
                {
                    depth = depth.Substring(0, 1);
                }

                var country = c.InnerHtml.Split(">")[19].Split("<br")[0];
                if (country.Contains("<"))
                {
                    country = c.InnerHtml.Split(">")[28].Split('<')[0];
                }

                var location = c.InnerHtml.Split(">")[21].Split('<')[0];

                var city = c.InnerHtml.Split(">")[24];

                if (city.Contains("<"))
                {
                    city = city.Substring(0, city.Length - 3);
                }
                var addThis = String.Format("{0} | {1} | {2} km | {3} | {4} {5}", time, magnitude, depth, country, location, city);
                returnAllEarthQuakes.Add(addThis);
            }

            returnAllEarthQuakes.Add("Zadnji potresi iznad magnitude 4");
            returnAllEarthQuakes.Add("Vrijeme potresa, magnituda, dubina potresa, država, lokacija, grad");

            foreach (var c in allEarthQuakesMag4)
            {
                string[] infos = c.InnerHtml.Split(">");
                var time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0];
                if (time.EndsWith('('))
                {
                    time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0].Substring(0, time.Length - 2);
                }
                if (time.Length > 30)
                {
                    time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0].Substring(0, 18);
                }

                var magnitude = c.InnerHtml.Split(">")[9].Substring(0, 3);
                if (magnitude.Contains("td"))
                {
                    magnitude = c.InnerHtml.Split(">")[11].Substring(0, 3);
                }
                if (magnitude.Contains("?"))
                {
                    magnitude = "Nisu potvrđena mjerenja do sada.";
                }

                var depth = c.InnerHtml.Split(">")[11].Split('&')[0];
                if (depth.Contains("di"))
                {
                    depth = c.InnerHtml.Split(">")[13].Substring(0, 2);
                }
                if (depth.Contains("&"))
                {
                    depth = depth.Substring(0, 1);
                }

                var country = c.InnerHtml.Split(">")[19].Split("<br")[0];
                if (country.Contains("<"))
                {
                    country = c.InnerHtml.Split(">")[28].Split('<')[0];
                }
                if (country.Contains("reports"))
                {
                    country = c.InnerHtml.Split(">")[21].Split('<')[0];
                }

                var location = c.InnerHtml.Split(">")[21].Split('<')[0];

                var city = c.InnerHtml.Split(">")[24];

                if (magnitude.Contains("Nisu potvrđena"))
                {
                    city = "Croatia";
                }
                if (city.Contains("<"))
                {
                    city = city.Substring(0, city.Length - 3);
                }

                var addThis = String.Format("{0} | {1} | {2} km | {3} | {4} {5}", time, magnitude, depth, country, location, city);
                returnAllEarthQuakes.Add(addThis);
            }

            returnAllEarthQuakes.Add("Zadnji potresi iznad magnitude 3");
            returnAllEarthQuakes.Add("Vrijeme potresa, magnituda, dubina potresa, država, lokacija, grad");

            foreach (var c in allEarthQuakesMag3)
            {
                string[] infos = c.InnerHtml.Split(">");
                var time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0];
                if (time.EndsWith('('))
                {
                    time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0].Substring(0, time.Length - 2);
                }
                if (time.Length > 30)
                {
                    time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0].Substring(0, 18);
                }

                var magnitude = c.InnerHtml.Split(">")[9].Substring(0, 3);
                if (magnitude.Contains("td"))
                {
                    magnitude = c.InnerHtml.Split(">")[11].Substring(0, 3);
                }

                var depth = c.InnerHtml.Split(">")[11].Split('&')[0];
                if (depth.Contains("di"))
                {
                    depth = c.InnerHtml.Split(">")[13].Substring(0, 2);
                }
                if (depth.Contains("&"))
                {
                    depth = depth.Substring(0, 1);
                }

                var country = c.InnerHtml.Split(">")[19].Split("<br")[0];
                if (country.Contains("<"))
                {
                    country = c.InnerHtml.Split(">")[28].Split('<')[0];
                }

                var location = c.InnerHtml.Split(">")[21].Split('<')[0];

                var city = c.InnerHtml.Split(">")[24];
                if (city.Contains("<"))
                {
                    city = city.Substring(0, city.Length - 3);
                }

                var addThis = String.Format("{0} | {1} | {2} km | {3} | {4} {5}", time, magnitude, depth, country, location, city);
                if (addThis.Contains("sisak") && addThis.Contains("moslavina") && addThis.Contains("earthq"))
                {
                    continue;
                }

                returnAllEarthQuakes.Add(addThis);
            }
        }

        private static void PotresiManjihMagnituda(List<string> returnAllEarthQuakes, IEnumerable<IElement> allEarthQuakesMag1, IEnumerable<IElement> allEarthQuakesMag2)
        {
            returnAllEarthQuakes.Add("Earthquake over 2.0 magnitude");
            returnAllEarthQuakes.Add("Vrijeme potresa, magnituda, dubina potresa, država, lokacija, grad");

            foreach (var c in allEarthQuakesMag2)
            {
                string[] infos = c.InnerHtml.Split(">");
                var time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0];
                if (time.EndsWith('('))
                {
                    time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0].Substring(0, time.Length - 2);
                }
                if (time.Length > 30)
                {
                    time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0].Substring(0, 18);
                }

                var magnitude = c.InnerHtml.Split(">")[9].Substring(0, 3);
                if (magnitude.Contains("td"))
                {
                    magnitude = c.InnerHtml.Split(">")[11].Substring(0, 3);
                }

                var depth = c.InnerHtml.Split(">")[11].Split('&')[0];
                if (depth.Contains("di"))
                {
                    depth = c.InnerHtml.Split(">")[13].Substring(0, 2);
                }
                if (depth.Contains("&"))
                {
                    depth = depth.Substring(0, 1);
                }

                var country = c.InnerHtml.Split(">")[19].Split("<br")[0];
                if (country.Contains("<"))
                {
                    country = c.InnerHtml.Split(">")[28].Split('<')[0];
                }

                var location = c.InnerHtml.Split(">")[21].Split('<')[0];

                var city = c.InnerHtml.Split(">")[24];
                if (city.Contains("<"))
                {
                    city = city.Substring(0, city.Length - 3);
                }

                var addThis = String.Format("{0} | {1} | {2} km | {3} | {4} {5}", time, magnitude, depth, country, location, city);
                if (addThis.Contains("sisak") && addThis.Contains("moslavina") && addThis.Contains("earthq"))
                {
                    continue;
                }

                returnAllEarthQuakes.Add(addThis);
            }

            returnAllEarthQuakes.Add("Earthquake over 1.0 magnitude");
            returnAllEarthQuakes.Add("Vrijeme potresa, magnituda, dubina potresa, država, lokacija, grad");

            foreach (var c in allEarthQuakesMag1)
            {
                string[] infos = c.InnerHtml.Split(">");
                var time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0];
                if (time.EndsWith('('))
                {
                    time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0].Substring(0, time.Length - 2);
                }
                if (time.Length > 30)
                {
                    time = c.InnerHtml.Split("<td>")[1].Split("GMT")[0].Substring(0, 18);
                }

                var magnitude = c.InnerHtml.Split(">")[9].Substring(0, 3);
                if (magnitude.Contains("td"))
                {
                    magnitude = c.InnerHtml.Split(">")[11].Substring(0, 3);
                }

                var depth = c.InnerHtml.Split(">")[11].Split('&')[0];
                if (depth.Contains("di"))
                {
                    depth = c.InnerHtml.Split(">")[13].Substring(0, 2);
                }
                if (depth.Contains("&"))
                {
                    depth = depth.Substring(0, 1);
                }

                var country = c.InnerHtml.Split(">")[19].Split("<br")[0];
                if (country.Contains("<"))
                {
                    country = c.InnerHtml.Split(">")[28].Split('<')[0];
                }

                var location = c.InnerHtml.Split(">")[21].Split('<')[0];

                var city = c.InnerHtml.Split(">")[24];
                if (city.Contains("<"))
                {
                    city = city.Substring(0, city.Length - 3);
                }

                var addThis = String.Format("{0} | {1} | {2} km | {3} | {4} {5}", time, magnitude, depth, country, location, city);

                if (addThis.Contains("sisak") && addThis.Contains("moslavina") && addThis.Contains("earthq"))
                {
                    continue;
                }
                returnAllEarthQuakes.Add(addThis);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
