using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDevMobile.ViewModels
{
	public class UserInfo
	{
		public string DisplayName { get; set; }

		public List<Team> Teams { get; set; }
		public List<Project> Projects { get; set; }
		public List<Queue> Queues { get; set; }

		public class Team { public int ID { get; set; } public string Name { get; set; } }
		public class Project { public int ID { get; set; } public string Name { get; set; } }
		public class Queue { public int ID { get; set; } public string Name { get; set; } public int WorkItemCount { get; set; } public bool Watching { get; set; } }
	}
}
