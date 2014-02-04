using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TechnicalInterview_FeedReader.Models
{
    public class FeedsDB : DbContext
    {
        public FeedsDB()
            : base("DefaultConnection")
        {

        }

        public DbSet<Feed> Feeds { get; set; }
    }
}