﻿using Core.Services.Authenticate.WebAPI.Models;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Core.Services.Authenticate.WebAPI.Controller
{
    [EnableCors(origins: "*", headers: "*", methods: "*")] 
    public class DemoController : ApiController
    {
        [HttpPut]
        public void Register(WebAPIContext sharePointServiceContext)
        {
            WebAPIHelper.AddToCache(sharePointServiceContext);
        }

        [WebAPIContextFilter]
        [HttpGet]
        public IEnumerable<Item> GetItems()
        {
            using (var clientContext = WebAPIHelper.GetClientContext(ControllerContext))
            {
                if (clientContext != null)
                {
                    List demoList = clientContext.Web.Lists.GetByTitle("WebAPIDemo");
                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query></Query></View>";
                    ListItemCollection demoItems = demoList.GetItems(camlQuery);
                    clientContext.Load(demoItems);
                    clientContext.ExecuteQuery();

                    Item[] items = new Item[demoItems.Count];

                    int i = 0;
                    foreach (ListItem item in demoItems)
                    {
                        items[i] = new Item() { Id = item.Id, Title = item["Title"].ToString() };
                        i++;
                    }

                    return items;
                }
                else
                {
                    return new Item[0];
                }
            }
        }


    }
}
