using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DDD.Domain.Core.Events;
namespace DDD.Application.EventSourcedNormalizers
{
    class CategoryHistory
    {
        public static IList<CategoryHistoryData> HistoryData { get; set; }

        public static IList<CategoryHistoryData> ToJavaScriptCategoryHistory(IList<StoredEvent> storedEvents)
        {
            HistoryData = new List<CategoryHistoryData>();
            CategoryHistoryDeserializer(storedEvents);

            var sorted = HistoryData.OrderBy(c => c.When);
            var list = new List<CategoryHistoryData>();
            var last = new CategoryHistoryData();

            foreach (var change in sorted)
            {
                var jsSlot = new CategoryHistoryData
                {
                   Id = change.Id == Guid.Empty.ToString() || change.Id == last.Id
                        ? ""
                        : change.Id,
                    CategoryName = string.IsNullOrWhiteSpace(change.CategoryName) || change.CategoryName == last.CategoryName
                        ? ""
                        : change.CategoryName,
                    Status = change.Status,

                    Action = string.IsNullOrWhiteSpace(change.Action) ? "" : change.Action,
                    When = change.When,
                    Who = change.Who
                };

                list.Add(jsSlot);
                last = change;
            }
            return list;
        }

        private static void CategoryHistoryDeserializer(IEnumerable<StoredEvent> storedEvents)
        {
            foreach (var e in storedEvents)
            {
                var slot = new CategoryHistoryData();
                dynamic values;

                switch (e.MessageType)
                {
                    case "CategoryRegisteredEvent":
                        values = JsonSerializer.Deserialize<Dictionary<string, string>>(e.Data);
                       
                        slot.CategoryName = values["CategoryName"];

                        slot.Id = values["CategoryId"];
                        slot.Status = values["Status"];

                        slot.Who = e.User;
                        break;
                    case "CategoryUpdatedEvent":
                        values = JsonSerializer.Deserialize<Dictionary<string, string>>(e.Data);
                        slot.CategoryName = values["CategoryName"];

                        slot.Id = values["CategoryId"];
                        slot.Status = values["Status"];

                        slot.Who = e.User;
                        break;
                    case "CategoryRemovedEvent":
                        values = JsonSerializer.Deserialize<Dictionary<string, string>>(e.Data);
                        slot.Action = "Removed";
                        slot.When = values["Timestamp"];
                        slot.Id = values["CategoryId"];
                        slot.Who = e.User;
                        break;
                }
                HistoryData.Add(slot);
            }
        }
    }
}
