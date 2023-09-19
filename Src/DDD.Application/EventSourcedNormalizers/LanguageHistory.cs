using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DDD.Domain.Core.Events;

namespace DDD.Application.EventSourcedNormalizers
{
  public  class LanguageHistory
    {
        public static IList<LanguageHistoryData> HistoryData { get; set; }

        public static IList<LanguageHistoryData> ToJavaScriptBookHistory(IList<StoredEvent> storedEvents)
        {
            HistoryData = new List<LanguageHistoryData>();
            //LanguageHistoryDeserializer(storedEvents);

            var sorted = HistoryData.OrderBy(c => c.When);
            var list = new List<LanguageHistoryData>();
            var last = new LanguageHistoryData();

            foreach (var change in sorted)
            {
                var jsSlot = new LanguageHistoryData
                {
                    LanguageId = change.LanguageId == Guid.Empty.ToString() || change.LanguageId == last.LanguageId
                        ? ""
                        : change.LanguageId,
                    Name = string.IsNullOrWhiteSpace(change.Name) || change.Name == last.Name
                        ? ""
                        : change.Name,
                    CodeAlpha2 = string.IsNullOrWhiteSpace(change.CodeAlpha2) || change.CodeAlpha2 == last.CodeAlpha2
                        ? ""
                        : change.CodeAlpha2,
                    CodeAlpha3 = string.IsNullOrWhiteSpace(change.CodeAlpha3) || change.CodeAlpha3 == last.CodeAlpha3
                        ? ""
                        : change.CodeAlpha3,
                    Code = string.IsNullOrWhiteSpace(change.Code) || change.Code == last.Code
                        ? ""
                        : change.Code,
              
                  
                    Action = string.IsNullOrWhiteSpace(change.Action) ? "" : change.Action,
                    When = change.When,
                    Who = change.Who
                };

                list.Add(jsSlot);
                last = change;
            }
            return list;
        }
      
    }
}
