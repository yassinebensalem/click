using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using DDD.Application.ILogics;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Syncfusion.EJ2.PdfViewer;

namespace MipLivrelStore.Controllers
{
    public class BookViewerController : Controller
    {
        private IMemoryCache _cache;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IBookService _bookService;
        private readonly IFileManagerLogic _fileManagerLogic;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILibraryService _libraryService;
        private readonly IPromotionService _promotionService;

        public BookViewerController(IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment,
            IBookService bookService, IFileManagerLogic fileManagerLogic, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ILibraryService libraryService,
            IPromotionService promotionService)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
            _bookService = bookService;
            _fileManagerLogic = fileManagerLogic;
            _userManager = userManager;
            _signInManager = signInManager;
            _libraryService = libraryService;
            _promotionService = promotionService;
            _libraryService = libraryService;
        }
        public IActionResult Default()
        {
            return View();
        }

        public IActionResult Custom()
        {
            return View();
        }

        public IActionResult RTL()
        {
            return View();
        }

        public IActionResult BookExample()
        {
            return PartialView("BookViewer_PartialView");
        }

        //[HttpGet]
        //public IActionResult LoadBook(string _BookId)
        //{
        //    var bookVM = _bookService.GetBookById(Guid.Parse(_BookId));

        //    ViewBag.BookId = bookVM.PDFPath;
        //    return PartialView("BookViewer_PartialView");
        //}

