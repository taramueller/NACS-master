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

namespace NACSMagazine
{
	/// <summary>
	/// Represents a content item of type <see cref="Article"/>.
	/// </summary>
	[RegisterContentTypeMapping(CONTENT_TYPE_NAME)]
	public partial class Article : IContentItemFieldsSource
	{
		/// <summary>
		/// Code name of the content type.
		/// </summary>
		public const string CONTENT_TYPE_NAME = "NACSMagazine.Article";


		/// <summary>
		/// Represents system properties for a content item.
		/// </summary>
		[SystemField]
		public ContentItemFields SystemFields { get; set; }


		/// <summary>
		/// Title.
		/// </summary>
		public string Title { get; set; }


		/// <summary>
		/// Locked.
		/// </summary>
		public bool Locked { get; set; }


		/// <summary>
		/// EditorPick.
		/// </summary>
		public bool EditorPick { get; set; }


		/// <summary>
		/// CoverStory.
		/// </summary>
		public bool CoverStory { get; set; }


		/// <summary>
		/// CurrentIssue.
		/// </summary>
		public bool CurrentIssue { get; set; }


		/// <summary>
		/// Image.
		/// </summary>
		public ContentItemAsset Image { get; set; }


		/// <summary>
		/// RollupImage.
		/// </summary>
		public ContentItemAsset RollupImage { get; set; }


		/// <summary>
		/// RollupImageURL.
		/// </summary>
		public string RollupImageURL { get; set; }


		/// <summary>
		/// LedeText.
		/// </summary>
		public string LedeText { get; set; }


		/// <summary>
		/// IssueDate.
		/// </summary>
		public DateTime IssueDate { get; set; }


		/// <summary>
		/// PageContent.
		/// </summary>
		public string PageContent { get; set; }


		/// <summary>
		/// PageContentTeaser.
		/// </summary>
		public string PageContentTeaser { get; set; }


		/// <summary>
		/// Authors.
		/// </summary>
		public IEnumerable<Author> Authors { get; set; }


		/// <summary>
		/// AuthorNames.
		/// </summary>
		public string AuthorNames { get; set; }


		/// <summary>
		/// MagazineSection.
		/// </summary>
		public string MagazineSection { get; set; }


		/// <summary>
		/// ContentCategory.
		/// </summary>
		public IEnumerable<TagReference> ContentCategory { get; set; }


		/// <summary>
		/// CategoryTags.
		/// </summary>
		public string CategoryTags { get; set; }
	}
}