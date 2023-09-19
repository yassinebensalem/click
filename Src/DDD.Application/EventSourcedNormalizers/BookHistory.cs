using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DDD.Domain.Core.Events;

namespace DDD.Application.EventSourcedNormalizers
{
    public class BookHistory
    {
        public static IList<BookHistoryData> HistoryData { get; set; }

        public static IList<BookHistoryData> ToJavaScriptBookHistory(IList<StoredEvent> storedEvents)
        {
            HistoryData = new List<BookHistoryData>();
            BookHistoryDeserializer(storedEvents);

            var sorted = HistoryData.OrderBy(c => c.When);
            var list = new List<BookHistoryData>();
            var last = new BookHistoryData();

            foreach (var change in sorted)
            {
                var jsSlot = new BookHistoryData
                {
                    Id = change.Id == Guid.Empty.ToString() || change.Id == last.Id
                        ? ""
                        : change.Id,
                    Title = string.IsNullOrWhiteSpace(change.Title) || change.Title == last.Title
                        ? ""
                        : change.Title,
                    PageNumbers = string.IsNullOrWhiteSpace(change.PageNumbers) || change.PageNumbers == last.PageNumbers
                        ? ""
                        : change.PageNumbers,
                    CoverPath = string.IsNullOrWhiteSpace(change.CoverPath) || change.CoverPath == last.CoverPath
                        ? ""
                        : change.CoverPath,
                    Price = string.IsNullOrWhiteSpace(change.Price) || change.Price == last.Price
                        ? ""
                        : change.Price,
                    Description = string.IsNullOrWhiteSpace(change.Description) || change.Description == last.Description
                        ? ""
                        : change.Description,
                    PublicationDate = string.IsNullOrWhiteSpace(change.PublicationDate) || change.PublicationDate == last.PublicationDate
                        ? ""
                        : change.PublicationDate.Substring(0, 10),
                    ISBN = string.IsNullOrWhiteSpace(change.ISBN) || change.ISBN == last.ISBN
                        ? ""
                        : change.ISBN,
                    ISSN = string.IsNullOrWhiteSpace(change.ISSN) || change.ISSN == last.ISSN
                        ? ""
                        : change.ISSN,
                    EISBN = string.IsNullOrWhiteSpace(change.EISBN) || change.EISBN == last.EISBN
                        ? ""
                        : change.EISBN,
                    PDFPath = string.IsNullOrWhiteSpace(change.PDFPath) || change.PDFPath == last.PDFPath
                        ? ""
                        : change.PDFPath,
                    IsFree = change.IsFree,
                    AuthorId = string.IsNullOrWhiteSpace(change.AuthorId) || change.AuthorId == last.AuthorId
                        ? ""
                        : change.AuthorId,
                    PublisherId = string.IsNullOrWhiteSpace(change.PublisherId) || change.PublisherId == last.PublisherId
                        ? ""
                        : change.PublisherId,
                    CategoryId = change.CategoryId == Guid.Empty|| change.CategoryId == last.CategoryId
                        ? Guid.Empty
                        : change.CategoryId,
                    CountryId = change.CountryId == Guid.Empty || change.CountryId == last.CountryId
                        ? Guid.Empty
                        : change.CountryId,
                    LanguageId = change.LanguageId == Guid.Empty || change.LanguageId == last.LanguageId
                        ? Guid.Empty
                        : change.LanguageId,
                    Action = string.IsNullOrWhiteSpace(change.Action) ? "" : change.Action,
                    When = change.When,
                    Who = change.Who
                };

                list.Add(jsSlot);
                last = change;
            }
            return list;
        }

        private static void BookHistoryDeserializer(IEnumerable<StoredEvent> storedEvents)
        {
            foreach (var e in storedEvents)
            {
                var slot = new BookHistoryData();
                dynamic values;

                switch (e.MessageType)
                {
                    case "BookRegisteredEvent":
                        values = JsonSerializer.Deserialize<Dictionary<string, string>>(e.Data);
                        slot.PublicationDate = values["PublicationDate"];
                        slot.Title = values["Title"];
                        slot.PageNumbers = values["PageNumbers"];
                        slot.CoverPath = values["CoverPath"];
                        slot.Price = values["Price"];
                        slot.Description = values["Description"];
                        slot.ISSN = values["ISSN"];
                        slot.ISBN = values["ISBN"];
                        slot.EISBN = values["EISBN"];
                        slot.PDFPath = values["PDFPath"];
                        slot.IsFree = values["IsFree"];
                        slot.AuthorId = values["AuthorId"];
                        slot.PublisherId = values["PublisherId"];
                        slot.CategoryId = values["CategoryId"];
                        slot.CountryId = values["CountryId"];
                        slot.LanguageId = values["LanguageId"];
                        slot.Action = "Registered";
                        slot.When = values["Timestamp"];
                        slot.Id = values["Id"];
                        slot.Who = e.User;
                        break;
                    case "BookUpdatedEvent":
                        values = JsonSerializer.Deserialize<Dictionary<string, string>>(e.Data);
                        slot.PublicationDate = values["PublicationDate"];
                        slot.Title = values["Title"];
                        slot.PageNumbers = values["PageNumbers"];
                        slot.CoverPath = values["CoverPath"];
                        slot.Price = values["Price"];
                        slot.Description = values["Description"];
                        slot.ISSN = values["ISSN"];
                        slot.ISBN = values["ISBN"];
                        slot.EISBN = values["EISBN"];
                        slot.PDFPath = values["PDFPath"];
                        slot.IsFree = values["IsFree"];

                        slot.AuthorId = values["AuthorId"];
                        slot.PublisherId = values["PublisherId"];
                        slot.CategoryId = values["CategoryId"];
                        slot.CountryId = values["CountryId"];
                        slot.LanguageId = values["LanguageId"];
                        slot.Action = "Updated";
                        slot.When = values["Timestamp"];
                        slot.Id = values["Id"];
                        slot.Who = e.User;
                        break;
                    case "BookRemovedEvent":
                        values = JsonSerializer.Deserialize<Dictionary<string, string>>(e.Data);
                        slot.Action = "Removed";
                        slot.When = values["Timestamp"];
                        slot.Id = values["Id"];
                        slot.Who = e.User;
                        break;
                }
                HistoryData.Add(slot);
            }
        }
    }
}
