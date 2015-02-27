﻿using SimpleBlog.Infrastructure;
using SimpleBlog.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlog.Areas.Admin.ViewModels
{
   
    public class TagCheckbox
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
    
    public class PostsIndex
    {
        public PagedData<Post> Posts { get; set; }
    }

    public class PostsForm
    {
        public bool isNew { get; set; }
        public int? PostId{ get; set; }

        [Required, MaxLength(128)]
        public string Title{ get; set; }

        [Required, MaxLength(128)]
        public string Slug{ get; set; }
        [Required, DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public IList<TagCheckbox> Tags { get; set; }
    }
}