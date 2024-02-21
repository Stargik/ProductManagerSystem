using System;
using BLL.Models;
using DAL.Entities;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces
{
	public interface IExportService<T> where T : BaseEntity
	{
		public Task WriteToAsync(Stream stream, IEnumerable<T> entities, PortType portType = PortType.Default);
	}
}

