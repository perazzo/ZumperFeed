using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace ZumperFeed.Models.UserModel
{
    public class CreateFeed
    {
        public void CreateXML(List<Listing> listData)
        {
            try
            {
                AzureStorage storage = new AzureStorage();
                var blob = storage.GetAzureBlob("zumper", "Feed.xml");
                XDocument document = XDocument.Load(blob.Uri.AbsoluteUri);

                foreach(var data in listData)
                {
                    // Search by address
                    XElement checkListing = (from el in document.Root.Elements("property").Elements("details").Elements("provider-listingid")
                                             where int.Parse(el.Value) == data.ListingID
                                             select el).FirstOrDefault();
                    if (checkListing != null)
                    {
                        checkListing.Parent.Parent.Remove();
                    }

                    XElement property = new XElement("property");
                    XElement location = new XElement("location");
                    XElement unitNumber = new XElement("unit-number", data.UnitNumber);
                    XElement streetAddress = new XElement("street-address", data.StreetAddress);
                    XElement cityName = new XElement("city-name", data.City);
                    XElement stateCode = new XElement("state-code", data.State);
                    XElement zipCode = new XElement("zipCode", data.ZipCode);
                    XElement displayAddress = new XElement("display-address", "no");
                    location.Add(unitNumber);
                    location.Add(streetAddress);
                    location.Add(cityName);
                    location.Add(stateCode);
                    location.Add(zipCode);
                    location.Add(displayAddress);
                    property.Add(location);

                    XElement details = new XElement("details");
                    XElement propertyType = new XElement("property-type", data.PropertyType);
                    XElement price = new XElement("price", data.Price);
                    XElement numBedrooms = new XElement("num-bedrooms", data.Bedroooms);
                    XElement numBathrooms = new XElement("num-bathrooms", data.Bathrooms);
                    XElement sqFeet = new XElement("living-area-square-fee", data.SquareFeet);
                    XElement description = new XElement("description", data.Description);
                    XElement listingID = new XElement("provider-listingid", data.ListingID);
                    details.Add(propertyType);
                    details.Add(price);
                    details.Add(numBedrooms);
                    details.Add(numBathrooms);
                    details.Add(sqFeet);
                    details.Add(description);
                    details.Add(listingID);
                    property.Add(details);

                    XElement landingpage = new XElement("landing-page");
                    XElement lpURL = new XElement("lp-url", data.LpURL);
                    landingpage.Add(lpURL);
                    property.Add(landingpage);

                    XElement listingType = new XElement("listing-type", "rental");
                    property.Add(listingType);

                    XElement status = new XElement("status", data.Status);
                    property.Add(status);

                    XElement site = new XElement("site");
                    XElement siteURL = new XElement("site-url", data.SiteURL);
                    XElement siteName = new XElement("site-name", data.SiteName);
                    site.Add(siteURL);
                    site.Add(siteName);
                    property.Add(site);

                    XElement pets = new XElement("pets");
                    XElement smallDogs = new XElement("small-dogs-allowed", data.SmallDogs);
                    XElement largeDogs = new XElement("large-dogs-allowed", data.LargeDogs);
                    XElement cats = new XElement("cats-allowed", data.Cats);
                    pets.Add(smallDogs);
                    pets.Add(largeDogs);
                    pets.Add(cats);
                    property.Add(pets);

                    // pictures
                    XElement pictures = new XElement("pictures");
                    int i = 1;
                    foreach (var pic in data.Images)
                    {
                        XElement picture = new XElement("picture");
                        XElement pictureURL = new XElement("picture-url", pic);
                        XElement pictureNum = new XElement("picture-seq-number", i);
                        picture.Add(pictureURL);
                        picture.Add(pictureNum);
                        pictures.Add(picture);
                        i++;
                    }
                    property.Add(pictures);

                    XElement agent = new XElement("agent");
                    XElement agentName = new XElement("agent-name", data.AgentName);
                    XElement agentEmail = new XElement("agent-email", data.AgentEmail);
                    agent.Add(agentName);
                    agent.Add(agentEmail);
                    property.Add(agent);

                    XElement brokerage = new XElement("brokerage");
                    XElement brokerageName = new XElement("brokerage-name", data.AgentName);
                    XElement brokerageEmail = new XElement("brokerage-email", data.AgentEmail);
                    XElement brokerageSite = new XElement("brokerage-website", "https://myirent.com");
                    brokerage.Add(brokerageName);
                    brokerage.Add(brokerageEmail);
                    brokerage.Add(brokerageSite);
                    property.Add(brokerage);

                    XElement detailedCharacteristics = new XElement("detailed-characteristics");
                    XElement appliances = new XElement("appliances");
                    XElement hasWasher = new XElement("has-washer", data.HasWasher);
                    XElement hasDryer = new XElement("has-dryer", data.HasDryer);
                    XElement hasDishwasher = new XElement("has-dishwasher", data.HasDishwasher);
                    appliances.Add(hasWasher);
                    appliances.Add(hasDryer);
                    appliances.Add(hasDishwasher);
                    detailedCharacteristics.Add(appliances);
                    XElement coolingSystem = new XElement("cooling-system");
                    XElement hasAirConditioning = new XElement("has-air-conditioning", data.HasAirConditioning);
                    detailedCharacteristics.Add(coolingSystem);
                    XElement floorCoverings = new XElement("floor-coverings", data.FloorCovering);
                    detailedCharacteristics.Add(floorCoverings);
                    XElement hasVaultedCeling = new XElement("has-vaulted-ceiling", data.HasVaultedCeiling);
                    detailedCharacteristics.Add(hasVaultedCeling);
                    property.Add(detailedCharacteristics);

                    document.Element("properties").Add(property);
                }

                // Upload to Azure
                blob.UploadText(document.ToString());

                MailMessage mailMessage = new MailMessage();
                string sendTo = "support@myirent.com";
                mailMessage.To.Add(sendTo);
                mailMessage.From = new MailAddress("support@myirent.com");
                mailMessage.Subject = "iRent - Zumper Feed Test";
                string message = "Test";
                mailMessage.Body = message;
                //Attach
                byte[] contentAsBytes = Encoding.Default.GetBytes(document.ToString());
                MemoryStream memoryStream = new MemoryStream();
                memoryStream.Write(contentAsBytes, 0, contentAsBytes.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);
                ContentType contentType = new ContentType();
                contentType.MediaType = MediaTypeNames.Text.Plain;
                contentType.Name = "XMLFeed.xml";
                Attachment attachment = new Attachment(memoryStream, contentType);
                mailMessage.Attachments.Add(attachment);
                // Send
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.myirent.com"; //Or Your SMTP Server Address
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("support@myirent.com", "iRent4Now!");
                smtp.Send(mailMessage);

            } catch(Exception any)
            {
                MailMessage mailMessage = new MailMessage();
                string sendTo = "support@myirent.com";
                mailMessage.To.Add(sendTo);
                mailMessage.From = new MailAddress("support@myirent.com");
                mailMessage.Subject = "iRent - Zumper Feed Error";
                string message = any.ToString();
                mailMessage.Body = message;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.myirent.com"; //Or Your SMTP Server Address
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("support@myirent.com", "iRent4Now!");
                smtp.Send(mailMessage);
            }
        }
    }
}