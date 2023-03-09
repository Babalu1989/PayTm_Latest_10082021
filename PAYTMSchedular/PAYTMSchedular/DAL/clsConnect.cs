using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SAP.Middleware.Connector;
using System.Configuration;


namespace PAYTMSchedular.DAL
{
    class  clsConnect : IDestinationConfiguration
    {
        public bool ChangeEventsSupported()
        {
            return false;
        }

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        public RfcConfigParameters GetParameters(string destinationName)
        {

            RfcConfigParameters parms = new RfcConfigParameters();
            try
            {
                if (destinationName.Equals("mySAPdestination"))
                {
                    //parms.Add(RfcConfigParameters.AppServerHost, "10.8.61.47");
                    //parms.Add(RfcConfigParameters.SystemNumber, "00");
                    //parms.Add(RfcConfigParameters.SystemID, "00");
                    //parms.Add(RfcConfigParameters.User, "sys_iss");
                    //parms.Add(RfcConfigParameters.Password, "123456");
                    //parms.Add(RfcConfigParameters.Client, "102");
                    //parms.Add(RfcConfigParameters.Language, "EN");
                    //parms.Add(RfcConfigParameters.MaxPoolSize, "1500");


                    /////////////////////////////////////////////////////////////////
                    //parms.Add(RfcConfigParameters.AppServerHost, "10.8.55.219");
                    //parms.Add(RfcConfigParameters.SystemNumber, "01");
                    //parms.Add(RfcConfigParameters.SystemID, "01");
                    //parms.Add(RfcConfigParameters.User, "sys_web");
                    //parms.Add(RfcConfigParameters.Password, "123456");
                    //parms.Add(RfcConfigParameters.Client, "100");
                    //parms.Add(RfcConfigParameters.Language, "EN");
                    //parms.Add(RfcConfigParameters.LogonGroup, "P92REL");
                    //parms.Add(RfcConfigParameters.Trace, "False");
                    ////parms.Add(RfcConfigParameters.MaxPoolSize, "1500");
                    //parms.Add(RfcConfigParameters.PeakConnectionsLimit, "1500");
                    ////////////////////////////////////////////////////////////

                    parms.Add(RfcConfigParameters.AppServerHost, "10.8.54.176");
                    parms.Add(RfcConfigParameters.SystemNumber, "01");
                    parms.Add(RfcConfigParameters.SystemID, "UP3");
                    parms.Add(RfcConfigParameters.User, "sys_iss");
                    parms.Add(RfcConfigParameters.Password, "123456");
                    parms.Add(RfcConfigParameters.Client, "100");
                    parms.Add(RfcConfigParameters.Language, "en");

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return parms;

        }
    }


}
