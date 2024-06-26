﻿using System;
using System.Collections.Generic;

namespace GogLibrary.Models
{
    public class StorePageResult
    {
        public class ProductDetails
        {
            public class SluggedName
            {
                public string name;
                public string slug;
            }

            public class Feature
            {
                public string name;
                public string id;
            }

            public List<SluggedName> genres;
            public List<SluggedName> tags;
            public List<SluggedName> gameTags;
            public List<Feature> features;
            public string publisher;
            public List<SluggedName> developers;
            public DateTime? globalReleaseDate;
            public string id;
            public string galaxyBackgroundImage;
            public string backgroundImage;
            public string boxArtImage;
            public string image;
            public int size;
        }

        public ProductDetails cardProduct;
    }
}
