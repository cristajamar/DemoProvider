using DemoProviderWeb.Models;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoProviderWeb.Controllers
{
    public class HomeController : Controller
    {
        [SharePointContextFilter]
        public ActionResult Index()
        {
            User spUser = null;

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            var data = new List<TelefonosViewModel>();
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
 


                    var telefonosList = clientContext.Web.Lists.GetByTitle("Telefonos");
                    clientContext.Load(telefonosList);
                    clientContext.ExecuteQuery();

                    var query = new CamlQuery();
                    var telefonosItems = telefonosList.GetItems(query);
                    clientContext.Load(telefonosItems);
                    clientContext.ExecuteQuery();

                    foreach (var x in telefonosItems)
                    {
                        data.Add(TelefonosViewModel.FromListItem(x));
                    }
                }
            }

            return View(data);
        }

        public ActionResult Add()
        {
            return View(new TelefonosViewModel());
        }

        [HttpPost]
        public ActionResult Add(TelefonosViewModel model)
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            var data = new List<TelefonosViewModel>();
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    
                    var telefonosList = clientContext.Web.Lists.GetByTitle("Telefonos");
                    clientContext.Load(telefonosList);
                    clientContext.ExecuteQuery();

                    var listCreationInfo = new ListItemCreationInformation();

                    var item = telefonosList.AddItem(listCreationInfo);
                    item["Title"] = model.Nombre;
                    item["Numero"] = model.Numero;
                    item.Update();
                    clientContext.ExecuteQuery();
                    

                }
            }

            return RedirectToAction("Index", new {SPHostUrl= SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri});
        }

        public ActionResult Delete (int id)
        {
      
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    
                    var telefonosList = clientContext.Web.Lists.GetByTitle("Telefonos");       
                    var telefonosItem = telefonosList.GetItemById(id);

                    telefonosItem.DeleteObject();
                    clientContext.ExecuteQuery();

                }
            }

            return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }


        public ActionResult Update (int id)
        {

            User spUser = null;

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            TelefonosViewModel model = null;
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {

                    var telefonosList = clientContext.Web.Lists.GetByTitle("Telefonos");
                    clientContext.Load(telefonosList);
                    var telefonosItem = telefonosList.GetItemById(id);
                    clientContext.Load(telefonosItem);
                    clientContext.ExecuteQuery();

                    model = TelefonosViewModel.FromListItem(telefonosItem);

                }
            }

            return View(model);

        }

        [HttpPost]
        public ActionResult Update(TelefonosViewModel model)
        {
            User spUser = null;

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
      
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {

                    var telefonosList = clientContext.Web.Lists.GetByTitle("Telefonos");
                    var item = telefonosList.GetItemById(model.id);

                    item["Title"] = model.Nombre;
                    item["Numero"] = model.Numero;
                    item.Update();

                    clientContext.ExecuteQuery();
                }
            }

            return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
