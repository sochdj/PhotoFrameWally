using Dropbox.Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static System.Web.Hosting.HostingEnvironment;

namespace AngularJSWebApiEmpty.Controllers
{

    public class DropboxController : ApiController
    {
        DropboxClient dbx = new DropboxClient("92SlODg-f5kAAAAAAACfKIJV7iOS6ZqdDGfsltmoU6rAV-K7bIf9lrnfiarAKNvS");
        private String pathToImage;
        [HttpGet]
        public object GetFolders()
        {

            var task = Task.Run((Func<Task>)Run);
            task.Wait();

            return new {FileName = pathToImage};



        }
        private async Task Run()
        {
            var list = await dbx.Files.ListFolderAsync(string.Empty);


            List<String> files = new List<string>();
            foreach (var f in list.Entries.Where(i => i.IsFile))
            {
                files.Add(f.Name);
                Debug.WriteLine(f);
            }
            if (files.Count == 0) return;
            try
            {
                Random r = new Random();
                var index = r.Next(0, files.Count);

                using (var response = await dbx.Files.DownloadAsync("/"+files[index]))
                {
                   // String d= await response.GetContentAsStringAsync();
                   
                    var b = await response.GetContentAsStreamAsync();

                    String path = Path.Combine(MapPath("~/Images"), files[index]);
                    FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
                    b.CopyTo(file);
                    file.Close();
                    b.Close();

                    pathToImage = files[index];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


    }
}