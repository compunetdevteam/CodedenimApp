using System.Collections.Generic;

namespace CodedenimWebApp.Controllers.Api.ApiViewModel
{
    public class ModuleTrackVm
    {
        public string Message { get; set; }
        public List<ModuleVm> ModuleVms { get; set; }
    }

    public class ModuleVm
    {
        public int ModuleId { get; set; }
        public int CourseId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public int ExpectedTime { get; set; }
        public IEnumerable<TopicVm> Topics { get; set; }
    }
}