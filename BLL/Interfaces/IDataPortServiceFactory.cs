using System;
using BLL.Models;
using DAL.Entities;

namespace BLL.Interfaces
{
	public interface IDataPortServiceFactory<T> where T : BaseEntity
	{
		IExportService<T> GetExportService(string contentType);
	}
}

