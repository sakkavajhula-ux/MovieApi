using System.Globalization;
using CsvHelper;
using MovieApi.Models;

namespace MovieApi.Data
{
    // Initialises the DB with movie data from the CSV file
    // Data is imported only if the DB is empty
    // static class to be called from Program.cs
    public static class DbInitializer
    {
        public static void Initialize(MovieDbContext context, string csvFilePath)
        {
            // Check if the database is empty
            if (context.Movies.Any())
            {
                return; // DB has been seeded
            }
            // Read the CSV file and seed the database
            using var reader = new StreamReader(csvFilePath);
            //got some errors while importing data, so added below.
            var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null, // Ignore missing fields
                BadDataFound = null // Ignore bad data
            };
            using var csv = new CsvReader(reader, config);

           csv.Read();
           csv.ReadHeader();

            while(csv.Read())
            {
                //Skip invalid rows !!
                if (csv.Parser.Count <9 )
                {
                    continue;
                }
                var movie = new Movie
                {
                    ReleaseDate = csv.GetField<DateTime>("Release_Date"),
                    Title = csv.GetField<string>("Title") ?? string.Empty,
                    Overview = csv.GetField<string>("Overview") ?? string.Empty,
                    Popularity = csv.GetField<double>("Popularity"),
                    VoteCount = csv.GetField<int>("Vote_Count"),
                    VoteAverage = csv.GetField<double>("Vote_Average"),
                    OriginalLanguage = csv.GetField<string>("Original_Language") ?? string.Empty,
                    PosterUrl = csv.GetField<string>("Poster_Url") ?? string.Empty,
                    Genre = csv.GetField<string>("Genre") ?? string.Empty   
                };
                context.Movies.Add(movie);
            }
            context.SaveChanges();


        }



    }
}
