using System;


namespace DDD.Application.EventSourcedNormalizers
{
  public  class CommunityHistoryData
    {
        public string Action { get; set; }
        public string When { get; set; }
        public string Who { get; set; }
        public string Id { get; set; }
        public string CommunityName { get; set; }
        public bool Status { get; set; }
    }
}
