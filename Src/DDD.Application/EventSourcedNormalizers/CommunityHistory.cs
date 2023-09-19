using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DDD.Domain.Core.Events;
namespace DDD.Application.EventSourcedNormalizers
{
    class CommunityHistory
    {
        public static IList<CommunityHistoryData> HistoryData { get; set; }

        public static IList<CommunityHistoryData> ToJavaScriptCommunityHistory(IList<StoredEvent> storedEvents)
        {
            HistoryData = new List<CommunityHistoryData>();
            CommunityHistoryDeserializer(storedEvents);

            var sorted = HistoryData.OrderBy(c => c.When);
            var list = new List<CommunityHistoryData>();
            var last = new CommunityHistoryData();

            foreach (var change in sorted)
            {
                var jsSlot = new CommunityHistoryData
                {
                   Id = change.Id == Guid.Empty.ToString() || change.Id == last.Id
                        ? ""
                        : change.Id,
                    CommunityName = string.IsNullOrWhiteSpace(change.CommunityName) || change.CommunityName == last.CommunityName
                        ? ""
                        : change.CommunityName,
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

        private static void CommunityHistoryDeserializer(IEnumerable<StoredEvent> storedEvents)
        {
            foreach (var e in storedEvents)
            {
                var slot = new CommunityHistoryData();
                dynamic values;

                switch (e.MessageType)
                {
                    case "CommunityRegisteredEvent":
                        values = JsonSerializer.Deserialize<Dictionary<string, string>>(e.Data);
                       
                        slot.CommunityName = values["CommunityName"];

                        slot.Id = values["CommunityId"];
                        slot.Status = values["Status"];

                        slot.Who = e.User;
                        break;
                    case "CommunityUpdatedEvent":
                        values = JsonSerializer.Deserialize<Dictionary<string, string>>(e.Data);
                        slot.CommunityName = values["CommunityName"];

                        slot.Id = values["CommunityId"];
                        slot.Status = values["Status"];

                        slot.Who = e.User;
                        break;
                    case "CommunityRemovedEvent":
                        values = JsonSerializer.Deserialize<Dictionary<string, string>>(e.Data);
                        slot.Action = "Removed";
                        slot.When = values["Timestamp"];
                        slot.Id = values["CommunityId"];
                        slot.Who = e.User;
                        break;
                }
                HistoryData.Add(slot);
            }
        }
    }
}
