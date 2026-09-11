using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SetupReportGenerator.Models;

namespace SetupReportGenerator.Services
{
    public class XmlService
    {
        public RecipeHeader ReadRecipeHeader(string filePath)
        {
            XDocument document = XDocument.Load(filePath);

            RecipeHeader recipe = new RecipeHeader();

            recipe.RecipeName =
                document.Root.Attribute("Name").Value;

            recipe.Model =
                document.Root
                        .Element("BoardsRightLane")
                        .Element("Board")
                        .Attribute("Name")
                        .Value;

            recipe.LineName =
                document.Root
                        .Element("Setup")
                        .Element("Line")
                        .Attribute("Name")
                        .Value;

            recipe.BoardSide = "Right";

            return recipe;
        }

        public List<SetupDetail> ReadSetupDetails(string filePath)
        {
            XDocument document = XDocument.Load(filePath);

            List<SetupDetail> setupDetails = new List<SetupDetail>();

            XElement station =
                document.Root
                        .Element("Setup")
                        .Element("Line")
                        .Element("Station");

            IEnumerable<XElement> headSteps = station.Elements("HeadStep");

            foreach (XElement headStep in headSteps)
            {
                XElement position = headStep.Element("Position");

                XElement pickup = headStep.Element("Pickup");

                SetupDetail detail = new SetupDetail();

                detail.MachineName = station.Attribute("Name").Value;
                detail.PartNumber = position.Attribute("Component").Value;
                detail.ReferenceDesignator = position.Attribute("ReferenceDesignator").Value;
                detail.Table = pickup.Attribute("Table").Value;
                detail.Track = pickup.Attribute("Track").Value;
                detail.FeederType = pickup.Attribute("FeederType").Value;

                setupDetails.Add(detail);
            }
            return setupDetails;
        }

    }
}
