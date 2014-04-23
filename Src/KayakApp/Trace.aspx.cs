using System;
using System.Collections.Generic;
using System.Linq;
using AppKickStart.Common.Logging;

namespace IKayak
{
    public partial class Trace : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string logId;
                ITraceLog[] allEvents;

                if ((logId = Request.QueryString["Id"]) != null)
                {
                    allEvents = InMemoryLogEvents.Instance.FetchEventsBy(logId);
                    if (!allEvents.Any())
                    {
                        ShowAllEvents();
                        return;
                    }
                    
                    ITraceLog request = allEvents[0];
                    var list = request.Events;
                    if (list.Count > 0)
                        log.Visible = true;
                    rtpLogLines.DataSource = list;
                    rtpLogLines.DataBind();

                    requestDetails.DataSource = new RequestDictionary(request).Properties;
                    requestDetails.DataBind();
                }
                else
                {
                    ShowAllEvents();
                }
            }
        }

        private void ShowAllEvents()
        {
            ITraceLog[] allEvents;
            filter.Visible = true;
            allEvents = InMemoryLogEvents.Instance.FetchEventsBy(null);
            rptResults.DataSource = allEvents;
            rptResults.DataBind();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            filter.Visible = true;
            var results = InMemoryLogEvents.Instance.FetchEventsBy(txtFilterIp.Text);
            rptResults.DataSource = results;
            rptResults.DataBind();
        }

        public class Property
        {
            public string PropertyName { get; set; }
            public string PropertyValue { get; set; }
        }

        public class RequestDictionary
        {
            public List<Property> Properties { get; set; }

            public RequestDictionary(ITraceLog log)
            {
                Properties = new List<Property>();

                //var originalRequest = log.Request as BaseRequest;

                Properties.Add(new Property { PropertyName = "Ip Address", PropertyValue = log.ClientIp });
                Properties.Add(new Property { PropertyName = "Title", PropertyValue = log.Title });

                //if (originalRequest == null)
                //{
                //    return;
                //}

                //Properties.Add(new Property { PropertyName = "Security Token", PropertyValue = originalRequest.SecurityToken   });
                //Properties.Add(new Property { PropertyName = "Device", PropertyValue = originalRequest.Device });
                //Properties.Add(new Property { PropertyName = "Latitude", PropertyValue = originalRequest.Latitude });
                //Properties.Add(new Property { PropertyName = "Longitude", PropertyValue = originalRequest.Longitude });
            }
        }
    }
}