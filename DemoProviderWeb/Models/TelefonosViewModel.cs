using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoProviderWeb.Models
{
    public class TelefonosViewModel
    {
        public int id { get; set; }
        public String Nombre { get; set; }
        public String Numero { get; set; }

        public static TelefonosViewModel FromListItem (ListItem item)
        {
            var data = new TelefonosViewModel();
            var id = item["ID"].ToString();
            int ido = 0;
            int.TryParse(id, out ido);
            data.id = ido;
            data.Nombre = item["Title"].ToString();
            data.Numero = item["Numero"].ToString();

            return data;
        }
    }
}