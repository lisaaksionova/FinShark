using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public static class SeedData
    {
        public static void SeedDatabase(ApplicationDbContext context)
        {
            context.Database.Migrate();
            if (context.Stocks.Count() == 0 && context.Comments.Count() == 0)
            {
                Stock s1 = new Stock
                {
                    Symbol = "TSLA",
                    CompanyName = "Tesla",
                    Purchase = 100,
                    LastDiv = 2,
                    Industry = "Automotive",
                    MarketCap = 234234234
                };
                Stock s2 = new Stock
                {
                    Symbol = "MSFT",
                    CompanyName = "Microsoft",
                    Purchase = 100,
                    LastDiv = 1.20M,
                    Industry = "Technology",
                    MarketCap = 234234245
                };
                Stock s3 = new Stock
                {
                    Symbol = "VTI",
                    CompanyName = "Vanguard Total Stock Market Index Fund ETF",
                    Purchase = 200,
                    LastDiv = 2.10M,
                    Industry = "Index Fund",
                    MarketCap = 234234223
                };
                Stock s4 = new Stock
                {
                    Symbol = "PLTR",
                    CompanyName = "Plantir",
                    Purchase = 23,
                    LastDiv = 0,
                    Industry = "Technology",
                    MarketCap = 1234234
                };

                context.Stocks.AddRange(s1, s2, s3, s4);

                context.SaveChanges();
                Comment c1 = new Comment
                {
                    Title = "test comment",
                    Content = "test comment",
                    CreatedOn = DateTime.Now,
                    StockId = s1.Id
                };
                Comment c2 = new Comment
                {
                    Title = "test comment",
                    Content = "test comment",
                    CreatedOn = DateTime.Now,
                    StockId = s1.Id
                };
                context.Comments.AddRange(c1, c2);
                context.SaveChanges();
                //Portfolio p1 = new Portfolio
                //{
                //    AppUserId = "319f6694-fc57-4aeb-9c70-636cc96184d4",
                //    StockId = 1
                //};
                //context.Portfolios.AddRange(p1);
                //context.SaveChanges();
            }
        }
    }
}
