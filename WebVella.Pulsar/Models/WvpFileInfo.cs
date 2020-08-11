using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebVella.Pulsar.Components;

namespace WebVella.Pulsar.Models
{
	public class WvpFileInfo
	{
		public long Id { get; set; }

		public DateTime LastModified { get; set; }

		public string Name { get; set; }

		public long Size { get; set; }

		public string ContentType { get; set; }

		public string Url { get; set; }

		public string Path { get; set; }

		public string ServerTempPath { get; set; }

		public string Status { get; set; } = "";

		public long ProgressMax = 100;

		public long ProgressValue = 0;


	}
}
