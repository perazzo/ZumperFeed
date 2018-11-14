using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Xml.Linq;
using ZumperFeed.Models;
using ZumperFeed.Models.UserModel;

namespace ZumperFeed.Controllers
{
    public class ZumperController : Controller
    {
        public iRentEntities db = new iRentEntities();

        // GET: Zumper
        public ActionResult Index()
        {
            AzureStorage storage = new AzureStorage();
            var blob = storage.GetAzureBlob("zumper", "Feed.xml");

            XDocument document = XDocument.Load(blob.Uri.AbsoluteUri);

            return this.Content(document.ToString(), "application/xml");
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.HttpPost]
        public ActionResult sendData(FeedData receiveData)
        {
            List<Listing> dataList = new List<Listing>();

            // Get property
            var getProperty = db.properties.Find(receiveData.PropertyID);

            // Get Property Manager
            var getPropManager = (from u in db.users
                                  join upm in db.userpropertymaps on u.UserID equals upm.UserID
                                  where upm.PropertyID == receiveData.PropertyID
                                  && u.SecurityLevelID == 2
                                  select u).FirstOrDefault();

            // Get Units
            var getVacantUnitTypes = (from ut in db.unittypes
                                      join u in db.units on ut.UnitTypeID equals u.UnitTypeID
                                      where receiveData.Units.Contains(u.UnitID)
                                      select new { UnitName = u.UnitName, Price = ut.UnitCharge, ut.TotalBedrooms, ut.TotalBathrooms,
                                      ut.SquareFootage, u.UnitID}).ToList();
            foreach(var vacantUnit in getVacantUnitTypes)
            {
                Listing data = new Listing();
                data.UnitNumber = vacantUnit.UnitName;
                data.StreetAddress = getProperty.PropertyAddress1;
                data.City = getProperty.PropertyCity;
                data.State = getProperty.PropertyState;
                data.ZipCode = getProperty.PropertyZip;
                data.Price = vacantUnit.Price;
                data.Bedroooms = Int32.Parse(vacantUnit.TotalBedrooms);
                int i;
                if (int.TryParse(vacantUnit.TotalBathrooms, out i))
                {
                    data.Bathrooms = Int32.Parse(vacantUnit.TotalBathrooms);
                }
                else
                {
                    float baths = float.Parse(vacantUnit.TotalBathrooms);
                    int fullbah = (int)baths;
                    data.Bathrooms = fullbah;
                }
                data.SquareFeet = vacantUnit.SquareFootage;
                data.Description = getProperty.PropertyLongDescription;
                data.ListingID = vacantUnit.UnitID;
                data.LpURL = getProperty.PropertyWebsite;
                data.SiteURL = getProperty.PropertyWebsite;
                data.SiteName = getProperty.PropertyName;

                if (getPropManager != null)
                {
                    data.AgentName = getPropManager.UserFName + " " + getPropManager.UserLName;
                    data.AgentEmail = getPropManager.UserEmail;
                }

                // Get images
                var getPropertyPhotos = (from up in db.unitphotos
                                         where db.unittypes.Any(ut => ut.PropertyID == getProperty.PropertyID && ut.UnitTypeID == up.UnitTypeID) &&
                                         up.MarketingFileTypeID == 2
                                         select up).ToList();
                List<string> listImages = new List<string>();
                foreach (var propImage in getPropertyPhotos)
                {
                    string url = "http://www.myirent.com/rent/UnitPhotos/" + getProperty.PropertyID.ToString() + "/" + propImage.FileName;
                    listImages.Add(url);
                }
                data.Images = listImages;

                dataList.Add(data);
            }

            CreateFeed createXML = new CreateFeed();
            createXML.CreateXML(dataList);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}