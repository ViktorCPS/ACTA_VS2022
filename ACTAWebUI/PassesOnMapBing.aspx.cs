using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using Common;
using TransferObjects;
using Util;

namespace ACTAWebUI
{
    public partial class PassesOnMapBing : System.Web.UI.Page
    {
        const string pageName = "PassesOnMapBingPage";
        string[] centerLoc = new string[2];

        protected void Page_Load(object sender, EventArgs e)     
        {
             string Locations = GetLocations();
             Literal1.Text= @"
                  <script type='text/javascript' src='http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0'>
                  </script>
                  <script type='text/javascript'>
                       var  map = new Microsoft.Maps.Map(document.getElementById('myMap'), {credentials: 'Aiwi2QQaV3-yGTfHYC7avb8egv_0RcPe1e4wqOv0hemVqjLOnE0_jXAlL9WM4041' });
         
                       function GetMap() {
                            map.entities.clear();
                            " + Locations+ @"
                            
                            function ZoomIn(e){
                                 if (e.targetType == 'pushpin'){
                                       var location = e.target.getLocation();
                                       map.setView({
                                            zoom:16,
                                            center: location
                                       });
                                 }
                            }
                        }

        </script>";
        }

        private string GetLocations()
        {
            string Locations = "";

            PassesAdditionalInfo pass = new PassesAdditionalInfo();
            PassesAdditionalInfoTO searchPass = new PassesAdditionalInfoTO();
            List<PassesAdditionalInfoTO> passesOnMap = pass.Search(searchPass);
            int i = 0;
            if (passesOnMap.Count > 0)
            {
                foreach (PassesAdditionalInfoTO p in passesOnMap)
                {
                    string longilati = p.GpsData;
                    string[] separate = longilati.Split('|');
                    if (i == 0)
                    {
                        centerLoc = separate;
                    }

                        Locations += "var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(" +
                        separate[0] + ", " + separate[1] +
                        "), null);Microsoft.Maps.Events.addHandler(pushpin, 'mouseup', ZoomIn);map.entities.push(pushpin);";

                       i++;
                }
            }
            return Locations;
        }

        
    }
}
