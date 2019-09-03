using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Solution
{
    public class StockPrice
    {

        public string date;
        public string open;
        public string high;
        public string low;
        public string close;
    }

    public class ResponseClass
    {
        public int page;
        public int per_page;
        public int total;
        public int total_pages;
        public List<StockPrice> data;
    }


    /*
     * Complete the function below.
     */
    static void openAndClosePrices(string firstDate, string lastDate, string weekDay)
    {

        string[] firstTokens = firstDate.Split('-');
        string[] lastTokens = lastDate.Split('-');
        string firstYear = firstTokens[2];
        string lastYear = lastTokens[2];
        int iniYear = int.Parse(firstYear);
        int endYear = int.Parse(lastYear);

        for (int i = iniYear; i <= endYear; i++)
        {
            // make request by year
            List<StockPrice> stocks = requestStocks(i.ToString());
            // Validate if each day is equal to 'weekDay'
            foreach (var s in stocks)
            {
                if (validateDay(weekDay, s.date, firstDate, lastDate))
                {
                    System.Console.WriteLine(s.date + " " + s.open + " " + s.close);
                }
            }
        }

    }

    public static string CreateRequest(string url)
    {
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Headers["AuthToken"] = "8686330657058660259";

        var response = (HttpWebResponse)request.GetResponse();
        using (var streamReader = new StreamReader(response.GetResponseStream()))
        {
            return streamReader.ReadToEnd();
        }
    }


    private static List<StockPrice> requestStocks(string year)
    {
        System.Security.Policy.Url url;
        System.Text.StringBuilder response = new System.Text.StringBuilder();
        try
        {
            url = new System.Security.Policy.Url("https://jsonmock.hackerrank.com/api/stocks/search?date=" + year);
            string req = CreateRequest(url.Value);
            dynamic responseObj = JsonConvert.DeserializeObject(req, typeof(ResponseClass));
            return responseObj.data;

        }
        catch (IOException e)
        {
            throw e;
        }
        finally
        {
        }

    }


    private static bool validateDay(String weekDay, String date, String firstDate, String lastDate)
    {
        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
        DateTime currentDate = DateTime.ParseExact(date.Replace(" ", ""), "d-MMMM-yyyy", provider);
        DateTime localFirstDate = DateTime.ParseExact(firstDate.Replace(" ", ""), "d-MMMM-yyyy", provider);
        DateTime localLastDate = DateTime.ParseExact(lastDate.Replace(" ", ""), "d-MMMM-yyyy", provider);

        if (currentDate.DayOfWeek.ToString().ToUpper().Equals(weekDay.ToUpper())
            && currentDate.AddDays(-1) < localLastDate
            && currentDate.AddDays(1) > localFirstDate
            )
        {
            return true;
        }
        return false;
    }

    static void Main(String[] args)
    {
        string _firstDate;
        _firstDate = Console.ReadLine();

        string _lastDate;
        _lastDate = Console.ReadLine();

        string _weekDay;
        _weekDay = Console.ReadLine();

        openAndClosePrices("1-January-2000 ", "22-February-2000 ", "Monday");
        Console.ReadKey();
    }
}