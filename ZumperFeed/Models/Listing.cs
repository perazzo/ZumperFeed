using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZumperFeed.Models
{
    public class Listing
    {
        public string UnitNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PropertyType { get; set; } = "apartment";
        public Decimal Price { get; set; }
        public int Bedroooms { get; set; }
        public int Bathrooms { get; set; }
        public string SquareFeet { get; set; }
        public string Description { get; set; }
        public int ListingID { get; set; }
        public string LpURL { get; set; }
        public string Status { get; set; } = "for rent";
        public string SiteURL { get; set; }
        public string SiteName { get; set; }
        public string SmallDogs { get; set; } = "no";
        public string LargeDogs { get; set; } = "no";
        public string Cats { get; set; } = "no";
        public List<string> Images { get; set; }
        public string AgentName { get; set; } = "";
        public string AgentEmail { get; set; } = "";
        public string HasWasher { get; set; } = "no";
        public string HasDryer { get; set; } = "no";
        public string HasDishwasher { get; set; } = "yes";
        public string HasAirConditioning { get; set; } = "yes";
        public string FloorCovering { get; set; } = "carpet";
        public string HasVaultedCeiling { get; set; } = "no";
    }
}