using System;
namespace BLL.Models
{
	public class EmailAttachmentModel
	{
		public byte[] File { get; set; }
		public string FileName { get; set; }
		public string FileType { get; set; }
	}
}

