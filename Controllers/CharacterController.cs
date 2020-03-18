using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.Extensions.Configuration;
using Terra.Data;
using Terra.Models;
using Terra.Extensions;

namespace Terra.Controllers
{
    [Authorize]
    public class CharacterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IConfiguration _configuration;

        public CharacterController(
            ApplicationDbContext context, 
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public static IConfiguration configuration { get; private set; }

        // GET: Character
        [Authorize(Roles="Admin, Manager")]
        public async Task<IActionResult> Index(string currentFilter, string sortOrder, string searchString, int? page)
        {
            ViewData["NameSort"] = String.IsNullOrEmpty(sortOrder) ? "Name" : "";
            ViewData["OriginSort"] = String.IsNullOrEmpty(sortOrder) ? "Origin" : "";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            IQueryable<Character> characters = _context.Characters
                                                .Include(c => c.Pictures)
                                                .Include(c => c.Stats)
                                                .Include(c => c.DatingProfile)
                                                .ThenInclude(c => c.Responses);
            characters = characters.OrderByDescending(c => c.Origin ).ThenBy(c => c.Name);

            if (!String.IsNullOrEmpty(searchString))
            {
                characters = characters.Where(c => c.Name.Contains(searchString.First().ToString().ToUpper() + searchString.Substring(1)) || c.Origin.Contains(searchString));
            }

            int pageSize = 14;
            return View(await PaginatedList<Character>.CreateAsync(characters.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Character/Details/5
        [Authorize(Roles="Admin, Manager")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characters = await _context.Characters
                                    .Include(c => c.Pictures)
                                    .Include(c => c.Stats)
                                    .Include(c => c.DatingProfile)
                                    .ThenInclude(c => c.Responses)
                                    .SingleOrDefaultAsync(c => c.Id == id);

            if (characters == null)
            {
                return NotFound();
            }

            return View(characters);
        }

        // GET: Character/Create
        [Authorize(Roles="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Character/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Create(Character characters)
        {
            var account = _configuration["AzureStorageConfig:AccountName"];
            var key = _configuration["AzureStorageConfig:AccountKey"];
            var storageCredentials = new StorageCredentials(account, key);
            var cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = cloudBlobClient.GetContainerReference("images");
            await container.CreateIfNotExistsAsync();

            if (ModelState.IsValid)
            {
                _context.Add(characters);
            }
            await _context.SaveChangesAsync();

            var files = HttpContext.Request.Form.Files;
            var characterFromDb = _context.Characters.Find(characters.Id);

            //var picture = new Picture();
            //Picture pictureFromDb;

            // if (files.Count != 0) 
            // {
            //     for (var i = 0; i < files.Count; i++) {
            //         picture.CharacterId = characterFromDb.Id.ToString();
            //         _context.Add(picture);
            //         pictureFromDb = _context.Pictures.Find(picture.PictureId);

            //         var extension = Path.GetExtension(files[i].FileName);
            //         var newBlob = container.GetBlockBlobReference(pictureFromDb.PictureId.ToString() + extension);

            //         using (var filestream = new MemoryStream())
            //         {
            //             files[i].CopyTo(filestream);
            //             filestream.Position = 0;
            //             await newBlob.UploadFromStreamAsync(filestream);
            //         }
            //     }
            // }
        
            // var pictures = from p in _context.Pictures select p;
            // pictures = pictures.Where(i => i.CharacterId == characterFromDb.Id.ToString());
            // characterFromDb.Pictures = pictures.ToList();

            TempData["ClassName"] = "bg-success";
            TempData["ContainerHeight"] = "height: 50px; border-radius: 5px;";
            TempData["Message"] = "Character added!";
            TempData["Status"] = "Success";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Character/Edit/5
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters.SingleOrDefaultAsync(m => m.Id == id);

            if (character == null)
            {
                return NotFound();
            }
            return View(character);
        }

        // POST: Character/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Edit(Guid id, Character characters)
        {
            var account = _configuration["AzureStorageConfig:AccountName"];
            var key = _configuration["AzureStorageConfig:AccountKey"];
            var storageCredentials = new StorageCredentials(account, key);
            var cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = cloudBlobClient.GetContainerReference("images");
            await container.CreateIfNotExistsAsync();

            if (id != characters.Id)
            {
                return NotFound();
            }
            var characterFromDb = await _context.Characters.SingleOrDefaultAsync(c => c.Id == id);

            if (ModelState.IsValid)
            {
                try
                {
                    // _context.Update(characters);
                    // await _context.SaveChangesAsync();
                    characterFromDb.Name = characters.Name;
                    characterFromDb.Age = characters.Age;
                    characterFromDb.Race = characters.Race;
                    characterFromDb.Gender = characters.Gender;
                    characterFromDb.Job = characters.Job;
                    characterFromDb.Height = characters.Height;
                    characterFromDb.Weight = characters.Weight;
                    characterFromDb.Origin = characters.Origin;
                    characterFromDb.Description = characters.Description;

                    var files = HttpContext.Request.Form.Files;

                    // if (characters.Picture != characterFromDb.Picture) 
                    // {
                    //     if (files.Count != 0) 
                    //     {
                    //         for (var i = 0; i < files.Count; i++) {
                    //             var extension = Path.GetExtension(files[i].FileName);
                    //             var newBlob = container.GetBlockBlobReference("Character-" + characters.Id + (i + 1).ToString() + extension);

                    //             using (var filestream = new MemoryStream())
                    //             {
                    //                 files[i].CopyTo(filestream);
                    //                 filestream.Position = 0;
                    //                 await newBlob.UploadFromStreamAsync(filestream);
                    //             }
                    //             if (i == 0) 
                    //             {
                    //                 characterFromDb.Picture = "https://mooglestorage.blob.core.windows.net/images/Character-" + characters.Id + (i + 1).ToString() + extension;
                    //             }
                    //             if (i == 1) 
                    //             {
                    //                 characterFromDb.Picture2 = "https://mooglestorage.blob.core.windows.net/images/Character-" + characters.Id + (i + 1).ToString() + extension;
                    //             }
                    //             if (i == 2) 
                    //             {
                    //                 characterFromDb.Picture3 = "https://mooglestorage.blob.core.windows.net/images/Character-" + characters.Id + (i + 1).ToString() + extension;
                    //             }
                    //             if (i == 3) 
                    //             {
                    //                 characterFromDb.Picture4 = "https://mooglestorage.blob.core.windows.net/images/Character-" + characters.Id + (i + 1).ToString() + extension;
                    //             }
                    //             if (i == 4) 
                    //             {
                    //                 characterFromDb.Picture5 = "https://mooglestorage.blob.core.windows.net/images/Character-" + characters.Id + (i + 1).ToString() + extension;
                    //             }
                    //         }
                    //     }
                    //}
                    TempData["ClassName"] = "bg-success";
                    TempData["ContainerHeight"] = "height: 50px; border-radius: 5px;";
                    TempData["Message"] = "Character updated!";
                    TempData["Status"] = "Success";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharactersExists(characters.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit));
            }
            return View(characterFromDb);
        }

        // GET: Character/Delete/5
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characters = await _context.Characters
                .SingleOrDefaultAsync(m => m.Id == id);
            if (characters == null)
            {
                return NotFound();
            }

            return View(characters);
        }

        // POST: Character/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var characters = await _context.Characters.SingleOrDefaultAsync(m => m.Id == id);
            _context.Characters.Remove(characters);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharactersExists(Guid id)
        {
            return _context.Characters.Any(e => e.Id == id);
        }
    }
}