        [HttpGet]
        public IActionResult LoadBookPDFContent(string _BookId)
        {
            var bookVM = _bookService.GetBookById(Guid.Parse(_BookId));
            ViewBag.BookId = _BookId;
            ViewBag.PDFPath = bookVM.PDFPath;
            return PartialView("PDFJS_BookViewer_PartialView");
        }
        #region PDFJS
        [HttpGet]
        [Route("api/[controller]/GetPDF")]
        public IActionResult GetPDF(string document)
        {
            MemoryStream stream = new MemoryStream();
            byte[] bytes = GetDocumentPath2(document.Split('_')[1]);
            stream = new MemoryStream(bytes);
            MemoryStream dolly = new MemoryStream(stream.ToArray());
            var reader = new PdfReader(dolly);
            Document sourceDocument = new Document(reader.GetPageSizeWithRotation(1));
            MemoryStream target = new MemoryStream();
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage = null;
            sourceDocument.Open();

            pdfCopyProvider = new PdfCopy(sourceDocument, target);
            sourceDocument.Open();
            int pagesToShow = reader.NumberOfPages;
            if (_signInManager.IsSignedIn(User))
            {
                var bookId = document.Split('_')[0];
                var freeBooks = _promotionService.GetFreeBooks(0, 50).ToList();
                bool isFree = freeBooks.Exists(x => x.Id.ToString() == bookId);
                var appUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
                var libraryList = _libraryService.GetLibrariesByUserId(appUser.Id).ToList();
                if (libraryList != null)
                {
                    if (!libraryList.Any(lib => lib.BookId.ToString().Equals(bookId)))
                    {
                        pagesToShow = Math.Min(8, reader.NumberOfPages);
                    }
                }
            }
            for (int i = 1; i <= pagesToShow; i++)
            {
                importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                pdfCopyProvider.AddPage(importedPage);
            }
            sourceDocument.Close();
            reader.Close();

            byte[] page = target.ToArray();
            string temp_inBase64 = Convert.ToBase64String(page);

            return Content(temp_inBase64);

        }
        #endregion PDFJS

        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/Load")]
        public IActionResult Load([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            MemoryStream stream = new MemoryStream();
            var jsonObject = JsonConverterstring(responseData);
            object jsonResult = new object();
            if (jsonObject != null && jsonObject.ContainsKey("document"))
            {
                if (bool.Parse(jsonObject["isFileName"]))
                {
                    byte[] bytes = GetDocumentPath2(jsonObject["document"].Split('_')[1]);
                    stream = new MemoryStream(bytes);
                }
                else
                {
                    byte[] bytes = Convert.FromBase64String(jsonObject["document"]);
                    stream = new MemoryStream(bytes);
                }
            }

            jsonResult = pdfviewer.Load(stream, jsonObject);
            if (_signInManager.IsSignedIn(User))
            {
                string bookId = String.Empty;
                if (jsonObject != null && jsonObject.ContainsKey("document"))
                {
                    bookId = jsonObject["document"].Split('_')[0];
                }
                else if (jsonObject != null && jsonObject.ContainsKey("documentId"))
                {
                    bookId = jsonObject["documentId"].Split('_')[0];
                }
                var freeBooks = _promotionService.GetFreeBooks(0, 50).ToList();
                bool isFree = freeBooks.Exists(x => x.Id.ToString() == bookId);
                if (!isFree)
                {
                    var appUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
                    var libraryList = _libraryService.GetLibrariesByUserId(appUser.Id).ToList();
                    if (libraryList == null || !libraryList.Any(lib => lib.BookId.ToString().Equals(bookId)))
                    {
                        var _pageSizes = (Dictionary<string, System.Drawing.SizeF>)(jsonResult.GetType().GetProperty("pageSizes").GetValue(jsonResult));
                        jsonResultDTO jsonResultDto = new jsonResultDTO()
                        {
                            hashId = (string)jsonResult.GetType().GetProperty("hashId").GetValue(jsonResult),
                            uniqueId = (string)jsonResult.GetType().GetProperty("uniqueId").GetValue(jsonResult),
                            pageCount = 8,
                            documentLiveCount = (int)jsonResult.GetType().GetProperty("documentLiveCount").GetValue(jsonResult),
                            pageSizes = _pageSizes.Take(8).ToDictionary(x => x.Key, x => x.Value)
                        };
                        return Content(JsonConvert.SerializeObject(jsonResultDto));
                    }
                }

                return Content(JsonConvert.SerializeObject(jsonResult));

            }
            return this.Content("This content is not available");
        }

        private Byte[] GetDocumentPath2(string document)
        {
            return _fileManagerLogic.Get(document, "bookspdf").Result;
        }

        public Dictionary<string, string> JsonConverterstring(jsonObjects results)
        {
            Dictionary<string, object> resultObjects = new Dictionary<string, object>();
            resultObjects = results.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(results, null));
            var emptyObjects = (from kv in resultObjects
                                where kv.Value != null
                                select kv).ToDictionary(kv => kv.Key, kv => kv.Value);
            Dictionary<string, string> jsonResult = emptyObjects.ToDictionary(k => k.Key, k => k.Value.ToString());
            return jsonResult;
        }

        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/RenderPdfPages")]
        public IActionResult RenderPdfPages([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonObject = JsonConverterstring(responseData);
            object jsonResult = pdfviewer.GetPage(jsonObject);

            //UpdateLibraryVM updateLibraryVM = new UpdateLibraryVM();
            //var _currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            //updateLibraryVM.UserId = _currentUser.Id;
            //_libraryService.UpdateCurrentPage(updateLibraryVM);

            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/RenderAnnotationComments")]
        public IActionResult RenderAnnotationComments([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonObject = JsonConverterstring(responseData);
            object jsonResult = pdfviewer.GetAnnotationComments(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/Unload")]
        public IActionResult Unload([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonObject = JsonConverterstring(responseData);
            pdfviewer.ClearCache(jsonObject);
            return this.Content("Document cache is cleared");
        }

        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/RenderThumbnailImages")]
        public IActionResult RenderThumbnailImages([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonObject = JsonConverterstring(responseData);
            object result = pdfviewer.GetThumbnailImages(jsonObject);
            return Content(JsonConvert.SerializeObject(result));
        }

        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/Bookmarks")]
        public IActionResult Bookmarks([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonObject = JsonConverterstring(responseData);
            object jsonResult = pdfviewer.GetBookmarks(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/ImportAnnotations")]
        public IActionResult ImportAnnotations([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            string jsonResult = string.Empty;
            object JsonResult;
            if (jsonObject != null && jsonObject.ContainsKey("fileName"))
            {
                string documentPath = GetDocumentPath(jsonObject["fileName"]);
                if (!string.IsNullOrEmpty(documentPath))
                {
                    jsonResult = System.IO.File.ReadAllText(documentPath);
                }
                else
                {
                    return this.Content(jsonObject["document"] + " is not found");
                }
            }
            else
            {
                string extension = Path.GetExtension(jsonObject["importedData"]);
                if (extension != ".xfdf")
                {
                    JsonResult = pdfviewer.ImportAnnotation(jsonObject);
                    return Content(JsonConvert.SerializeObject(JsonResult));
                }
                else
                {
                    string documentPath = GetDocumentPath(jsonObject["importedData"]);
                    if (!string.IsNullOrEmpty(documentPath))
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(documentPath);
                        jsonObject["importedData"] = Convert.ToBase64String(bytes);
                        JsonResult = pdfviewer.ImportAnnotation(jsonObject);
                        return Content(JsonConvert.SerializeObject(JsonResult));
                    }
                    else
                    {
                        return this.Content(jsonObject["document"] + " is not found");
                    }
                }
            }
            return Content(jsonResult);
        }

        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/RenderPdfTexts")]
        public IActionResult RenderPdfTexts([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object result = pdfviewer.GetDocumentText(jsonObject);
            return Content(JsonConvert.SerializeObject(result));
        }

        private string GetDocumentPath(string document)
        {
            string documentPath = string.Empty;
            if (!System.IO.File.Exists(document))
            {
                string basePath = _hostingEnvironment.WebRootPath;
                string dataPath = string.Empty;
                dataPath = basePath + @"/BooKViewer/";
                if (System.IO.File.Exists(dataPath + document))
                    documentPath = dataPath + document;
            }
            else
            {
                documentPath = document;
            }
            return documentPath;
        }

    }

    public class jsonObjects
    {

        public string document { get; set; }
        public string password { get; set; }
        public int zoomFactor { get; set; }
        public bool isFileName { get; set; }
        public int xCoordinate { get; set; }
        public int yCoordinate { get; set; }
        public int pageNumber { get; set; }
        public int tileXcount { get; set; }
        public int tileYcount { get; set; }
        public string extraText { get; set; }
        public string documentId { get; set; }
        public string hashId { get; set; }
        public float sizeX { get; set; }
        public float sizeY { get; set; }
        public int startPage { get; set; }
        public int endPage { get; set; }
        public string stampAnnotations { get; set; }
        public string textMarkupAnnotations { get; set; }
        public string stickyNotesAnnotation { get; set; }
        public string shapeAnnotations { get; set; }
        public string measureShapeAnnotations { get; set; }
        public string action { get; set; }
        public int pageStartIndex { get; set; }
        public int pageEndIndex { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
        public string elementId { get; set; }
        public string pdfAnnotation { get; set; }
        public string importPageList { get; set; }
        public string annotationDataFormat { get; set; }
        public string uniqueId { get; set; }
        public string data { get; set; }
        public float viwePortWidth { get; set; }
        public float viewportHeight { get; set; }
        public int tilecount { get; set; }
        public bool isCompletePageSizeNotReceived { get; set; }
        public string freeTextAnnotation { get; set; }
        public string signatureData { get; set; }
        public string fieldsData { get; set; }
        public string documentLiveCount { get; set; }
    }

    public class jsonResultDTO
    {
        public string hashId { get; set; }
        public string uniqueId { get; set; }
        public int pageCount { get; set; }
        public object pageSizes { get; set; }
        public int documentLiveCount { get; set; }
    }
}
