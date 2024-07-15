//--------------------------------------------------------------------------------------------------
// <auto-generated>
//
//     This code was generated by code generator tool.
//
//     To customize the code use your own partial class. For more info about how to use and customize
//     the generated code see the documentation at https://docs.xperience.io/.
//
// </auto-generated>
//--------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using CMS.ContentEngine;
using CMS.Websites;

namespace NACSMagazine
{
	/// <summary>
	/// Represents a page of type <see cref="ArchivePage"/>.
	/// </summary>
	[RegisterContentTypeMapping(CONTENT_TYPE_NAME)]
	public partial class ArchivePage : IWebPageFieldsSource
	{
		/// <summary>
		/// Code name of the content type.
		/// </summary>
		public const string CONTENT_TYPE_NAME = "NACSMagazine.ArchivePage";


		/// <summary>
		/// Represents system properties for a web page item.
		/// </summary>
		[SystemField]
		public WebPageFields SystemFields { get; set; }


		/// <summary>
		/// Title.
		/// </summary>
		public string Title { get; set; }


		/// <summary>
		/// PageContent.
		/// </summary>
		public string PageContent { get; set; }


		/// <summary>
		/// Months.
		/// </summary>
		public string Months { get; set; }


		/// <summary>
		/// Years.
		/// </summary>
		public string Years { get; set; }


		/// <summary>
		/// SelectedMonth.
		/// </summary>
		public string SelectedMonth { get; set; }


		/// <summary>
		/// IssuesList.
		/// </summary>
		public IEnumerable<Issue> IssuesList { get; set; }


		/// <summary>
		/// SelectedYear.
		/// </summary>
		public string SelectedYear { get; set; }


		/// <summary>
		/// PageNumber.
		/// </summary>
		public int PageNumber { get; set; }


		/// <summary>
		/// PageSize.
		/// </summary>
		public int PageSize { get; set; }


		/// <summary>
		/// TotalPages.
		/// </summary>
		public int TotalPages { get; set; }
	}
}